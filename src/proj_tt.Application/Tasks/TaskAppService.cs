using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using AutoMapper.Internal.Mappers;
using Microsoft.EntityFrameworkCore;
using proj_tt.Tasks.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj_tt.Tasks
{
    public class TaskAppService : proj_ttAppServiceBase, ITaskAppService
    {
        private readonly IRepository<Task> _taskRepository;
        


        public TaskAppService(IRepository<Task> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async System.Threading.Tasks.Task Create(CreateTaskInput input)
        {
            var task = ObjectMapper.Map<Task>(input);
            await _taskRepository.InsertAsync(task);
        }

        public async Task<ListResultDto<TaskListDto>> GetAll(GetAllTasksInput input)
        {
            var tasks = await _taskRepository
                .GetAll()
                .Include(t=>t.AssignedPerson)
                .WhereIf(input.State.HasValue, t => t.State == input.State.Value)
                .OrderByDescending(t => t.CreationTime)
                .ToListAsync();

            return new ListResultDto<TaskListDto>(
                ObjectMapper.Map<List<TaskListDto>>(tasks)
            );
        }



    }
}
