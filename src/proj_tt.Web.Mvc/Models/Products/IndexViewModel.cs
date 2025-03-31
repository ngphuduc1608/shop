using proj_tt.Products.Dto;
using proj_tt.Tasks.Dto;
using System.Collections.Generic;

namespace proj_tt.Web.Models.Products
{
    public class IndexViewModel
    {
        public IReadOnlyList<ProductDto> Products { get; }

        public IndexViewModel(IReadOnlyList<ProductDto> products)
        {
            Products = products;
        }
    }
}
