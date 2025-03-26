using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using System;
using System.Linq;
using System.Text;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;

namespace proj_tt.Tasks
{
    
    public class TaskAppService : proj_ttAppServiceBase, ITaskAppService
    {
        private readonly IRepository<Task> _taskRepository;

        public TaskAppService(IRepository<Task> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<ListResultDto<TaskListDto>> GetAll(GetAllTasksInput input)
        {
            var tasks = await _taskRepository.GetAll()
                .WhereIf(input.State.HasValue, t => t.State == input.State.Value)
                .OrderByDescending(t => t.CreationTime)
                .ToListAsync();

            return new ListResultDto<TaskListDto>(ObjectMapper.Map<List<TaskListDto>>(tasks));
        }
    }
}
