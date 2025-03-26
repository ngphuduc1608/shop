using Microsoft.AspNetCore.Mvc;
using proj_tt.Controllers;
using proj_tt.Tasks;
using proj_tt.Tasks.Dto;
using proj_tt.Web.Models.Tasks;
using System.Threading.Tasks;

namespace proj_tt.Web.Controllers
{
    public class TaskController: proj_ttControllerBase
    {
        private readonly ITaskAppService _taskAppService;

        public TaskController(ITaskAppService taskAppService)
        {
            _taskAppService = taskAppService;
        }

        public async Task<ActionResult> Index(GetAllTasksInput input)
        {
            var output= await _taskAppService.GetAll(input);
            var model = new ViewModel(output.Items);
            return View(output);
        }
    }
    
}
