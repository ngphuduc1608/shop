using Abp.Application.Services.Dto;

namespace proj_tt.Orders.Dto
{
    public class PagedOrderResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
} 