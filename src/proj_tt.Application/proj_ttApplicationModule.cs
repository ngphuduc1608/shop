using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using proj_tt.Authorization;

namespace proj_tt
{
    [DependsOn(
        typeof(proj_ttCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class proj_ttApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<proj_ttAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(proj_ttApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
