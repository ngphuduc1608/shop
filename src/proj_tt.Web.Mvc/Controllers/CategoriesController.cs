using Abp.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using proj_tt.Categories;
using proj_tt.Categories.Dto;
using proj_tt.Web.Models.Categories;
using System.Threading.Tasks;

namespace proj_tt.Web.Controllers
{
    public class CategoriesController : AbpController
    {
        private readonly ICategoriesAppService _categoriesAppService;

        public CategoriesController(ICategoriesAppService categoriesAppService)
        {
            _categoriesAppService = categoriesAppService;
        }

        public async Task<ActionResult> Index()
        {
            var categories = await _categoriesAppService.GetAllCategories(new PagedCategoriesDto());  // gọi all categories 

            var viewModel = new IndexViewModel(categories.Items); // truyền list vào model
            return View(viewModel);

        }

        public async Task<ActionResult> EditModal(int categoryId)
        {
            var category = await _categoriesAppService.GetCategories(categoryId);

            var model = new IndexViewModel
            {
                Category = category
            };

            return PartialView("_EditModal", model); // nó sẽ tìm đến _EditModal.cshtml và truyền model
        }



    }
}


// View() là sẽ trả về 1 trang Html đầy đủ, nó sẽ tìm trong folder Views
// PartialView() là sẽ trả về 1 phần của Html ,(Modal, Popup) 
// Ok() là trả về Json , dùng khi chỉ muốn lấy dữ liệu và hiển thị thông báo 