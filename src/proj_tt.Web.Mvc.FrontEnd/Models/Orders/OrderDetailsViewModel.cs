using proj_tt.Orders.Dto;

namespace proj_tt.Web.Models.Orders
{
    public class OrderDetailsViewModel
    {
        public OrderDto Order { get; set; }

        public OrderDetailsViewModel(OrderDto order)
        {
            Order = order;
        }
    }
}
