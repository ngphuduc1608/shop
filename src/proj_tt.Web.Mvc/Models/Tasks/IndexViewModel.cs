using proj_tt.Tasks.Dto;
using System.Collections.Generic;
using Abp.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using proj_tt.Tasks;

namespace proj_tt.Web.Models.Tasks
{
    public class IndexViewModel
    {
        public IReadOnlyList<TaskListDto> Tasks { get; }

        public IndexViewModel(IReadOnlyList<TaskListDto> tasks)
        {
            Tasks = tasks;
        }

        public string GetTaskLabel(TaskListDto task)
        {
            switch (task.State)
            {
                case TaskState.Open:
                    return "bg-success";
                default:
                    return "bg-gradient-gray";
            }
        }

        public TaskState? SelectedTaskState { get; set; }

        public List<SelectListItem> GetTasksStateSelectListItems(ILocalizationManager localizationManager)
        {
            var list = new List<SelectListItem>
            
        {
            new SelectListItem
            {
                Text = localizationManager.GetString(proj_ttConsts.LocalizationSourceName, "AllTasks"),
                Value = "",
                Selected = SelectedTaskState == null
            }
        };

            list.AddRange(Enum.GetValues(typeof(TaskState))
                    .Cast<TaskState>()
                    .Select(state =>
                        new SelectListItem
                        {
                            Text = localizationManager.GetString(proj_ttConsts.LocalizationSourceName, $"{state}"),
                            Value = state.ToString(),
                            Selected = state == SelectedTaskState
                        })
            );

            return list;
        }
    }
}
