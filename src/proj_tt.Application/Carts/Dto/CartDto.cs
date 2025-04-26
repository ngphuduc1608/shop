using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using proj_tt.Products.Dto;
using System.Collections.Generic;

namespace proj_tt.Carts.Dto
{
    [AutoMapFrom(typeof(Cart))]
    public class CartDto : AuditedEntityDto
    {
        public string Name { get; set; }
        public long UserId { get; set; }
        public List<CartItemDto> CartItems { get; set; }
    }

    [AutoMapFrom(typeof(CartItem))]
    public class CartItemDto : AuditedEntityDto
    {
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public ProductDto Product { get; set; }
    }

    public class AddToCartInput
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateCartItemInput
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
} 