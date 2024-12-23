﻿using System.ComponentModel.DataAnnotations;

namespace honaapi.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
           
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}
