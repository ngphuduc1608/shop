using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public float Price { get; set; }

        public IFormFile ImageUrl { get; set; } // Có thể giữ ảnh cũ hoặc upload ảnh mới

        public int Discount { get; set; }

        public string ExistingImageUrl { get; set; } // Lưu ảnh cũ
    }

}
