using Microsoft.AspNetCore.Mvc;
using proj_tt.Tasks.Dto;
using proj_tt.Tasks;
using System.Threading.Tasks;
using proj_tt.Controllers;
using proj_tt.Web.Models.Tasks;

namespace proj_tt.Web.Controllers
{
    public class TasksController : proj_ttControllerBase
    {
        private readonly ITaskAppService _taskAppService;

        public TasksController(ITaskAppService taskAppService)
        {
            _taskAppService = taskAppService;
        }

        public async Task<ActionResult> Index(GetAllTasksInput input)
        {
            var output = await _taskAppService.GetAll(input);
            var model = new IndexViewModel(output.Items);
            return View(model);
        }
    }
}
