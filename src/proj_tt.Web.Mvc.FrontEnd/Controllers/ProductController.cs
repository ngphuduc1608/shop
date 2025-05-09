using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using proj_tt.Categories;
using proj_tt.Categories.Dto;
using proj_tt.Controllers;
using proj_tt.Products;
using proj_tt.Products.Dto;
using proj_tt.Web.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace proj_tt.Web.Controllers
{
    public class ProductController : proj_ttControllerBase
    {

        private readonly IProductAppService _productAppService;
        private readonly ICategoriesAppService _categoryAppService;
        //private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductAppService productAppService, ICategoriesAppService categoryAppService)
        {
            _productAppService = productAppService;
            _categoryAppService = categoryAppService;
            //_webHostEnvironment = webHostEnvironment;
        }

        public async Task<ActionResult> Index(int page = 1, int pageSize = 15, string sort = "CreationTime desc", decimal? minPrice = null, decimal? maxPrice = null, List<int> categoryIds = null, string keyword = null)
        {

            // Create filter DTO
            var pagedProductDto = new PagedProductDto
            {
                SkipCount = (page - 1) * pageSize,
                MaxResultCount = pageSize,
                Sorting = sort,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                CategoryIds = categoryIds,
                Keyword = keyword
            };
            ViewBag.Keyword = keyword;
            // Get products
            var products = await _productAppService.GetProductPaged(pagedProductDto);
            var result = products.Items.ToList();

            // Get categories for filter
            var categories = await _categoryAppService.GetAllCategories(new PagedCategoriesDto());
            var categoriesItems = categories.Items.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.NameCategory
            }).ToList();

            // Calculate pagination info
            var totalItems = products.TotalCount;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var model = new IndexViewModel(result, categoriesItems)
            {
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentSort = sort,
                CurrentMinPrice = minPrice,
                CurrentMaxPrice = maxPrice,
                SelectedCategoryIds = categoryIds,
                Keyword = keyword
            };

            return View(model);
        }



        public async Task<IActionResult> Details(int productId)
        {
            var product = await _productAppService.GetProducts(productId);
            var model = new IndexViewModel
            {
                Product = product
            };
            return View(model);
        }




    }
}
