using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using proj_tt.Categories;
using proj_tt.Categories.Dto;
using proj_tt.Controllers;
using proj_tt.Products;
using proj_tt.Products.Dto;
using proj_tt.Web.Models.Products;
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

        public async Task<ActionResult> Index()
        {
            var products = await _productAppService.GetProductPaged(new PagedProductDto());

            var categories = await _categoryAppService.GetAllCategories(new PagedCategoriesDto());
            var categoriesItems = categories.Items.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.NameCategory
            }).ToList();
            var model = new IndexViewModel(products.Items, categoriesItems);
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
