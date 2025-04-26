using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace proj_tt.Authorization
{
    public class proj_ttAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

            var productPermission = context.CreatePermission(PermissionNames.Pages_Products, L("Products"));
            productPermission.CreateChildPermission(PermissionNames.Pages_Products_Create, L("CreateProduct"));
            productPermission.CreateChildPermission(PermissionNames.Pages_Products_Edit, L("EditProduct"));
            productPermission.CreateChildPermission(PermissionNames.Pages_Products_Delete, L("DeleteProduct"));

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, proj_ttConsts.LocalizationSourceName);
        }
    }
}
