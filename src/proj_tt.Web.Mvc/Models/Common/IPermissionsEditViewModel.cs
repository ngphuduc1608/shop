using System.Collections.Generic;
using proj_tt.Roles.Dto;

namespace proj_tt.Web.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }
    }
}