using Abp.AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj_tt.Products.Dto
{
    [AutoMapTo(typeof(Product))]

    public class ProductListDto
    {
        //public int? Id { get; set; }

        [Required]
        [StringLength(Product.MaxNameLength)]
        public string Name { get; set; }
        [Required]

        public float Price { get; set; }
        //[Required]

        public IFormFile ImageUrl { get; set; }

        public int Discount { get; set; }


    }
}
