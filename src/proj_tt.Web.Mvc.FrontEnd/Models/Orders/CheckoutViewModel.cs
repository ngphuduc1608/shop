using proj_tt.Carts.Dto;
using proj_tt.Addresses.Dto;
using System.Collections.Generic;

namespace proj_tt.Web.Models.Orders
{
    public class CheckoutViewModel
    {
        public CartDto Cart { get; set; }
        public List<AddressDto> Addresses { get; set; }

        public CheckoutViewModel(CartDto cart, List<AddressDto> addresses = null)
        {
            Cart = cart;
            Addresses = addresses ?? new List<AddressDto>();
        }
    }
}
