using Abp.Application.Services;
using proj_tt.Carts.Dto;
using System.Threading.Tasks;

namespace proj_tt.Carts
{
    public interface ICartAppService : IApplicationService
    {
        Task<CartDto> GetCartAsync();
        Task<CartDto> AddToCartAsync(AddToCartInput input);
        Task<CartDto> UpdateCartItemAsync(UpdateCartItemInput input);
        Task RemoveFromCartAsync(int cartItemId);
        Task ClearCartAsync();

        // ch?c n?ng buy now
        Task<CartItemDto> GetCartItemByProductIdAsync(int productId);
    }
}