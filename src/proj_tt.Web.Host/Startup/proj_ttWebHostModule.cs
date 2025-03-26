using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using proj_tt.Configuration;

namespace proj_tt.Web.Host.Startup
{
    [DependsOn(
       typeof(proj_ttWebCoreModule))]
    public class proj_ttWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public proj_ttWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(proj_ttWebHostModule).GetAssembly());
        }
    }
}
