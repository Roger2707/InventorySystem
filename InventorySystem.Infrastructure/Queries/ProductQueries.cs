using InventorySystem.Application.DTOs.Products;
using InventorySystem.Application.Interfaces;
using InventorySystem.Application.Interfaces.Queries;
using InventorySystem.Domain.Common;

namespace InventorySystem.Infrastructure.Queries
{
    public class ProductQueries : IProductQueries
    {
        private readonly IDapperExecutor _dapper;

        public ProductQueries(IDapperExecutor dapper)
        {
            _dapper = dapper;
        }

        public async Task<Result<List<ProductDto>>> GetAllAsync(CancellationToken cancellationToken)
        {
            const string query = @"
                                    SELECT
	                                    p.Id
	                                    , p.Name
	                                    , p.SKU
	                                    , p.Barcode
	                                    , p.CategoryId
	                                    , ct.Name as CategoryName
	                                    , p.BaseUoMId
	                                    , u_base.Name as BaseUoMName
	                                    , p.MinStockLevel
	                                    , p.IsDeleted
	                                    , p.IsPerishable
	                                    , p.CreatedAt
	                                    , p.UpdatedAt

	                                    -- conversion

	                                    , c.FromUoMId as UoMIdFrom
	                                    , u_from.Name as UoMFromName
	                                    , c.ToUoMId as UoMIdTo
	                                    , u_to.Name as UoMToName
	                                    , c.Factor

                                    FROM Products p

                                    LEFT JOIN Categories ct ON ct.Id = p.CategoryId
                                    LEFT JOIN ProductUoMConversions c ON c.ProductId = p.Id
                                    LEFT JOIN UoMs u_base ON u_base.Id = p.BaseUoMId
                                    LEFT JOIN UoMs u_from ON u_from.Id = c.FromUoMId
                                    LEFT JOIN UoMs u_to ON u_to.Id = c.ToUoMId

                                    ORDER BY ProductId ASC
                                ";
            
            var product_rows = await _dapper.QueryAsync<ProductDto>(query);

            var productDtos = product_rows.ToList().Select(row => new ProductDto
            {

            }).ToList();

            return Result<List<ProductDto>>.Success(productDtos);
        }

        public Task<Result<ProductDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private sealed class ProductRow
        {
            public int Id { get; init; }
            public string Name { get; init; }
            public string SKU { get; set; }
            public string Barcode { get; set; }

            public int CategoryId { get; set; }
            public string CategoryName { get; set; }

            public int BaseUoMId { get; set; }
            public string BaseUoMName { get; set; }

            public decimal MinStockLevel { get; set; }
            public bool IsPerishable { get; set; }
            public bool IsActive { get; set; }

            public DateTime CreatedAt { get; init; }
            public DateTime? UpdatedAt { get; init; }

            public int UoMIdFrom { get; set; }
            public string UoMFromName { get; set; }
            public int UoMIdTo { get; set; }
            public string UoMToName { get; set; }
            public decimal Factor { get; set; }
        }
    }
}
