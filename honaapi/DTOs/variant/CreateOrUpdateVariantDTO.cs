using honaapi.Models.Enums;

namespace honaapi.DTOs.variant
{
    public class CreateOrUpdateVariantDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int InventoryQuantity { get; set; }
        public ProductStatus Status { get; set; } = ProductStatus.InStock;
    }
}
