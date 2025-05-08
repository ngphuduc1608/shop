using Abp.AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace proj_tt.Products.Dto
{
    [AutoMapTo(typeof(Product))]

    public class ProductListDto
    {

        [Required]
        [StringLength(Product.MaxNameLength)]
        public string Name { get; set; }
        [Required]

        public decimal Price { get; set; }
        //[Required]

        public IFormFile ImageUrl { get; set; }

        public int Discount { get; set; }

        public int CategoryId { get; set; }
        public string NameCategory { get; set; }


        public DateTime ProductionDate { get; set; }
        public int Stock { get; set; }

    }
}
