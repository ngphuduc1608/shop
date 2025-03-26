using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using proj_tt.EntityFrameworkCore;
using proj_tt.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace proj_tt.Web.Tests
{
    [DependsOn(
        typeof(proj_ttWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class proj_ttWebTestModule : AbpModule
    {
        public proj_ttWebTestModule(proj_ttEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(proj_ttWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(proj_ttWebMvcModule).Assembly);
        }
    }
}