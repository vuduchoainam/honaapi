using honaapi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace honaapi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(500, ErrorMessage = "Tên sản phẩm không vượt quá 500 ký tự")]
        public string Name { get; set; } = string.Empty;

        public string? Slug { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal BasePrice { get; set; }

        public virtual ICollection<VariantProduct>  VariantProducts { get; set; } = new List<VariantProduct>();

        public int? Sold { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        //Relationship: Many to one
        public Category? Category { get; set; }
        public int CategoryId { get; set; }

        public Brand? Brand { get; set; }
        public int BrandId { get; set; }

        public UploadImage? Image { get; set; }
        public int ImageId { get; set; }
    }
}
