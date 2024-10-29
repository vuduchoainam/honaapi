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
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        public int Quantity { get; set; }

        public virtual ICollection<UploadImage> Images { get; set; } = new List<UploadImage>();

        public ProductStatus Status { get; set; } = ProductStatus.InStock;

        public int Sold { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        //Relationship: Many to one
        public Category? Category { get; set; }
        public int CategoryId { get; set; }

        public Brand? Brand { get; set; }
        public int BrandId { get; set; }
    }
}
