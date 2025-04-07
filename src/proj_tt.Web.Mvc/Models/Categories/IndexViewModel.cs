using proj_tt.Categories.Dto;
using System.Collections.Generic;

namespace proj_tt.Web.Models.Categories
{
    public class IndexViewModel
    {
        public CategoriesDto Category { get; set; }
        public IReadOnlyList<CategoriesDto> Categories { get; }

        public IndexViewModel(IReadOnlyList<CategoriesDto> categories)
        {
            Categories = categories;
        }

        public IndexViewModel()
        {
        }
    }
}
