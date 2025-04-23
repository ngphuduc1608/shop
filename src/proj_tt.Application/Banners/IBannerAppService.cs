using Abp.Application.Services;
using proj_tt.Banners.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace proj_tt.Banners
{
    public interface IBannerAppService : IApplicationService
    {
        Task<List<BannerDto>> GetListBanner();

        //Task Create(CreateBannerInput input);
    }
}
