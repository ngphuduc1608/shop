using proj_tt.Tasks;
using proj_tt.Tasks.Dto;
using System.Collections.Generic;

namespace proj_tt.Web.Models.Tasks
{
    public class ViewModel
    {
        public IReadOnlyList<TaskListDto> Tasks { get; }
        public ViewModel(IReadOnlyList<TaskListDto> tasks) {
            Tasks = tasks;
        }

        public string GetTaskLabel(TaskListDto task)
        {
            switch (task.State)
            {
                case TaskState.Open:
                    return "label-success";
                default:
                    return "label-default";
            }
        }
    }
}
