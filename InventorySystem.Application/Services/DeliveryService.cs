using InventorySystem.Application.DTOs.Delivery;
using InventorySystem.Application.Interfaces;
using InventorySystem.Application.Interfaces.Services;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.Delivery;
using InventorySystem.Domain.Entities.Inventory;
using InventorySystem.Domain.Entities.SalesOrder;
using InventorySystem.Domain.Enums;

namespace InventorySystem.Application.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDeliveryGenerator _deliveryGenerator;

        public DeliveryService(IUnitOfWork unitOfWork, IDeliveryGenerator deliveryGenerator)
        {
            _unitOfWork = unitOfWork;
            _deliveryGenerator = deliveryGenerator;
        }

        #region GETs

        public async Task<Result<List<DeliveryDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var deliveries = await _unitOfWork.DeliveryRepository.GetAllAsync(cancellationToken);
            var dtos = deliveries.Select(MapToDto).ToList();
            return Result<List<DeliveryDto>>.Success(dtos);
        }

        public async Task<Result<DeliveryDto>> GetWithLinesAsync(int id, CancellationToken cancellationToken = default)
        {
            var delivery = await _unitOfWork.DeliveryRepository.GetWithLinesAsync(id, cancellationToken);
            if (delivery == null)
                return Result<DeliveryDto>.Failure($"Delivery Id: {id} is not existed !");
            var dto = MapToDto(delivery);
            return Result<DeliveryDto>.Success(dto);
        }

        #endregion

        #region CRUDs

        public async Task<Result<DeliveryDto>> CreateAsync(CreateDeliveryDto createDeliveryDto, CancellationToken cancellationToken = default)
        {
            #region Validations

            var existedSalesOrder = await _unitOfWork.SalesOrderRepository.GetWithLinesAsync(createDeliveryDto.SalesOrderId, cancellationToken);
            if (existedSalesOrder == null)
                return Result<DeliveryDto>.Failure($"SalesOrder ID: {createDeliveryDto.SalesOrderId} is not existed !");

            if (existedSalesOrder.Status != SalesOrderStatus.Draft)
                return Result<DeliveryDto>.Failure($"Only Draft Status can be created Delivery - SalesOrder ID: {createDeliveryDto.SalesOrderId} !");

            #endregion

            var salesOrderLines = existedSalesOrder.Lines;
            var deliveriesLines = new List<DeliveryLine>();

            foreach (var deliveryLineDto in createDeliveryDto.LinesDto)
            {
                var salesOrderLine = salesOrderLines
                    .FirstOrDefault(sl => sl.ProductId == deliveryLineDto.ProductId && sl.SalesOrderId == createDeliveryDto.SalesOrderId && deliveryLineDto.RowNumber == sl.RowNumber);

                if (salesOrderLine == null)
                    return Result<DeliveryDto>.Failure($"Product ID {deliveryLineDto.ProductId} is not Exist in SalesOrder {salesOrderLine.SalesOrderId} !");

                if (deliveryLineDto.Quantity > salesOrderLine.RemainingQty)
                    return Result<DeliveryDto>.Failure("Delivery Qty cannot be greater than RemainingQty");

                var deliveryLine = new DeliveryLine
                {
                    ProductId = deliveryLineDto.ProductId,
                    RowNumber = deliveryLineDto.RowNumber,
                    Quantity = deliveryLineDto.Quantity,
                    UnitPrice = salesOrderLine.UnitPrice,
                };
                deliveriesLines.Add(deliveryLine);
            }

            string orderNumber = await _deliveryGenerator.GenerateAsync(cancellationToken);
            var delivery = new Delivery
            {
                OrderNumber = orderNumber,
                SalesOrderId = createDeliveryDto.SalesOrderId,
                TotalAmount = deliveriesLines.Sum(l => l.UnitPrice * l.Quantity),
                Lines = deliveriesLines
            };

            await _unitOfWork.DeliveryRepository.AddAsync(delivery);
            await _unitOfWork.SaveChangesAsync();

            var dto = MapToDto(delivery);
            return Result<DeliveryDto>.Success(dto);
        }

        public async Task<Result<DeliveryDto>> UpdateAsync(int id, UpdateDeliveryDto updateDeliveryDto, CancellationToken cancellationToken = default)
        {
            #region Validations

            var existedDelivery = await _unitOfWork.DeliveryRepository.GetWithLinesAsync(id, cancellationToken);
            if(existedDelivery == null)
                return Result<DeliveryDto>.Failure($"Delivery ID: {id} is not existed !");

            var existedSalesOrder = await _unitOfWork.SalesOrderRepository.GetWithLinesAsync(existedDelivery.SalesOrderId, cancellationToken);
            if (existedSalesOrder == null)
                return Result<DeliveryDto>.Failure($"SalesOrder ID: {existedDelivery.SalesOrderId} is not existed !");

            if (existedDelivery.Status != DeliveryStatus.Draft)
                return Result<DeliveryDto>.Failure($"Only Draft Status can be Update!");

            #endregion

            #region Remove Line Exist is not match New Lines

            var removedDeliveryLines = existedDelivery.Lines
                .Where(e => !updateDeliveryDto.LinesDto.Any(u => u.ProductId == e.ProductId && u.RowNumber == e.RowNumber))
                .ToList();

            foreach (var removedDeliveryLine in removedDeliveryLines)
                existedDelivery.Lines.Remove(removedDeliveryLine);

            #endregion

            foreach (var updateDeliveryLineDto in updateDeliveryDto.LinesDto)
            {
                var salesOrderLine = existedSalesOrder.Lines
                    .FirstOrDefault(sl => sl.ProductId == updateDeliveryLineDto.ProductId && sl.SalesOrderId == existedDelivery.SalesOrderId && sl.RowNumber == updateDeliveryLineDto.RowNumber);
                
                if (salesOrderLine == null)
                    return Result<DeliveryDto>.Failure($"Product ID {updateDeliveryLineDto.ProductId} is not Exist in SalesOrder {salesOrderLine.SalesOrderId} !");

                if (updateDeliveryLineDto.Quantity > salesOrderLine.RemainingQty)
                    return Result<DeliveryDto>.Failure("Delivery Qty cannot be greater than RemainingQty");

                var existedDeliveryLine = existedDelivery.Lines
                    .FirstOrDefault(e => e.ProductId == updateDeliveryLineDto.ProductId && e.RowNumber == updateDeliveryLineDto.RowNumber);
                if(existedDeliveryLine != null)
                {
                    existedDeliveryLine.Quantity = updateDeliveryLineDto.Quantity;
                }
                else
                {
                    var newDeliveryLine = new DeliveryLine
                    {
                        ProductId = updateDeliveryLineDto.ProductId,
                        RowNumber = updateDeliveryLineDto.RowNumber,
                        Quantity = updateDeliveryLineDto.Quantity,
                        UnitPrice = salesOrderLine.UnitPrice,
                    };
                    existedDelivery.Lines.Add(newDeliveryLine);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            var dto = MapToDto(existedDelivery);
            return Result<DeliveryDto>.Success(dto);
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var exist = await _unitOfWork.DeliveryRepository.GetByIdAsync(id, cancellationToken);
            if (exist == null)
                return Result.Failure($"Delivery Id: {id} is not existed !");

            exist.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<bool>> ExistAsync(int id, CancellationToken cancellationToken = default)
        {
            var isExist = await _unitOfWork.DeliveryRepository.ExistsAsync(x => x.Id == id);
            return Result<bool>.Success(isExist);
        }

        #endregion

        #region Actions

        public async Task<Result> PostAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                var delivery = await _unitOfWork.DeliveryRepository.GetWithLinesAsync(id, cancellationToken);
                if (delivery == null || delivery.Status == DeliveryStatus.Cancelled || delivery.Status == DeliveryStatus.Posted)
                    return Result.Failure("Delivery invalid status!");

                var salesOrder = await _unitOfWork.SalesOrderRepository.GetWithLinesAsync(delivery.SalesOrderId, cancellationToken);
                if (salesOrder == null || salesOrder.Status == SalesOrderStatus.Completed || salesOrder.Status == SalesOrderStatus.Cancelled)
                    return Result.Failure("Something went wrong with own SalesOrder !");

                var reservations = await _unitOfWork.InventoryReservationRepository.GetReservationBySalesOrder(delivery.SalesOrderId, cancellationToken);

                int isSalesOrderCompleted = 0;
                foreach(var deliveryLine in delivery.Lines)
                {
                    var salesOrderLine = salesOrder.Lines.FirstOrDefault(sl => sl.ProductId == deliveryLine.ProductId && sl.RowNumber == deliveryLine.RowNumber);
                    if(salesOrderLine == null)
                        return Result.Failure($"This DeliveryLine {deliveryLine.RowNumber}: Product_{deliveryLine.ProductId} is not found SalesOrderLine !");

                    if(deliveryLine.Quantity > salesOrderLine.RemainingQty)
                        return Result.Failure($"This DeliveryLine {deliveryLine.RowNumber}: Product_{deliveryLine.ProductId} has quanty greater than SalesOrderLine !");

                    // Update SalesOrderLine Qty
                    salesOrderLine.DeliveredQty += deliveryLine.Quantity;

                    // Check each Line OrderQuanty and DeliveredQty
                    if (salesOrderLine.DeliveredQty >= salesOrderLine.OrderedQty)
                        isSalesOrderCompleted++;

                    var reservation = reservations.FirstOrDefault(r => r.ProductId == deliveryLine.ProductId && r.RowNumber == deliveryLine.RowNumber);
                    if(reservation == null)
                        return Result.Failure($"This DeliveryLine {deliveryLine.RowNumber}: Product_{deliveryLine.ProductId}, SalesOrder: {salesOrderLine.SalesOrderId}, Row: {salesOrderLine.RowNumber} can not find Reservation !");

                    // Reduce ReservedQty in Reservation entity
                    if (reservation.ReservedQty < deliveryLine.Quantity)
                        return Result.Failure("Reservation not enough");

                    reservation.ReservedQty -= deliveryLine.Quantity;
                    if(reservation.ReservedQty == 0)
                        reservation.IsDeleted = true;

                    // Cost Layer
                    int costLayerId = reservation.LayerId;
                    var costLayer = await _unitOfWork.InventoryCostLayerRepository.GetByIdAsync(costLayerId, cancellationToken);
                    if (costLayer.RemainingQty < deliveryLine.Quantity)
                        return Result.Failure("Cost layer not enough");

                    costLayer.RemainingQty -= deliveryLine.Quantity;
                    costLayer.ReservedQty -= deliveryLine.Quantity;

                    // Ledger (History Transactions)
                    var ledger = new InventoryLedger
                    {
                        ProductId = deliveryLine.ProductId,
                        WarehouseId = costLayer.WarehouseId,
                        TransactionType = InventoryTransactionType.Issue,
                        ReferenceId = delivery.SalesOrderId,
                        ReferenceType = "SalesOrder",
                        QuantityIn = 0,
                        QuantityOut = deliveryLine.Quantity,
                        UnitCost = costLayer.UnitCost,
                        TotalCost = costLayer.UnitCost * deliveryLine.Quantity
                    };

                    await _unitOfWork.InventoryLedgerRepository.AddAsync(ledger);
                }

                // Update SalesOrder Status depends on all qty completed
                if (isSalesOrderCompleted == salesOrder.Lines.Count)
                    salesOrder.Status = SalesOrderStatus.Completed;
                else
                    salesOrder.Status = SalesOrderStatus.PartiallyDelivered;

                // Update Delivery Status
                delivery.Status = DeliveryStatus.Posted;

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Failure(ex.Message);
            }
        }

        public async Task<Result> CancelAsync(int id, CancellationToken cancellationToken = default)
        {
            var exist = await _unitOfWork.DeliveryRepository.GetByIdAsync(id, cancellationToken);
            if(exist == null)
                return Result.Failure($"Delivery Id: {id} is not existed !");

            exist.Status = DeliveryStatus.Cancelled;
            await _unitOfWork.SaveChangesAsync (cancellationToken);
            return Result.Success();
        }

        #endregion

        #region Helpers

        private static DeliveryDto MapToDto(Delivery entity)
        {
            return new DeliveryDto
            {
                Id = entity.Id,
                OrderNumber = entity.OrderNumber,
                SalesOrderId = entity.SalesOrderId,
                Status = entity.Status,
                DeliveryDate = entity.DeliveryDate,
                TotalAmount = entity.TotalAmount,
                LinesDto = entity.Lines
                    .Select(l => new DeliveryLineDto
                    {
                        DeliveryId = entity.Id,
                        ProductId = l.ProductId,
                        Quantity = l.Quantity,
                        UnitPrice = l.UnitPrice,     
                        LineTotal = l.LineTotal,
                    })
                    .ToList()
            };
        }

        #endregion
    }
}
