using Microsoft.AspNetCore.Http;

namespace proj_tt.Banners.Dto
{
    public class CreateBannerInput
    {

        public string Name { get; set; }

        public IFormFile BannerFile { get; set; }
    }
}
