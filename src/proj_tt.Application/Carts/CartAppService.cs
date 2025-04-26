using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using proj_tt.Carts.Dto;
using proj_tt.Products;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace proj_tt.Carts
{
    public class CartAppService : ApplicationService, ICartAppService
    {
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<CartItem> _cartItemRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IAbpSession _abpSession;

        public CartAppService(
            IRepository<Cart> cartRepository,
            IRepository<CartItem> cartItemRepository,
            IRepository<Product> productRepository,
            IAbpSession abpSession)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _abpSession = abpSession;
        }

        public async Task<CartDto> GetCartAsync()
        {
            var userId = _abpSession.UserId;
            if (userId == null)
            {
                throw new ApplicationException("User not logged in");
            }

            var cart = await _cartRepository.GetAll()
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart("Default Cart", userId.Value);
                await _cartRepository.InsertAsync(cart);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            return ObjectMapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> AddToCartAsync(AddToCartInput input)
        {
            var userId = _abpSession.UserId;
            if (userId == null)
            {
                throw new ApplicationException("User not logged in");
            }

            var cart = await _cartRepository.GetAll()
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart("Default Cart", userId.Value);
                await _cartRepository.InsertAsync(cart);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            // n?u sp có r?i thì t?ng sl , không thì thêm vào cart
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == input.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += input.Quantity;
                await _cartItemRepository.UpdateAsync(existingItem);
            }
            else
            {
                var cartItem = new CartItem(cart.Id, input.ProductId, input.Quantity);
                await _cartItemRepository.InsertAsync(cartItem);
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            // tr? v? c?p nh?t gi? 
            return await GetCartAsync();
        }

        public async Task<CartDto> UpdateCartItemAsync(UpdateCartItemInput input)
        {
            var cartItem = await _cartItemRepository.GetAsync(input.CartItemId);
            cartItem.Quantity = input.Quantity;

            //l?u db khi thay ??i sl
            await _cartItemRepository.UpdateAsync(cartItem);
            await CurrentUnitOfWork.SaveChangesAsync();

            return await GetCartAsync();
        }

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            await _cartItemRepository.DeleteAsync(cartItemId);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task ClearCartAsync()
        {
            var userId = _abpSession.UserId;
            if (userId == null)
            {
                throw new ApplicationException("User not logged in");
            }

            var cart = await _cartRepository.GetAll()
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                foreach (var item in cart.CartItems.ToList())
                {
                    await _cartItemRepository.DeleteAsync(item);
                }
                await CurrentUnitOfWork.SaveChangesAsync();
            }
        }

        public async Task<CartItemDto> GetCartItemByProductIdAsync(int productId)
        {
            var userId = _abpSession.UserId;
            if (userId == null)
            {
                throw new ApplicationException("User not logged in");
            }

            var cart = await _cartRepository.GetAll()
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return null;
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            return cartItem != null ? ObjectMapper.Map<CartItemDto>(cartItem) : null;
        }
    }
}