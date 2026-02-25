namespace InventorySystem.Application.DTOs.Products
{
    public class UpdateProductDto
    {
        public string Name { get; init; }
        public string Barcode { get; set; }
        public int CategoryId { get; set; }
        public int BaseUoMId { get; set; }
        public decimal MinStockLevel { get; set; }
        public List<UpdateProductUOMConversionDto>? ConversionDtos { get; set; } = new();
    }

    public class UpdateProductUOMConversionDto
    {
        public int FromUoMId { get; set; }
        public int ToUoMId { get; set; }
        public decimal Factor { get; set; }
    }
}
