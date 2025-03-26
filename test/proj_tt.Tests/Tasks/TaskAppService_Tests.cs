using proj_tt.Tasks;
using proj_tt.Tasks.Dto;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace proj_tt.Tests.Tasks
{
    public class TaskAppService_Tests: proj_ttTestBase
    {
        private readonly ITaskAppService _taskAppService;
        public TaskAppService_Tests()
        {
            _taskAppService = Resolve<ITaskAppService>();
        }
        [Fact]

        public async System.Threading.Tasks.Task Should_Get_All_Tasks()
        {
            
            var output = await _taskAppService.GetAll(new GetAllTasksInput());
            
            output.Items.Count.ShouldBe(2);
        }
        [Fact]
        public async System.Threading.Tasks.Task Should_Get_Filtered_Tasks()
        {

            var output = await _taskAppService.GetAll(new GetAllTasksInput { State=TaskState.Completed});

            output.Items.ShouldAllBe(t => t.State == TaskState.Open);
        }
    }
}
