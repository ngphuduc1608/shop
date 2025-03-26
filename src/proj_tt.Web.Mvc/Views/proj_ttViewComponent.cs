using Abp.AspNetCore.Mvc.ViewComponents;

namespace proj_tt.Web.Views
{
    public abstract class proj_ttViewComponent : AbpViewComponent
    {
        protected proj_ttViewComponent()
        {
            LocalizationSourceName = proj_ttConsts.LocalizationSourceName;
        }
    }
}
