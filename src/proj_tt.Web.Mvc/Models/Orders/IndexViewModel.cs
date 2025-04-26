using proj_tt.Orders.Dto;
using System.Collections.Generic;

namespace proj_tt.Web.Models.Orders
{
    public class IndexViewModel
    {
        public OrderDto Order { get; set; }
        public List<OrderDto> Orders { get; set; }

        public IndexViewModel()
        {
            Orders = new List<OrderDto>();
        }
    }
} 