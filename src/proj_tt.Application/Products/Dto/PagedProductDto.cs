using Abp.Application.Services.Dto;

namespace proj_tt.Products.Dto
{
    public class PagedProductDto : PagedAndSortedResultRequestDto
    {


        public string Keyword { get; set; }

        //public PagedProductDto()
        //{
        //    MaxResultCount = 15;
        //}
    }
}
