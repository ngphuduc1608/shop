using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using proj_tt.Controllers;

namespace proj_tt.Web.Controllers
{
    [AbpMvcAuthorize]
    //[AllowAnonymous]
    public class HomeController : proj_ttControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
