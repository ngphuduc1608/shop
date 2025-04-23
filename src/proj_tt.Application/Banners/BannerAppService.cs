using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using proj_tt.Banners.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace proj_tt.Banners
{
    public class BannerAppService : proj_ttAppServiceBase, IBannerAppService
    {
        private readonly IRepository<Banner, Guid> _bannerRepository;

        public BannerAppService(IRepository<Banner, Guid> bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<List<BannerDto>> GetListBanner()
        {
            // Lấy tất cả các banner và lọc theo IsActive
            var banners = await _bannerRepository
                .GetAll()  // Lấy tất cả banner
                .ToListAsync();  // Chuyển kết quả thành danh sách bất đồng bộ

            // Ánh xạ từ entity Banner sang BannerDto
            return banners.Select(b => new BannerDto
            {
                Id = b.Id,
                Title = b.Title,
                ImageUrl = b.ImageUrl,
                Link = b.Link,
                IsActive = b.IsActive
            }).ToList();

        }
    }
}
