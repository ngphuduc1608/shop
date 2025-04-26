using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proj_tt.Categories
{
    [Table("AppCategories")]
    public class Category : AuditedEntity
    {
        public const int MaxNameLength = 32;

        [Required]
        [StringLength(MaxNameLength)]
        public string NameCategory { get; set; }
    }
}
