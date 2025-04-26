using Abp.Domain.Entities.Auditing;
using proj_tt.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proj_tt.Carts
{
    [Table("AppCarts")]
    public class Cart : AuditedEntity
    {
        public const int MaxNameLength = 256;

        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        public long UserId { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }

        public Cart(string name, long userId)
        {
            Name = name;
            UserId = userId;
            CartItems = new List<CartItem>();
        }
    }

    [Table("AppCartItems")]
    public class CartItem : AuditedEntity
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        [ForeignKey(nameof(CartId))]
        public virtual Cart Cart { get; set; }

        public CartItem(int cartId, int productId, int quantity)
        {
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
} 