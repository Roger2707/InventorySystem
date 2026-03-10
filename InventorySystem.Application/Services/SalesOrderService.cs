using InventorySystem.Application.DTOs.PurchaseOrders;
using InventorySystem.Application.DTOs.SalesOrder;
using InventorySystem.Application.Interfaces;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.Inventory;
using InventorySystem.Domain.Entities.SalesOrder;
using System.Linq;

namespace InventorySystem.Application.Services
{
    public class SalesOrderService : ISalesOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISalesOrderGenerator _salesOrderGenerator;

        public SalesOrderService(IUnitOfWork unitOfWork, ISalesOrderGenerator salesOrderGenerator)
        {
            _unitOfWork = unitOfWork;
            _salesOrderGenerator = salesOrderGenerator;
        }

        #region GET

        public async Task<Result<List<SalesOrderDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var salesOrders = await _unitOfWork.SalesOrderRepository.GetAllWithLinesAsync(cancellationToken);
            var dtos = salesOrders.Select(MapToDto).ToList();
            return Result<List<SalesOrderDto>>.Success(dtos);
        }

        public async Task<Result<SalesOrderDto>> GetWithLinesAsync(int id, CancellationToken cancellationToken = default)
        {
            var salesOrder = await _unitOfWork.SalesOrderRepository.GetWithLinesAsync(id, cancellationToken);
            if (salesOrder == null)
                return Result<SalesOrderDto>.Failure($"SalesOrder with ID: {id} is not existed !");

            var dto = MapToDto(salesOrder);
            return Result<SalesOrderDto>.Success(dto);
        }

        #endregion

        #region CRUD

        public async Task<Result<SalesOrderDto>> CreateAsync(CreateSalesOrderDto createSalesOrderDto, CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                #region Validations

                var isCustomerExisted = await _unitOfWork.CustomerRepository
                    .ExistsAsync(c => c.Id == createSalesOrderDto.CustomerId, cancellationToken);

                if (!isCustomerExisted)
                    return Result<SalesOrderDto>.Failure($"Customer ID {createSalesOrderDto.CustomerId} is not Existed !");

                #endregion

                IEnumerable<(int ProductId, decimal Qty)> p =
                    createSalesOrderDto.CreateLinesDto
                           .Select(c => (c.ProductId, c.OrderedQty))
                           .ToList();

                var (salesOrderLines, reservations) = await BuildFifoReservation(p, cancellationToken);

                #region SalesOrder

                string orderNumber = await _salesOrderGenerator.GenerateAsync(cancellationToken);

                var salesOrder = new SalesOrder
                {
                    OrderNumber = orderNumber,
                    CustomerId = createSalesOrderDto.CustomerId,
                    OrderDate = DateTime.Now,
                    Lines = salesOrderLines
                };

                await _unitOfWork.SalesOrderRepository.AddAsync(salesOrder);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                #endregion

                #region Reservation Stock

                foreach (var reservation in reservations)
                {
                    reservation.SourceId = salesOrder.Id;
                    await _unitOfWork.InventoryReservationRepository.AddAsync(reservation);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                #endregion

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                var dto = MapToDto(salesOrder);
                return Result<SalesOrderDto>.Success(dto);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result<SalesOrderDto>.Failure(ex.Message);
            }
        }    

        public async Task<Result<SalesOrderDto>> UpdateAsync(int id, UpdateSalesOrderDto updateSalesOrderDto, CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                #region Validations

                var salesOrderExist = await _unitOfWork.SalesOrderRepository.GetWithLinesAsync(id, cancellationToken);
                if (salesOrderExist == null)
                    return Result<SalesOrderDto>.Failure($"SalesOrder with ID: {id} is not existed !");

                var isCustomerExisted = await _unitOfWork.CustomerRepository.ExistsAsync(c => c.Id == updateSalesOrderDto.CustomerId, cancellationToken);
                if (!isCustomerExisted)
                    return Result<SalesOrderDto>.Failure($"Customer ID {updateSalesOrderDto.CustomerId} is not Existed !");

                bool isAllow = salesOrderExist.AllowUpdate();
                if (!isAllow)
                    return Result<SalesOrderDto>.Failure($"Only Draft Status can be Updated !");

                #endregion

                #region Delete Old Reservation records

                var reserveExist = await _unitOfWork.InventoryReservationRepository.GetReservationBySalesOrder(id, cancellationToken);
                var layerIds = reserveExist
                                .Select(x => x.LayerId)
                                .Distinct();

                var layers = await _unitOfWork.InventoryCostLayerRepository.GetByIdsAsync(layerIds, cancellationToken);
                var layerDict = layers.ToDictionary(x => x.Id);

                foreach (var reservation in reserveExist)
                {
                    await _unitOfWork.InventoryReservationRepository.DeleteAsync(reservation);

                    if (layerDict.TryGetValue(reservation.LayerId, out var layer))
                    {
                        layer.ReservedQty = Math.Max(0, layer.ReservedQty - reservation.ReservedQty);
                    }
                }

                #endregion

                #region FIFO logic

                IEnumerable<(int ProductId, decimal Qty)> p =
                    updateSalesOrderDto.UpdateLinesDto
                           .Select(c => (c.ProductId, c.OrderedQty))
                           .ToList();

                var (salesOrderLines, reservations) = await BuildFifoReservation(p, cancellationToken);

                #endregion

                #region SalesOrder

                salesOrderExist.CustomerId = updateSalesOrderDto.CustomerId;
                salesOrderExist.Lines.Clear();
                foreach (var line in salesOrderLines)
                {
                    salesOrderExist.Lines.Add(line);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                #endregion

                #region Reservation Stock

                foreach (var reservation in reservations)
                {
                    reservation.SourceId = salesOrderExist.Id;
                    await _unitOfWork.InventoryReservationRepository.AddAsync(reservation);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                #endregion

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                var dto = MapToDto(salesOrderExist);
                return Result<SalesOrderDto>.Success(dto);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result<SalesOrderDto>.Failure(ex.Message);
            }
        }

        private async Task<(List<SalesOrderLine>, List<InventoryReservation>)> BuildFifoReservation(
                            IEnumerable<(int ProductId, decimal Qty)> lines,
                            CancellationToken cancellationToken)
        {
            var salesOrderLines = new List<SalesOrderLine>();
            var reservations = new List<InventoryReservation>();
            int rowNumber = 1;

            foreach (var line in lines)
            {
                var layers = await _unitOfWork.InventoryCostLayerRepository
                    .GetFIFOProductsById(line.ProductId);

                if (layers == null || layers.Count == 0)
                    throw new Exception($"Product {line.ProductId} not in inventory");

                decimal remainingQty = line.Qty;

                foreach (var layer in layers)
                {
                    if (remainingQty <= 0)
                        break;

                    var takeQty = Math.Min(layer.RemainingQty, remainingQty);

                    remainingQty -= takeQty;

                    salesOrderLines.Add(new SalesOrderLine
                    {
                        ProductId = line.ProductId,
                        RowNumber = rowNumber,
                        UnitPrice = layer.UnitCost,
                        OrderedQty = takeQty
                    });

                    reservations.Add(new InventoryReservation
                    {
                        ProductId = line.ProductId,
                        LayerId = layer.Id,
                        ReservedQty = takeQty,
                        UnitPrice = layer.UnitCost
                    });

                    layer.ReservedQty += takeQty;
                    rowNumber++;
                }


                if (remainingQty > 0)
                    throw new Exception($"Not enough stock for product {line.ProductId}");
            }

            return (salesOrderLines, reservations);
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var salesOrderExist = await _unitOfWork.SalesOrderRepository.GetWithLinesAsync(id, cancellationToken);
            if (salesOrderExist == null)
                return Result.Failure($"SalesOrder with ID: {id} is not existed !");

            salesOrderExist.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result<bool>> ExistAsync(int id, CancellationToken cancellationToken = default)
        {
            var isExist = await _unitOfWork.SalesOrderRepository.ExistsAsync(s => s.Id == id, cancellationToken);
            return Result<bool>.Success(isExist);
        }

        #endregion

        public async Task<Result> CancelAsync(int id, CancellationToken cancellationToken = default)
        {
            var salesOrderExist = await _unitOfWork.SalesOrderRepository.GetWithLinesAsync(id, cancellationToken);
            if (salesOrderExist == null)
                return Result.Failure($"SalesOrder with ID: {id} is not existed !");

            salesOrderExist.Status = Domain.Enums.SalesOrderStatus.Cancelled;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> ConfirmAsync(int id, CancellationToken cancellationToken = default)
        {
            SalesOrder so = await _unitOfWork.SalesOrderRepository.GetByIdAsync(id, cancellationToken);
            if (so == null)
                return Result.Failure($"SalesOrder with ID {id} not found.");

            so.Confirm();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        #region Helpers

        private static SalesOrderDto MapToDto(SalesOrder entity)
        {
            return new SalesOrderDto
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                OrderDate = entity.OrderDate,
                Status = entity.Status,
                CustomerName = entity.Customer?.CustomerName,
                TotalAmount = entity.TotalAmount,
                LinesDto = entity.Lines.Select(l => new SalesOrderLineDto
                {
                    ProductId = l.ProductId,
                    ProductName = l.Product.Name,
                    DeliveredQty = l.DeliveredQty,
                    OrderedQty = l.OrderedQty,
                    RemainingQty = l.RemainingQty,
                    UnitPrice = l.UnitPrice,
                    LineTotal = l.LineTotal,
                }).ToList(),
            };
        }

        #endregion
    }
}
