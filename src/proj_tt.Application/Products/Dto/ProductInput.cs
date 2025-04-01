using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj_tt.Products.Dto
{
    public class ProductInput : PagedAndSortedResultRequestDto
    {
        ///public int orderindex , paging , sorting....
        public string SeachTerm { get; set; }

        //public void No
    }
}
