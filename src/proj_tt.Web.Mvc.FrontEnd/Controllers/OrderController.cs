using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using proj_tt.Addresses;
using proj_tt.Carts;
using proj_tt.Carts.Dto;
using proj_tt.Controllers;
using proj_tt.Orders;
using proj_tt.Products;
using proj_tt.Web.Models.Orders;
using System.Linq;
using System.Threading.Tasks;

namespace proj_tt.Web.Controllers
{
    public class OrderController : proj_ttControllerBase
    {
        private readonly IOrderAppService _orderAppService;
        private readonly ICartAppService _cartAppService;
        private readonly IAddressAppService _addressAppService;
        private readonly IProductAppService _productAppService;

        public OrderController(
            IOrderAppService orderAppService,
            ICartAppService cartAppService,
            IAddressAppService addressAppService,
            IProductAppService productAppService)
        {
            _orderAppService = orderAppService;
            _cartAppService = cartAppService;
            _addressAppService = addressAppService;
            _productAppService = productAppService;
        }

        public async Task<ActionResult> Checkout(int? productId = null, int? quantity = null, string selectedItems = null)
        {
            var addresses = await _addressAppService.GetUserAddresses();

            if (productId.HasValue && quantity.HasValue)
            {
                // Direct checkout with product and quantity
                var product = await _productAppService.GetProducts(productId.Value);
                if (product == null)
                {
                    return RedirectToAction("Index", "Product");
                }

                var cartItem = new CartItemDto
                {
                    ProductId = product.Id,
                    Product = product,
                    Quantity = quantity.Value
                };

                var model = new CheckoutViewModel(
                    new CartDto
                    {
                        CartItems = new System.Collections.Generic.List<CartItemDto> { cartItem }
                    },
                    addresses
                );
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

                var model = new CheckoutViewModel(
                    new CartDto
                    {
                        CartItems = filteredItems
                    },
                    addresses
                );
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

                var model = new CheckoutViewModel(cart, addresses);
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

        public async Task<ActionResult> MyOrders(int page = 1, int pageSize = 10, OrderStatus? status = null)
        {
            var input = new PagedAndSortedResultRequestDto
            {
                SkipCount = (page - 1) * pageSize,
                MaxResultCount = pageSize
            };

            var orders = await _orderAppService.GetUserOrders(input, status);
            var model = new MyOrdersViewModel(orders, page, pageSize, status);
            return View(model);
        }
    }
}
