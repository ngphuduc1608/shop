using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj_tt.Categories.Dto
{
    [AutoMapFrom(typeof(Category))]
    public class CategoriesDto:AuditedEntityDto
    {
        [Required]
        public string NameCategory {  get; set; }
    }
}
