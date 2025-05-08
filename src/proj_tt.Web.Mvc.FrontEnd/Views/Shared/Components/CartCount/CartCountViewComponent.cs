using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proj_tt.Carts;
using System.Linq;
using System.Threading.Tasks;

namespace proj_tt.Web.Views.Shared.Components.CartCount
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly IRepository<CartItem, int> _cartItemRepository;
        private readonly IAbpSession _abpSession;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CartCountViewComponent(
            IRepository<CartItem, int> cartItemRepository,
            IAbpSession abpSession,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _cartItemRepository = cartItemRepository;
            _abpSession = abpSession;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!_abpSession.UserId.HasValue)
            {
                return View(0);
            }

            using (var uow = _unitOfWorkManager.Begin())
            {
                var count = await _cartItemRepository
                    .GetAll()
                    .Where(x => x.Cart.UserId == _abpSession.UserId.Value)
                    .CountAsync();

                await uow.CompleteAsync();
                return View(count);
            }
        }
    }
}