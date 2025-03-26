using Abp.Authorization;
using proj_tt.Authorization.Roles;
using proj_tt.Authorization.Users;

namespace proj_tt.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
