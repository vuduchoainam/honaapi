using System.ComponentModel.DataAnnotations;

namespace honaapi.Models
{
    public class UploadImage
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }


    }
}
