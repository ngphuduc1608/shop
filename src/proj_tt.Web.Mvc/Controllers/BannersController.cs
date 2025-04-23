using Microsoft.AspNetCore.Mvc;
using proj_tt.Banners;
using proj_tt.Controllers;
using System.Threading.Tasks;

namespace proj_tt.Web.Views.Banners
{
    public class BannersController : proj_ttControllerBase
    {
        private readonly IBannerAppService _bannerAppService;

        public BannersController(IBannerAppService bannerAppService)
        {
            _bannerAppService = bannerAppService;
        }

        public async Task<IActionResult> Index()
        {
            var banners = await _bannerAppService.GetListBanner();
            return View(banners);
        }
    }
}
