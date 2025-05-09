using Abp.Application.Services;
using Abp.Application.Services.Dto;
using proj_tt.Orders.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace proj_tt.Orders
{
    public interface IOrderAppService : IApplicationService
    {
        Task<OrderDto> GetOrder(int id);
        Task<PagedResultDto<OrderDto>> GetUserOrders(PagedAndSortedResultRequestDto input, OrderStatus? status = null);
        Task<List<OrderDto>> GetAllOrders();
        Task<OrderDto> CreateOrder(CreateOrderInput input);
        Task<OrderDto> UpdateOrderStatus(UpdateOrderStatusInput input);
        Task DeleteOrder(int id);

        //dfgd
    }
}