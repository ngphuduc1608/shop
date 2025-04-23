using System.Collections.Generic;
using proj_tt.Roles.Dto;

namespace proj_tt.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
