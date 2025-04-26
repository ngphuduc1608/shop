using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;

namespace proj_tt.Products.Dto
{
    public class PagedProductDto : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        //public PagedProductDto()
        //{
        //    MaxResultCount = 15;
        //}

        public string Keyword { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<string> CategoryIds { get; set; }


        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting) || Sorting == "0 asc")
            {
                Sorting = "CreationTime desc";
            }
        }


    }
}
