using Abp.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using proj_tt.Carts;
using proj_tt.Carts.Dto;
using proj_tt.Web.Models.Carts;
using System.Threading.Tasks;

namespace proj_tt.Web.Controllers
{
    public class CartsController : AbpController
    {
        private readonly ICartAppService _cartAppService;

        public CartsController(ICartAppService cartAppService)
        {
            _cartAppService = cartAppService;
        }

        public async Task<ActionResult> Index()
        {
            var cart = await _cartAppService.GetCartAsync();
            var viewModel = new IndexViewModel
            {
                Cart = cart,
                CartItems = cart.CartItems
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart(AddToCartInput input)
        {
            var cart = await _cartAppService.AddToCartAsync(input);
            return Json(new { success = true, cart = cart });
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCartItem(UpdateCartItemInput input)
        {
            var cart = await _cartAppService.UpdateCartItemAsync(input);
            return Json(new { success = true, cart = cart });
        }

        [HttpPost]
        public async Task<ActionResult> RemoveFromCart(int cartItemId)
        {
            await _cartAppService.RemoveFromCartAsync(cartItemId);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<ActionResult> ClearCart()
        {
            await _cartAppService.ClearCartAsync();
            return Json(new { success = true });
        }
    }
} 