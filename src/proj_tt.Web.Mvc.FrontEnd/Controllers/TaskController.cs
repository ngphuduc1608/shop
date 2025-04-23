using Microsoft.AspNetCore.Mvc;
using proj_tt.Tasks.Dto;
using proj_tt.Tasks;
using System.Threading.Tasks;
using proj_tt.Controllers;
using proj_tt.Web.Models.Tasks;
using proj_tt.Persons;
using System.Linq;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using proj_tt.Web.Models.Persons;

namespace proj_tt.Web.Controllers
{
    public class TasksController : proj_ttControllerBase
    {
        private readonly ITaskAppService _taskAppService;
        private readonly ILookupAppService _lookupAppService;


        public TasksController(ITaskAppService taskAppService, ILookupAppService lookupAppService)
        {
            _taskAppService = taskAppService;
            _lookupAppService = lookupAppService;

        }

        public async Task<ActionResult> Index(GetAllTasksInput input)
        {
            var output = await _taskAppService.GetAll(input);
            var model = new IndexViewModel(output.Items)
            {
                SelectedTaskState=input.State
            };
            return View(model);
        }

        public async Task<ActionResult> Create()
        {
            var peopleSelectListItems=(await _lookupAppService.GetPeopleComboboxItems()).Items
                .Select(p=>p.ToSelectListItem()).ToList();

            peopleSelectListItems.Insert(0,new SelectListItem { Value=string.Empty,Text="Unassigned",Selected=true});

            return View(new CreateTaskViewModel(peopleSelectListItems));
        }
    }
}
