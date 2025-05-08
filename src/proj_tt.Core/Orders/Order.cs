using Abp.Domain.Entities.Auditing;
using proj_tt.Products;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proj_tt.Orders
{
    [Table("AppOrders")]
    public class Order : AuditedEntity
    {
        public const int MaxNameLength = 256;
        public const int MaxAddressLength = 512;
        public const int MaxPhoneLength = 20;

        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(MaxAddressLength)]
        public string Address { get; set; }

        [Required]
        [StringLength(MaxPhoneLength)]
        public string Phone { get; set; }

        public long UserId { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public Order(string name, string address, string phone, long userId)
        {
            Name = name;
            Address = address;
            Phone = phone;
            UserId = userId;
            Status = OrderStatus.Pending;
            OrderItems = new List<OrderItem>();
        }
    }

    [Table("AppOrderItems")]
    public class OrderItem : AuditedEntity
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; }

        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; }

        public OrderItem(int orderId, int productId, int quantity, decimal unitPrice)
        {
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = quantity * unitPrice;
        }
    }

    public enum OrderStatus
    {
        Pending = 0,
        Processing = 1,
        Shipped = 2,
        Delivered = 3,
        Cancelled = 4
    }
}