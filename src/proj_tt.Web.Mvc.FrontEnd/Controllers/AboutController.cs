using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using proj_tt.Controllers;

namespace proj_tt.Web.Controllers
{
    [AbpMvcAuthorize]
    public class AboutController : proj_ttControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
