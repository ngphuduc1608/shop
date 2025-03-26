using System.Collections.Generic;
using proj_tt.Roles.Dto;

namespace proj_tt.Web.Models.Roles
{
    public class RoleListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}
