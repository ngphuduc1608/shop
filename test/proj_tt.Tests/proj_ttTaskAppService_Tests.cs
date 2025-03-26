//using proj_tt.Tasks;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using System.Threading.Tasks;
using Xunit;

namespace proj_tt.Tests
{
    public class  TaskAppService_Tests: proj_ttTestBase 
    {
        private readonly ITaskAppService _taskAppService;
        public TaskAppService_Tests()
        {
            _taskAppService = Resolve<ITaskAppService>();
        }

        [Fact]
        public async Task Should_Get_All_Tasks()
        {
            var output= await _taskAppService.GetAll(new Tasks.GetAllTasksInput());

            output.Items.Count.ShouldBe(2);
        }

        [Fact]
        public async Task Should_Get_Filtered_Tasks()
        {
            var output = await _taskAppService.GetAll(new Tasks.GetAllTasksInput { State = TaskState.Open });

            output.Items.ShouldAllBe(t => t.State == TaskState.Open);
        }
    }
}
