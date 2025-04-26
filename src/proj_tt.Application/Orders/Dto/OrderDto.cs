using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using proj_tt.Products.Dto;
using System.Collections.Generic;

namespace proj_tt.Orders.Dto
{
    [AutoMapFrom(typeof(Order))]
    public class OrderDto : AuditedEntityDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public long UserId { get; set; }
        public string Username { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }

    [AutoMapFrom(typeof(OrderItem))]
    public class OrderItemDto : AuditedEntityDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public ProductDto Product { get; set; }
    }



    public class OrderItemInput
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateOrderStatusInput
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}