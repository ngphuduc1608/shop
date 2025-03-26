using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace proj_tt.Controllers
{
    public abstract class proj_ttControllerBase: AbpController
    {
        protected proj_ttControllerBase()
        {
            LocalizationSourceName = proj_ttConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
