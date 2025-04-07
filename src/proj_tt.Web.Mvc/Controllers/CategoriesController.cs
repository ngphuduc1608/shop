using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using proj_tt.Categories;
using proj_tt.Categories.Dto;
using proj_tt.Controllers;
using proj_tt.Web.Models.Categories;
using System.Threading.Tasks;

namespace proj_tt.Web.Controllers
{
    public class CategoriesController: AbpController
    {
        private readonly ICategoriesAppService _categoriesAppService;

        public CategoriesController(ICategoriesAppService categoriesAppService)
        {
            _categoriesAppService = categoriesAppService;
        }

        //[HttpGet]
        public async Task<ActionResult> Index()
        {
            var products = await _categoriesAppService.GetAllCategories(new PagedCategoriesDto());

            var viewModel = new IndexViewModel(products.Items);
            return View(viewModel);

        }

        public async Task<ActionResult> EditModal(int categoryId)
        {
            var category = await _categoriesAppService.GetCategories(categoryId);

            var model = new IndexViewModel
            {
                Category = category
            };
            
            return PartialView("_EditModal", model);
        }



    }
}
