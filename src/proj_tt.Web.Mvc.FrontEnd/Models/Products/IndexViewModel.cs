using Microsoft.AspNetCore.Mvc.Rendering;
using proj_tt.Products.Dto;
using System.Collections.Generic;

namespace proj_tt.Web.Models.Products
{
    public class IndexViewModel
    {
        public ProductDto Product { get; set; }
        public IReadOnlyList<ProductDto> Products { get; }
        public IReadOnlyList<SelectListItem> Categories { get; set; }

        // Pagination properties
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; }

        // Filter properties
        public string CurrentSort { get; set; }
        public decimal? CurrentMinPrice { get; set; }
        public decimal? CurrentMaxPrice { get; set; }
        public List<int> SelectedCategoryIds { get; set; }
        public string Keyword { get; set; }

        public IndexViewModel(IReadOnlyList<ProductDto> products, IReadOnlyList<SelectListItem> categories)
        {
            Products = products;
            Categories = categories;
        }

        public IndexViewModel()
        {
        }
    }
}
