using InventorySystem.Application.DTOs.PurchaseOrders;
using InventorySystem.Application.Interfaces;
using InventorySystem.Application.Interfaces.Services;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.PurchaseOrder;

namespace InventorySystem.Application.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IUnitOfWork _unitOfWork;

    public PurchaseOrderService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<PurchaseOrderDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.PurchaseOrderRepository.GetAllWithLinesAsync(cancellationToken);
        var dtos = entities.Select(MapToDto);
        return Result<IEnumerable<PurchaseOrderDto>>.Success(dtos);
    }

    public async Task<Result<PurchaseOrderDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.PurchaseOrderRepository.GetWithLinesAsync(id, cancellationToken);
        if (entity == null)
        {
            return Result<PurchaseOrderDto>.Failure($"PurchaseOrder with ID {id} not found.");
        }

        var dto = MapToDto(entity);
        return Result<PurchaseOrderDto>.Success(dto);
    }

    private static PurchaseOrderDto MapToDto(PurchaseOrder entity)
    {
        return new PurchaseOrderDto
        {
            Id = entity.Id,
            OrderNumber = entity.OrderNumber,
            SupplierId = entity.SupplierId,
            Status = entity.Status,
            OrderDate = entity.OrderDate,
            ApprovedDate = entity.ApprovedDate,
            TotalAmount = entity.TotalAmount,
            Lines = entity.Lines?.Select(l => new PurchaseOrderLineDto
            {
                PurchaseOrderId = l.PurchaseOrderId,
                ProductId = l.ProductId,
                OrderedQty = l.OrderedQty,
                ReceivedQty = l.ReceivedQty,
                UnitPrice = l.UnitPrice,
                LineTotal = l.OrderedQty * l.UnitPrice
            }).ToList() ?? new List<PurchaseOrderLineDto>()
        };
    }
}

