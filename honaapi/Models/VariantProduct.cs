using honaapi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace honaapi.Models
{
    public class VariantProduct
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public ProductStatus Status { get; set; } = ProductStatus.InStock;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        //Relationship: Many to one
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
