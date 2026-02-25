namespace InventorySystem.Application.DTOs.Products
{
    public class ProductDto
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

        public List<ProductConversionDto> ConversionDtos { get; set; } = new();
    }
}
