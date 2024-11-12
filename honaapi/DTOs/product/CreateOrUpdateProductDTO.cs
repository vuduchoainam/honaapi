using honaapi.DTOs.variant;

namespace honaapi.DTOs.product
{
    public class CreateOrUpdateProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int Sold {  get; set; }
        public int? ImageId { get; set; }
        public List<CreateOrUpdateVariantDTO> Variants { get; set; } = new List<CreateOrUpdateVariantDTO>();

    }
}
