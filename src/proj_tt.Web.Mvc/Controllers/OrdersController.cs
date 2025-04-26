using Abp.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using proj_tt.Orders;
using proj_tt.Orders.Dto;
using proj_tt.Web.Models.Orders;
using System.Threading.Tasks;

namespace proj_tt.Web.Controllers
{
    public class OrdersController : AbpController
    {
        private readonly IOrderAppService _orderAppService;

        public OrdersController(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }

        public async Task<ActionResult> Index()
        {
            var orders = await _orderAppService.GetAllOrders();
            var viewModel = new IndexViewModel
            {
                Orders = orders
            };
            return View(viewModel);
        }

        public async Task<ActionResult> Details(int id)
        {
            var order = await _orderAppService.GetOrder(id);
            var viewModel = new DetailsViewModel(order);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(CreateOrderInput input)
        {
            var order = await _orderAppService.CreateOrder(input);
            return Json(new { success = true, order = order });
        }

        [HttpPost]
        public async Task<ActionResult> UpdateOrderStatus(UpdateOrderStatusInput input)
        {
            var order = await _orderAppService.UpdateOrderStatus(input);
            return Json(new { success = true, order = order });
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            await _orderAppService.DeleteOrder(id);
            return Json(new { success = true });
        }
    }
}