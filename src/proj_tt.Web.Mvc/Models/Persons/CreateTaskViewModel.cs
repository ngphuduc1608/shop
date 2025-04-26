using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace proj_tt.Web.Models.Persons
{
    public class CreateTaskViewModel
    {
        public List<SelectListItem> People { get; set; }

        public CreateTaskViewModel(List<SelectListItem> people)
        {
            People = people;
        }
    }
}
