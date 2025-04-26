using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using proj_tt.Tasks.Dto;
using System.Threading.Tasks;


namespace proj_tt.Tasks
{
    public interface ITaskAppService : IApplicationService
    {
        Task<ListResultDto<TaskListDto>> GetAll(GetAllTasksInput input);

        System.Threading.Tasks.Task Create(CreateTaskInput input);
    }
}
