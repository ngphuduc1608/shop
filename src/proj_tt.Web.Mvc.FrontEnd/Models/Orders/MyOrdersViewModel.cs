using Abp.Application.Services.Dto;
using proj_tt.Orders;
using proj_tt.Orders.Dto;

namespace proj_tt.Web.Models.Orders
{
    public class MyOrdersViewModel
    {
        public PagedResultDto<OrderDto> Orders { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public OrderStatus? SelectedStatus { get; set; }

        public MyOrdersViewModel(PagedResultDto<OrderDto> orders, int currentPage = 1, int pageSize = 10, OrderStatus? selectedStatus = null)
        {
            Orders = orders;
            CurrentPage = currentPage;
            PageSize = pageSize;
            SelectedStatus = selectedStatus;
        }
    }
}