using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace proj_tt.Banners
{
    [Table("AppBanners")]
    public class Banner : AuditedEntity<Guid>
    {
        public Banner(string title, string imageUrl, string link, bool isActive)
        {
            Title = title;
            ImageUrl = imageUrl;
            Link = link;
            IsActive = isActive;
        }

        public string Title { get; set; }
        public string ImageUrl { get; set; }  // Đường dẫn ảnh (có thể là HTTP)
        public string Link { get; set; }      // Đường dẫn khi người dùng click vào banner (có thể là HTTP)
        public bool IsActive { get; set; }



    }
}
