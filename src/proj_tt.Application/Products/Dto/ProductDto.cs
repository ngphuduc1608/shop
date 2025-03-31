using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj_tt.Products.Dto
{
    [AutoMapFrom(typeof(Product))]
    public class ProductDto:AuditedEntityDto
    {
        public string Name { get; set; }

        public float Price { get; set; }

        public string ImageUrl { get; set; }
        public int Discount { get; set; }
    }
}
