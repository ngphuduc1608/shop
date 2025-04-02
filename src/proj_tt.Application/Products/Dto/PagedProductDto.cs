using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj_tt.Products.Dto
{
    public class PagedProductDto:PagedAndSortedResultRequestDto
    {
        

        public string Keyword { get; set; }

        //public PagedProductDto()
        //{
        //    MaxResultCount = 15;
        //}
    }
}
