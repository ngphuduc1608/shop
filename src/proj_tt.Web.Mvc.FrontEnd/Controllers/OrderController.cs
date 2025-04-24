using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using proj_tt.Carts;
using proj_tt.Carts.Dto;
using proj_tt.Controllers;
using proj_tt.Orders;
using proj_tt.Web.Models.Orders;
using System.Linq;
using System.Threading.Tasks;

namespace proj_tt.Web.Controllers
{
    public class OrderController : proj_ttControllerBase
    {
        private readonly IOrderAppService _orderAppService;
        private readonly ICartAppService _cartAppService;

        public OrderController(IOrderAppService orderAppService, ICartAppService cartAppService)
        {
            _orderAppService = orderAppService;
            _cartAppService = cartAppService;
        }

        public async Task<ActionResult> Checkout(int? productId = null, string selectedItems = null)
        {
            if (productId.HasValue)
            {
                // Single product checkout
                var cartItem = await _cartAppService.GetCartItemByProductIdAsync(productId.Value);
                if (cartItem == null)
                {
                    return RedirectToAction("Index", "Cart");
                }

                var model = new CheckoutViewModel(new CartDto
                {
                    CartItems = new System.Collections.Generic.List<CartItemDto> { cartItem }
                });
                return View(model);
            }
            else if (!string.IsNullOrEmpty(selectedItems))
            {
                // Checkout with selected items from cart
                var cart = await _cartAppService.GetCartAsync();
                if (cart.CartItems.Count == 0)
                {
                    return RedirectToAction("Index", "Cart");
                }

                // Filter cart items based on selected IDs
                var selectedIds = selectedItems.Split(',').Select(int.Parse).ToList();
                var filteredItems = cart.CartItems.Where(item => selectedIds.Contains(item.Id)).ToList();

                if (filteredItems.Count == 0)
                {
                    return RedirectToAction("Index", "Cart");
                }

                var model = new CheckoutViewModel(new CartDto
                {
                    CartItems = filteredItems
                });
                return View(model);
            }
            else
            {
                // Regular cart checkout
                var cart = await _cartAppService.GetCartAsync();
                if (cart.CartItems.Count == 0)
                {
                    return RedirectToAction("Index", "Cart");
                }

                var model = new CheckoutViewModel(cart);
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(CreateOrderInput input)
        {
            var order = await _orderAppService.CreateOrder(input);
            await _cartAppService.ClearCartAsync();
            return RedirectToAction("Details", new { orderId = order.Id });
        }

        public async Task<ActionResult> Details(int orderId)
        {
            var order = await _orderAppService.GetOrder(orderId);
            var model = new OrderDetailsViewModel(order);
            return View(model);
        }

        public async Task<ActionResult> MyOrders(int page = 1, int pageSize = 10)
        {
            var input = new PagedAndSortedResultRequestDto
            {
                SkipCount = (page - 1) * pageSize,
                MaxResultCount = pageSize
            };

            var orders = await _orderAppService.GetUserOrders(input);
            var model = new MyOrdersViewModel(orders, page, pageSize);
            return View(model);
        }
    }
}
