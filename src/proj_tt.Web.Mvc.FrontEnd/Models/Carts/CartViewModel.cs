using proj_tt.Carts.Dto;

namespace proj_tt.Web.Models.Carts
{
    public class CartViewModel
    {
        public CartDto Cart { get; set; }

        public CartViewModel(CartDto cart)
        {
            Cart = cart;
        }
    }
} 