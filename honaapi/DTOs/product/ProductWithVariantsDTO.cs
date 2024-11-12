using honaapi.DTOs.variant;

namespace honaapi.DTOs.product
{
    public class ProductWithVariantsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public int ImageId { get; set; }
        public int? Sold { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<VariantProductResponseDTO> VariantProducts { get; set; }
    }
}
