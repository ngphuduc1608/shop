using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace proj_tt.Web.Views
{
    public abstract class proj_ttRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected proj_ttRazorPage()
        {
            LocalizationSourceName = proj_ttConsts.LocalizationSourceName;
        }
    }
}
