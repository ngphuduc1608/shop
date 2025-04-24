using proj_tt.Carts.Dto;
using System.Threading.Tasks;

namespace proj_tt.Carts
{
    public interface ICartAppService
    {
        Task<CartDto> GetCartAsync();
        Task<CartDto> AddToCartAsync(AddToCartInput input);
        Task<CartDto> UpdateCartItemAsync(UpdateCartItemInput input);
        Task RemoveFromCartAsync(int cartItemId);
        Task ClearCartAsync();
        Task<CartItemDto> GetCartItemByProductIdAsync(int productId);
    }
} 