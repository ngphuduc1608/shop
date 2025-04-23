using Abp.Application.Services.Dto;
using System;

namespace proj_tt.Banners.Dto
{
    public class BannerDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; }  // Đường dẫn ảnh (có thể là HTTP)
        public string Link { get; set; }      // Đường dẫn khi người dùng click vào banner (có thể là HTTP)
        public bool IsActive { get; set; }
    }
}
