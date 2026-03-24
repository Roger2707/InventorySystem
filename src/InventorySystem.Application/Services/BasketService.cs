using InventorySystem.Application.DTOs.Baskets;
using InventorySystem.Application.Interfaces.Repositories;
using InventorySystem.Application.Interfaces.Services;
using InventorySystem.Domain.Common;
using InventorySystem.Domain.Entities.Baskets;

namespace InventorySystem.Application.Services
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
            if (basket == null)
            {
                return Result<BasketDto>.Failure($"Basket with ID {id} not found.");
            }

            return Result<BasketDto>.Success(MapToDto(basket));
        }

        public async Task<Result<BasketDto>> CreateAsync(CreateBasketDto createDto, CancellationToken cancellationToken = default)
        {
            if (createDto.Items == null || createDto.Items.Count == 0)
            {
                return Result<BasketDto>.Failure("Basket must contain at least one item.");
            }

            var customerExists = await _unitOfWork.CustomerRepository.ExistsAsync(c => c.Id == createDto.CustomerId, cancellationToken);
            if (!customerExists)
            {
                return Result<BasketDto>.Failure($"Customer with ID {createDto.CustomerId} not found.");
            }

            var items = new List<BasketItem>();
            var rowNumber = 1;
            foreach (var inputItem in createDto.Items)
            {
                if (inputItem.Quantity <= 0)
                {
                    return Result<BasketDto>.Failure("Item quantity must be greater than zero.");
                }

                var product = await _unitOfWork.ProductRepository.GetByIdAsync(inputItem.ProductId, cancellationToken);
                if (product == null)
                {
                    return Result<BasketDto>.Failure($"Product with ID {inputItem.ProductId} not found.");
                }

                items.Add(new BasketItem
                {
                    ProductId = inputItem.ProductId,
                    Quantity = inputItem.Quantity,
                    UnitPrice = product.BasePrice,
                    RowNumber = rowNumber++
                });
            }

            var basket = new Basket
            {
                CustomerId = createDto.CustomerId,
                Items = items
            };

            await _unitOfWork.BasketRepository.AddAsync(basket, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var created = await _unitOfWork.BasketRepository.GetWithItemsAsync(basket.Id, cancellationToken);
            return Result<BasketDto>.Success(MapToDto(created!));
        }

        public async Task<Result<BasketDto>> UpdateAsync(int id, UpdateBasketDto updateDto, CancellationToken cancellationToken = default)
        {
            var basket = await _unitOfWork.BasketRepository.GetWithItemsAsync(id, cancellationToken);
            if (basket == null)
            {
                return Result<BasketDto>.Failure($"Basket with ID {id} not found.");
            }

            if (updateDto.Items == null || updateDto.Items.Count == 0)
            {
                return Result<BasketDto>.Failure("Basket must contain at least one item.");
            }

            var customerExists = await _unitOfWork.CustomerRepository.ExistsAsync(c => c.Id == updateDto.CustomerId, cancellationToken);
            if (!customerExists)
            {
                return Result<BasketDto>.Failure($"Customer with ID {updateDto.CustomerId} not found.");
            }

            basket.CustomerId = updateDto.CustomerId;
            basket.Items.Clear();

            var rowNumber = 1;
            foreach (var inputItem in updateDto.Items)
            {
                if (inputItem.Quantity <= 0)
                {
                    return Result<BasketDto>.Failure("Item quantity must be greater than zero.");
                }

                var product = await _unitOfWork.ProductRepository.GetByIdAsync(inputItem.ProductId, cancellationToken);
                if (product == null)
                {
                    return Result<BasketDto>.Failure($"Product with ID {inputItem.ProductId} not found.");
                }

                basket.Items.Add(new BasketItem
                {
                    ProductId = inputItem.ProductId,
                    Quantity = inputItem.Quantity,
                    UnitPrice = product.BasePrice,
                    RowNumber = rowNumber++
                });
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<BasketDto>.Success(MapToDto(basket));
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var basket = await _unitOfWork.BasketRepository.GetByIdAsync(id, cancellationToken);
            if (basket == null)
            {
                return Result.Failure($"Basket with ID {id} not found.");
            }

            basket.IsDeleted = true;
            await _unitOfWork.BasketRepository.UpdateAsync(basket, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result<bool>> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            var exists = await _unitOfWork.BasketRepository.ExistsAsync(x => x.Id == id, cancellationToken);
            return Result<bool>.Success(exists);
        }

        private static BasketDto MapToDto(Basket basket)
        {
            var itemDtos = basket.Items.Select(x => new BasketItemDto
            {
                ProductId = x.ProductId,
                ProductName = x.Product?.Name ?? string.Empty,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                LineTotal = x.LineTotal
            }).ToList();

            return new BasketDto
            {
                Id = basket.Id,
                CustomerId = basket.CustomerId,
                CustomerName = basket.Customer?.CustomerName ?? string.Empty,
                CreatedAt = basket.CreatedAt,
                UpdatedAt = basket.UpdatedAt,
                Items = itemDtos,
                TotalAmount = itemDtos.Sum(i => i.LineTotal)
            };
        }
    }
}
