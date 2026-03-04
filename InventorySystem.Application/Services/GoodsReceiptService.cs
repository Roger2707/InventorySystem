using InventorySystem.Application.DTOs.GoodsReceipts;
using InventorySystem.Application.Interfaces;
using InventorySystem.Application.Interfaces.Services;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.GoodsReceipt;
using InventorySystem.Domain.Enums;

namespace InventorySystem.Application.Services;

public class GoodsReceiptService : IGoodsReceiptService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGoodsReceiptGenerator _goodsReceiptGenerator;

    public GoodsReceiptService(IUnitOfWork unitOfWork, IGoodsReceiptGenerator goodsReceiptGenerator)
    {
        _unitOfWork = unitOfWork;
        _goodsReceiptGenerator = goodsReceiptGenerator;
    }

    #region Get

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

    #endregion

    #region CRUD

    public async Task<Result<GoodsReceiptDto>> CreateAsync(CreateGoodsReceiptDto createGoodsReceipt, CancellationToken cancellationToken = default)
    {
        var poExist = await _unitOfWork.PurchaseOrderRepository.GetWithLinesAsync(createGoodsReceipt.PurchaseOrderId, cancellationToken);
        if (poExist == null)
            return Result<GoodsReceiptDto>.Failure($"PurchaseOrder with ID {createGoodsReceipt.PurchaseOrderId} not found.");

        var warehouseExist = await _unitOfWork.WarehouseRepository.ExistsAsync(w => w.Id == createGoodsReceipt.WarehouseId, cancellationToken);
        if (!warehouseExist)
            return Result<GoodsReceiptDto>.Failure($"Warehouse with ID {createGoodsReceipt.WarehouseId} not found.");

        foreach(var line in createGoodsReceipt.LinesDto)
        {
            // Check if the PurchaseOrderLine exists and belongs to the specified PurchaseOrder
            var lineExist = poExist.Lines.FirstOrDefault(po => po.PurchaseOrderId == line.PurchaseOrderId && po.ProductId == line.ProductId);
            if (lineExist == null)
                return Result<GoodsReceiptDto>.Failure($"PurchaseOrderLine with ProductID {line.ProductId} not found in PurchaseOrder {createGoodsReceipt.PurchaseOrderId}.");

            // Check if the Product exists
            var productExist = await _unitOfWork.ProductRepository.ExistsAsync(p => p.Id == line.ProductId, cancellationToken);
            if (!productExist)
                return Result<GoodsReceiptDto>.Failure($"Product with ID {line.ProductId} not found.");

            if(line.ReceivedQty <= 0)
                return Result<GoodsReceiptDto>.Failure($"Received quantity must be greater than zero for Product ID {line.ProductId}.");

            if(line.ReceivedQty > lineExist.OrderedQty)
                return Result<GoodsReceiptDto>.Failure($"Received quantity for Product ID {line.ProductId} cannot exceed the ordered quantity.");
        }

        var receiptNumber = await _goodsReceiptGenerator.GenerateAsync(cancellationToken);
        var lines = createGoodsReceipt.LinesDto.Select(l => new GoodsReceiptLine
        {
            PurchaseOrderId = poExist.Id,
            ProductId = l.ProductId,
            ReceivedQty = l.ReceivedQty,
            UnitCost = poExist.Lines.FirstOrDefault(pol => pol.ProductId == l.ProductId).UnitPrice,
        }).ToList();

        var entity = new GoodsReceipt
        {
            ReceiptNumber = receiptNumber,
            PurchaseOrderId = createGoodsReceipt.PurchaseOrderId,
            WarehouseId = createGoodsReceipt.WarehouseId,
            ReceiptDate = DateTime.UtcNow,
            Status = ReceiptStatus.Draft,
            Lines = lines,
            TotalAmount = lines.Sum(l => l.LineTotal)
        };

        await _unitOfWork.GoodsReceiptRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var dto = MapToDto(entity);
        return Result<GoodsReceiptDto>.Success(dto);
    }

    public async Task<Result<GoodsReceiptDto>> UpdateAsync(int id, UpdateGoodsReceiptDto updateGoodsReceipt, CancellationToken cancellationToken = default)
    {
        var exist = await _unitOfWork.GoodsReceiptRepository.GetWithLinesAsync(id, cancellationToken);
        if (exist == null)
            return Result<GoodsReceiptDto>.Failure($"GoodsReceipt with ID {id} not found.");

        var poExist = await _unitOfWork.PurchaseOrderRepository.GetWithLinesAsync(exist.PurchaseOrderId, cancellationToken);
        if (poExist == null)
            return Result<GoodsReceiptDto>.Failure($"PurchaseOrder with ID {exist.PurchaseOrderId} not found.");

        var warehouseExist = await _unitOfWork.WarehouseRepository.ExistsAsync(w => w.Id == updateGoodsReceipt.WarehouseId, cancellationToken);
        if (!warehouseExist)
            return Result<GoodsReceiptDto>.Failure($"Warehouse with ID {updateGoodsReceipt.WarehouseId} not found.");

        var lineParams = new List<(int ProductId, decimal orderedQty, decimal ReceivedQty, decimal UnitCost)>();
        foreach (var line in updateGoodsReceipt.LinesDto)
        {
            // Check if the PurchaseOrderLine exists and belongs to the specified PurchaseOrder
            var poLineExist = poExist.Lines.FirstOrDefault(po => po.ProductId == line.ProductId);
            if (poLineExist == null)
                return Result<GoodsReceiptDto>.Failure($"PurchaseOrderLine with ProductID {line.ProductId} not found in PurchaseOrder {poExist.Id}.");

            if (line.ReceivedQty <= 0)
                return Result<GoodsReceiptDto>.Failure($"Received quantity must be greater than zero for Product ID {line.ProductId}.");

            lineParams.Add((line.ProductId, poLineExist.OrderedQty, line.ReceivedQty, poLineExist.UnitPrice));
        }

        exist.Update(
            updateGoodsReceipt.WarehouseId,
            DateTime.Now,
            lineParams
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        var dto = MapToDto(exist);
        return Result<GoodsReceiptDto>.Success(dto);
    }

    public Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Actions

    public Task<Result> PostAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result> CancelAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Result<bool>> ExistAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Helpers

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
                PurchaseOrderLId = l.PurchaseOrderId,
                ProductId = l.ProductId,
                ReceivedQty = l.ReceivedQty,
                UnitCost = l.UnitCost,
                LineTotal = l.ReceivedQty * l.UnitCost
            }).ToList() ?? new List<GoodsReceiptLineDto>()
        };
    }

    #endregion
}

