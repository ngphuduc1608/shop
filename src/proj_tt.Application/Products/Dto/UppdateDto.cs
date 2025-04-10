using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace proj_tt.Products.Dto
{
    public class UpdateProductDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(Product.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public IFormFile ImageUrl { get; set; } // Có thể giữ ảnh cũ hoặc upload ảnh mới

        public int Discount { get; set; }

        public string ExistingImageUrl { get; set; } // Lưu ảnh cũ

        public int CategoryId { get; set; }
        public string NameCategory { get; set; }
    }

}
