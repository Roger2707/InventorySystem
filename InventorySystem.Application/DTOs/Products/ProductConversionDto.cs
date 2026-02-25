namespace InventorySystem.Application.DTOs.Products
{
    public class ProductConversionDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int UoMIdFrom { get; set; }
        public string UoMFromName { get; set; }
        public int UoMIdTo { get; set; }
        public string UoMToName { get; set; }
        public decimal Factor { get; set; }
    }
}
