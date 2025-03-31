using Microsoft.AspNetCore.Mvc;
using proj_tt.Controllers;
using proj_tt.Products;
using proj_tt.Products.Dto;
using proj_tt.Web.Models.Products;
using System.Threading.Tasks;

namespace proj_tt.Web.Controllers
{
    public class ProductController : proj_ttControllerBase
    {

        private readonly IProductAppService _productAppService;

        public ProductController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        public async Task<ActionResult> Index()
        {
            var output = await _productAppService.GetListProduct();
            var model = new IndexViewModel(output.Items);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        public async Task<ActionResult> Create(ProductListDto input)
        {

            if (!ModelState.IsValid)
            {
                return View(input);
            }

            await _productAppService.Create(input);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductDto input)
        {
            await _productAppService.Update(input);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _productAppService.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
