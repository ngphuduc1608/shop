using System.Threading.Tasks;
using Abp.Application.Services;
using proj_tt.Sessions.Dto;

namespace proj_tt.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
