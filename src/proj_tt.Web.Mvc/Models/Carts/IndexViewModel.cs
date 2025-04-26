using proj_tt.Carts.Dto;
using System.Collections.Generic;

namespace proj_tt.Web.Models.Carts
{
    public class IndexViewModel
    {
        public CartDto Cart { get; set; }
        public List<CartItemDto> CartItems { get; set; }

        public IndexViewModel()
        {
            CartItems = new List<CartItemDto>();
        }
    }
} 