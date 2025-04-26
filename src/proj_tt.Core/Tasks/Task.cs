using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;
using proj_tt.Persons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj_tt.Tasks
{

    [Table("AppTasks")]
    public class Task :Entity, IHasCreationTime
    {
        public const int MaxTitleLength = 256;
        public const int MaxDescriptionLength = 64 * 1024;
        [Required]
        [StringLength(MaxTitleLength)]
        public string Title { get; set; }
        [StringLength(MaxDescriptionLength)]
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }

        public TaskState State { get; set; }

        public Task()
        {
            CreationTime = Clock.Now;
            State = TaskState.Open;
        }


        [ForeignKey(nameof(AssignedPersonId))]
        public Person AssignedPerson { get; set; }
        public Guid? AssignedPersonId
        { 
            get;  set;
        }


        public Task(string title, string description = null,Guid? assignedPersonId= null) : this()
        {
            Title = title;
            Description = description;
            AssignedPersonId = assignedPersonId;
            State = assignedPersonId == null? TaskState.Completed : TaskState.Open;
        }

       
    }


    public enum TaskState : byte
    {
        Open = 0,
        Completed = 1
    }
}
