using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace proj_tt.Categories.Dto
{
    [AutoMap(typeof(Category))]
    public class CategoriesDto : AuditedEntityDto
    {
        [Required]
        public string NameCategory { get; set; }
    }
}
