using System.Threading.Tasks;
using Abp.Application.Services;
using proj_tt.Authorization.Accounts.Dto;

namespace proj_tt.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
