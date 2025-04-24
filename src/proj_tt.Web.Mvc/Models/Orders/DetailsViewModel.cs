using proj_tt.Orders;
using proj_tt.Orders.Dto;
using System.Collections.Generic;

namespace proj_tt.Web.Models.Orders
{
    public class DetailsViewModel
    {
        public OrderDto Order { get; set; }
        public List<OrderStatus> AvailableStatuses { get; set; }

        public DetailsViewModel(OrderDto order)
        {
            Order = order;
            AvailableStatuses = new List<OrderStatus>
            {
                OrderStatus.Pending,
                OrderStatus.Processing,
                OrderStatus.Shipped,
                OrderStatus.Delivered,
                OrderStatus.Cancelled
            };
        }
    }
}