using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace proj_tt.Products.Dto
{
    [AutoMapFrom(typeof(Product))]
    public class ProductDto:AuditedEntityDto
    {
        public string Name { get; set; }

        public float Price { get; set; }

        public string ImageUrl { get; set; }
        public int Discount { get; set; }
        public int CategoryId { get; set; }
        public string NameCategory { get; set; }
    }
}
