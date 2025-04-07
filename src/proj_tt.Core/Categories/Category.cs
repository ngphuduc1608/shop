using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj_tt.Categories
{
    [Table("AppCategories")]
    public class Category :AuditedEntity
    {
        public const int MaxNameLength = 32;

        //public Category()
        //{
        //    CreationTime=Clo
        //}

        [Required]
        [StringLength(MaxNameLength)]
        public string NameCategory { get; set; }
    }
}
