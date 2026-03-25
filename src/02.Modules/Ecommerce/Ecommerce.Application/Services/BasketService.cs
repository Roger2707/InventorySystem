using ECommerce.Application.DTOs.Baskets;
using ECommerce.Application.Interfaces.Repositories;
using ECommerce.Application.Interfaces.Services;
using ECommerce.Domain.Entities.Baskets;
using SharedKernel;

namespace ECommerce.Application.Services
{
    public class BasketService : IBasketService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BasketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<BasketDto>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var baskets = await _unitOfWork.BasketRepository.GetAllWithItemsAsync(cancellationToken);
            return Result<IEnumerable<BasketDto>>.Success(baskets.Select(MapToDto));
        }

        public async Task<Result<BasketDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var basket = await _unitOfWork.BasketRepository.GetWithItemsAsync(id, cancellationToken);
            if (basket == null) return Result<BasketDto>.Failure($"Basket with ID {id} not found.");
            return Result<BasketDto>.Success(MapToDto(basket));
        }

        public async Task<Result<BasketDto>> CreateAsync(CreateBasketDto createDto, CancellationToken cancellationToken = default)
        {
            if (createDto.Items == null || createDto.Items.Count == 0)
                return Result<BasketDto>.Failure("Basket must contain at least one item.");

            var rowNumber = 1;
            var basket = new Basket
            {
                CustomerId = createDto.CustomerId,
                Items = createDto.Items.Select(x => new BasketItem
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    UnitPrice = x.UnitPrice,
                    RowNumber = rowNumber++
                }).ToList()
            };

            await _unitOfWork.BasketRepository.AddAsync(basket, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var created = await _unitOfWork.BasketRepository.GetWithItemsAsync(basket.Id, cancellationToken);
            return Result<BasketDto>.Success(MapToDto(created!));
        }

        public async Task<Result<BasketDto>> UpdateAsync(int id, UpdateBasketDto updateDto, CancellationToken cancellationToken = default)
        {
            var basket = await _unitOfWork.BasketRepository.GetWithItemsAsync(id, cancellationToken);
            if (basket == null) return Result<BasketDto>.Failure($"Basket with ID {id} not found.");

            basket.CustomerId = updateDto.CustomerId;
            basket.Items.Clear();

            var rowNumber = 1;
            foreach (var item in updateDto.Items)
            {
                basket.Items.Add(new BasketItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    RowNumber = rowNumber++
                });
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<BasketDto>.Success(MapToDto(basket));
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var basket = await _unitOfWork.BasketRepository.GetByIdAsync(id, cancellationToken);
            if (basket == null) return Result.Failure($"Basket with ID {id} not found.");
            basket.IsDeleted = true;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        private static BasketDto MapToDto(Basket basket)
        {
            var items = basket.Items.Select(x => new BasketItemDto
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                LineTotal = x.LineTotal
            }).ToList();

            return new BasketDto
            {
                Id = basket.Id,
                CustomerId = basket.CustomerId,
                CreatedAt = basket.CreatedAt,
                UpdatedAt = basket.UpdatedAt,
                Items = items,
                TotalAmount = items.Sum(x => x.LineTotal)
            };
        }
    }
}
