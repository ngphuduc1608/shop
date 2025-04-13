using Abp.Localization;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero;
using Abp.Zero.Configuration;
using proj_tt.Authorization.Roles;
using proj_tt.Authorization.Users;
using proj_tt.Configuration;
using proj_tt.Localization;
using proj_tt.MultiTenancy;
using proj_tt.Timing;

namespace proj_tt
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class proj_ttCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            proj_ttLocalizationConfigurer.Configure(Configuration.Localization);

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = proj_ttConsts.MultiTenancyEnabled;

            // Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            Configuration.Settings.Providers.Add<AppSettingProvider>();

            Configuration.Localization.Languages.Add(new LanguageInfo("fa", "فارسی", "famfamfam-flags ir"));
            Configuration.Localization.Languages.Add(new LanguageInfo("fa", "Tiếng Việt", "famfamfam-flags vn"));


            Configuration.Settings.SettingEncryptionConfiguration.DefaultPassPhrase = proj_ttConsts.DefaultPassPhrase;
            SimpleStringCipher.DefaultPassPhrase = proj_ttConsts.DefaultPassPhrase;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(proj_ttCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
