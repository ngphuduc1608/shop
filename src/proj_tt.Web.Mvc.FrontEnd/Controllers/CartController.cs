using Microsoft.AspNetCore.Mvc;
using proj_tt.Carts;
using proj_tt.Carts.Dto;
using proj_tt.Controllers;
using proj_tt.Web.Models.Carts;
using System.Linq;
using System.Threading.Tasks;

namespace proj_tt.Web.Controllers
{
    public class CartController : proj_ttControllerBase
    {
        private readonly ICartAppService _cartAppService;

        public CartController(ICartAppService cartAppService)
        {
            _cartAppService = cartAppService;
        }

        public async Task<ActionResult> Index()
        {
            var cart = await _cartAppService.GetCartAsync();
            var model = new CartViewModel(cart);
            return View(model);
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

        [HttpGet]
        public async Task<ActionResult> GetCartCount()
        {
            var cart = await _cartAppService.GetCartAsync();
            var count = cart.CartItems.Sum(item => item.Quantity);
            return Content(count.ToString());
        }
    }
}