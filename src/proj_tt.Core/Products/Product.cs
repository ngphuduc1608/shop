using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj_tt.Products
{
    [Table("AppProducts")]
    public class Product : AuditedEntity
    {

        public const int MaxNameLength = 256;

        

        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get ; set ; }
        [Required]

        public float Price {  get; set ; }

        //[Required]

        public string ImageUrl {  get; set ; }

        public int Discount {  get; set ; }

        public Product()
        {
        }

        public Product(string name, float price, string imageUrl, int discount)
        {
            Name = name;
            Price = price;
            ImageUrl = imageUrl;
            Discount = discount;

        }
    }
}
