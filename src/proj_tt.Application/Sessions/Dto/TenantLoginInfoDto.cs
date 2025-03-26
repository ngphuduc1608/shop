using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using proj_tt.MultiTenancy;

namespace proj_tt.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
