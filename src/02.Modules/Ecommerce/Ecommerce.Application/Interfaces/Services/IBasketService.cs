using ECommerce.Application.DTOs.Baskets;
using SharedKernel;

namespace ECommerce.Application.Interfaces.Services
{
    public interface IBasketService
    {
        Task<Result<IEnumerable<BasketDto>>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Result<BasketDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<BasketDto>> CreateAsync(CreateBasketDto createDto, CancellationToken cancellationToken = default);
        Task<Result<BasketDto>> UpdateAsync(int id, UpdateBasketDto updateDto, CancellationToken cancellationToken = default);
        Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
