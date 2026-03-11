using InventorySystem.Application.DTOs.Delivery;
using InventorySystem.Domain.Common;

namespace InventorySystem.Application.Interfaces.Services
{
    public interface IDeliveryService
    {
        Task<Result<List<DeliveryDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<DeliveryDto>> GetWithLinesAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<DeliveryDto>> CreateAsync(CreateDeliveryDto createDeliveryDto, CancellationToken cancellationToken = default);
        Task<Result<DeliveryDto>> UpdateAsync(int id, UpdateDeliveryDto updateDeliveryDto, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
        Task<Result> PostAsync(int id, CancellationToken cancellationToken = default);
        Task<Result> CancelAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<bool>> ExistAsync(int id, CancellationToken cancellationToken = default);
    }
}
