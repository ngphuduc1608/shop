using Microsoft.AspNetCore.Mvc;
using proj_tt.Addresses;
using proj_tt.Addresses.Dto;
using proj_tt.Controllers;
using System.Threading.Tasks;

namespace proj_tt.Web.Controllers
{
    public class AddressController : proj_ttControllerBase
    {
        private readonly IAddressAppService _addressAppService;

        public AddressController(IAddressAppService addressAppService)
        {
            _addressAppService = addressAppService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress(CreateAddressInput input)
        {
            try
            {
                if (input == null)
                {
                    return Json(new { success = false, error = new { message = "Dữ liệu không hợp lệ" } });
                }

                var address = await _addressAppService.CreateAddress(input);
                return Json(new { success = true, address = address });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, error = new { message = ex.Message } });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAddress(UpdateAddressInput input)
        {
            try
            {
                if (input == null || input.Id <= 0)
                {
                    return Json(new { success = false, error = new { message = "Dữ liệu không hợp lệ" } });
                }

                var address = await _addressAppService.UpdateAddress(input);
                return Json(new { success = true, address = address });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, error = new { message = ex.Message } });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAddress(int id)
        {
            try
            {
                var address = await _addressAppService.GetAddress(id);
                return Json(new { success = true, address = address });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, error = new { message = ex.Message } });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {
                await _addressAppService.DeleteAddress(id);
                return Json(new { success = true });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, error = new { message = ex.Message } });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetDefaultAddress(int id)
        {
            try
            {
                await _addressAppService.SetDefaultAddress(id);
                return Json(new { success = true });
            }
            catch (System.Exception ex)
            {
                return Json(new { success = false, error = new { message = ex.Message } });
            }
        }
    }
} 