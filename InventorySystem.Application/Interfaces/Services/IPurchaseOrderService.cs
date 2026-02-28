using InventorySystem.Application.DTOs.PurchaseOrders;
using InventorySystem.Domain.Common;

namespace InventorySystem.Application.Interfaces.Services;

public interface IPurchaseOrderService
{
    Task<Result<IEnumerable<PurchaseOrderDto>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<PurchaseOrderDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Result<PurchaseOrderDto>> CreateAsync(CreatePurchaseOrderDto createPurchaseOrder, CancellationToken cancellationToken = default);
}

