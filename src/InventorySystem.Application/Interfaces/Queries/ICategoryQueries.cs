using InventorySystem.Application.DTOs.Categories;
using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.Interfaces.Queries
{
    public interface ICategoryQueries
    {
        Task<IReadOnlyList<CategoryDto>> GetCategoriesWithSubTree(CancellationToken cancellationToken);
        Task<CategoryDto?> GetCategoryWithSubTree(int categoryId, CancellationToken cancellationToken = default);
    }
}
