using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj_tt.Categories.Dto
{
    [AutoMapTo(typeof(Category))]

    public class CreateCategoriesDto
    {
        public int Id { get; set; }

        [Required]

        public string NameCategory { get; set; }

    }
}
