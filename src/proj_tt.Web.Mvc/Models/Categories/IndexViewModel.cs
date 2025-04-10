using proj_tt.Categories.Dto;
using System.Collections.Generic;

namespace proj_tt.Web.Models.Categories
{
    public class IndexViewModel
    {
        public CategoriesDto Category { get; set; } // gọi lên dùng để chỉnh sửa tt category
        public IReadOnlyList<CategoriesDto> Categories { get; } // IReadOnlyList là chỉ đọc , chứ không thay đổi dữ liệu 



        // constructor có tham số để khi dùng action Index()
        public IndexViewModel(IReadOnlyList<CategoriesDto> categories)
        {
            Categories = categories;
        }

        // constructor rỗng dùng khi set thủ công , khi dùng EditModal

        public IndexViewModel()
        {
        }
    }
}
