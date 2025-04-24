using proj_tt.Carts.Dto;

namespace proj_tt.Web.Models.Orders
{
    public class CheckoutViewModel
    {
        public CartDto Cart { get; set; }

        public CheckoutViewModel(CartDto cart)
        {
            Cart = cart;
        }
    }
}
