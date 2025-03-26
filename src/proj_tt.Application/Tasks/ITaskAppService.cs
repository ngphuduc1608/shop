using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System.Threading.Tasks;
using proj_tt.Tasks.Dto;

namespace proj_tt.Tasks
{
    public interface ITaskAppService : IApplicationService
    {
        Task<ListResultDto<TaskListDto>> GetAll(GetAllTasksInput input);
    }
}
