using Abp.Application.Services;
using proj_tt.MultiTenancy.Dto;

namespace proj_tt.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

