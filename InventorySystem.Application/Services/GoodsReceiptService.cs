using InventorySystem.Application.DTOs.GoodsReceipts;
using InventorySystem.Application.Interfaces;
using InventorySystem.Application.Interfaces.Services;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.GoodsReceipt;

namespace InventorySystem.Application.Services;

public class GoodsReceiptService : IGoodsReceiptService
{
    private readonly IUnitOfWork _unitOfWork;

    public GoodsReceiptService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<GoodsReceiptDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _unitOfWork.GoodsReceiptRepository.GetAllWithLinesAsync(cancellationToken);
        var dtos = entities.Select(MapToDto);
        return Result<IEnumerable<GoodsReceiptDto>>.Success(dtos);
    }

    public async Task<Result<GoodsReceiptDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.GoodsReceiptRepository.GetWithLinesAsync(id, cancellationToken);
        if (entity == null)
        {
            return Result<GoodsReceiptDto>.Failure($"GoodsReceipt with ID {id} not found.");
        }

        var dto = MapToDto(entity);
        return Result<GoodsReceiptDto>.Success(dto);
    }

    private static GoodsReceiptDto MapToDto(GoodsReceipt entity)
    {
        return new GoodsReceiptDto
        {
            Id = entity.Id,
            ReceiptNumber = entity.ReceiptNumber,
            PurchaseOrderId = entity.PurchaseOrderId,
            WarehouseId = entity.WarehouseId,
            Status = entity.Status,
            ReceiptDate = entity.ReceiptDate,
            Lines = entity.Lines?.Select(l => new GoodsReceiptLineDto
            {
                GoodsReceiptId = l.GoodsReceiptId,
                PurchaseOrderLineId = l.PurchaseOrderLineId,
                ProductId = l.ProductId,
                ReceivedQty = l.ReceivedQty,
                UnitCost = l.UnitCost,
                LineTotal = l.ReceivedQty * l.UnitCost
            }).ToList() ?? new List<GoodsReceiptLineDto>()
        };
    }
}

