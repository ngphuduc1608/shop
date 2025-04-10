using Abp.Domain.Entities.Auditing;
using proj_tt.Categories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proj_tt.Products
{
    [Table("AppProducts")]
    public class Product : AuditedEntity
    {

        public const int MaxNameLength = 256;

        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }


        [Required]
        public decimal Price { get; set; }

        [Required]


        public string ImageUrl { get; set; }

        public int Discount { get; set; }


        [ForeignKey(nameof(CategoryId))]

        public virtual Category Category { get; set; }

        public int? CategoryId { get; set; }


        public Product(string name, decimal price, string imageUrl, int discount = 0, int? categoryId = null)
        {
            Name = name;
            Price = price;
            ImageUrl = imageUrl;
            Discount = discount;
            CategoryId = categoryId;
        }

    }
}
