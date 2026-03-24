using InventorySystem.Application.DTOs.SalesOrder;
using InventorySystem.Application.Interfaces.Generators;
using InventorySystem.Application.Interfaces.Repositories;
using InventorySystem.Application.Interfaces.Services;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.Inventory;
using InventorySystem.Domain.Entities.SalesOrder;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Result<List<SalesOrder>>> GetConfirmedSalesOrders(CancellationToken cancellationToken = default)
        {
            var salesOrdersConfirmed = await _unitOfWork.SalesOrderRepository.GetConfirmedSalesOrders(cancellationToken);
            return Result<List<SalesOrder>>.Success(salesOrdersConfirmed);
        }

        #endregion

        #region CRUDs

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

                var salesOrderLines = new List<SalesOrderLine>();
                int rowNumber = 1;
                foreach(var create_line in createSalesOrderDto.CreateLinesDto)
                {
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(create_line.ProductId, cancellationToken);
                    if(product == null)
                        return Result<SalesOrderDto>.Failure($"Product ID {create_line.ProductId} is not Existed !");

                    salesOrderLines.Add(
                        new SalesOrderLine
                        {
                            ProductId = create_line.ProductId,
                            RowNumber = rowNumber,
                            UnitPrice = product.BasePrice,
                            OrderedQty = create_line.OrderedQty,
                        }
                    );

                    rowNumber++;
                }

                string orderNumber = await _salesOrderGenerator.GenerateAsync(cancellationToken);
                var salesOrder = new SalesOrder
                {
                    OrderNumber = orderNumber,
                    CustomerId = createSalesOrderDto.CustomerId,
                    OrderDate = DateTime.UtcNow,
                    Lines = salesOrderLines
                };

                await _unitOfWork.SalesOrderRepository.AddAsync(salesOrder);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
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

                // Concurrency check: attach client RowVersion so EF can detect conflicts
                if (updateSalesOrderDto.RowVersion != null && salesOrderExist.RowVersion != null)
                {
                    // This assignment tells EF which version the client thinks it is editing
                    salesOrderExist.RowVersion = updateSalesOrderDto.RowVersion;
                }

                salesOrderExist.CustomerId = updateSalesOrderDto.CustomerId;
                salesOrderExist.Lines.Clear();

                int rowNumber = 1;
                foreach (var update_line in updateSalesOrderDto.UpdateLinesDto)
                {
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(update_line.ProductId, cancellationToken);
                    if (product == null)
                        return Result<SalesOrderDto>.Failure($"Product ID {update_line.ProductId} is not Existed !");

                    salesOrderExist.Lines.Add(new SalesOrderLine
                    {
                        ProductId = update_line.ProductId,
                        RowNumber = rowNumber,
                        UnitPrice = product.BasePrice,
                        OrderedQty = update_line.OrderedQty,
                    });

                    rowNumber++;
                }

                try
                {
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await _unitOfWork.CommitTransactionAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result<SalesOrderDto>.Failure("SalesOrder is updated by other users, please update again !");
                }

                var dto = MapToDto(salesOrderExist);
                return Result<SalesOrderDto>.Success(dto);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result<SalesOrderDto>.Failure(ex.Message);
            }
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
            const int maxAttempts = 3;

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    await _unitOfWork.BeginTransactionAsync(cancellationToken);

                    var so = await _unitOfWork.SalesOrderRepository.GetWithLinesAsync(id, cancellationToken);
                    if (so == null)
                        return Result.Failure($"SalesOrder with ID {id} not found.");

                    so.Confirm();

                    #region FIFO - Reservation

                    int rowNumber = 1;
                    const decimal margin = 0.3m;
                    var salesOrderLinesSplit = new List<SalesOrderLine>();

                    foreach (var salesOrderLine in so.Lines)
                    {
                        var layers = await _unitOfWork.InventoryCostLayerRepository.GetFIFOProductsById(salesOrderLine.ProductId, cancellationToken);

                        if (layers == null || layers.Count == 0)
                            throw new Exception($"Product {salesOrderLine.ProductId} not in inventory");

                        decimal remainingOrderedQty = salesOrderLine.OrderedQty;
                        foreach (var layer in layers)
                        {
                            if (remainingOrderedQty <= 0)
                                break;

                            var available = layer.RemainingQty - layer.ReservedQty;
                            if (available <= 0)
                                continue;

                            var takeQty = Math.Min(available, remainingOrderedQty);
                            remainingOrderedQty -= takeQty;

                            var sellingPrice = Math.Round(layer.UnitCost * (1 + margin), 2);

                            salesOrderLinesSplit.Add(new SalesOrderLine
                            {
                                SalesOrderId = salesOrderLine.SalesOrderId,
                                ProductId = salesOrderLine.ProductId,
                                RowNumber = rowNumber,
                                UnitPrice = sellingPrice,
                                OrderedQty = takeQty,
                            });

                            var reservation = new InventoryReservation
                            {
                                LayerId = layer.Id,
                                ProductId = salesOrderLine.ProductId,
                                SourceId = so.Id,
                                RowNumber = rowNumber,
                                ReservedQty = takeQty,
                                UnitCost = layer.UnitCost
                            };
                            await _unitOfWork.InventoryReservationRepository.AddAsync(reservation);

                            layer.ReservedQty += takeQty;
                            rowNumber++;
                        }

                        if (remainingOrderedQty > 0)
                            throw new Exception($"Not enough stock for product {salesOrderLine.ProductId}");
                    }

                    #endregion

                    #region Re-check SalesOrder

                    so.Lines.Clear();
                    foreach (var split in salesOrderLinesSplit)
                        so.Lines.Add(split);

                    #endregion

                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                    await _unitOfWork.CommitTransactionAsync(cancellationToken);

                    return Result.Success();
                }
                catch (DbUpdateConcurrencyException)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);

                    if (attempt == maxAttempts)
                        return Result.Failure("Inventory Layer OR SalesOrder is updated by another transaction, please confirm again !");
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result.Failure(ex.Message);
                }
            }

            return Result.Failure("Inventory Layer OR SalesOrder is updated by another transaction, please confirm again !");
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
                    ProductName = l.Product?.Name,
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
