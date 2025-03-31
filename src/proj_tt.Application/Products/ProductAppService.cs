using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using proj_tt.Products.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace proj_tt.Products
{
    public class ProductAppService : proj_ttAppServiceBase, IProductAppService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRepository<Product> _productRepository;

        public ProductAppService(IRepository<Product> productRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;

        }

        public async System.Threading.Tasks.Task Create(ProductListDto input)
        {
            //var product = ObjectMapper.Map<Product>(input);
            //await _productRepository.InsertAsync(product);


            // Xử lý upload ảnh nếu có
            string imagePath = null;
            if (input.ImageUrl != null && input.ImageUrl.Length > 0)
            {
           
                imagePath = await SaveImageAsync(input.ImageUrl);

            }

            // Map DTO sang entity và gán ImagePath
            var product = ObjectMapper.Map<Product>(input);
            product.ImageUrl = imagePath;

            // Thêm sản phẩm vào database
            await _productRepository.InsertAsync(product);
        }

        
        public async Task<ListResultDto<ProductDto>> GetListProduct()
        {
            var products = await _productRepository.GetAll().OrderByDescending(t=>t.CreationTime).ToListAsync();

            return new ListResultDto<ProductDto>(ObjectMapper.Map<List<ProductDto>>(products));
        }

        public async Task Update(UpdateProductDto input)
        {
            var product = await _productRepository.FirstOrDefaultAsync((int)input.Id);

            if (product == null)
            {
                throw new UserFriendlyException("Sản phẩm không tồn tại!");
            }

            // Cập nhật thông tin
            product.Name = input.Name;
            product.Price = input.Price;
            product.Discount = input.Discount;
            Console.WriteLine("haha: ");
            if (input.ImageUrl != null)
            {
                product.ImageUrl = await SaveImageAsync(input.ImageUrl);
            }
            else
            {
                product.ImageUrl = input.ExistingImageUrl; // Giữ ảnh cũ nếu không có ảnh mới
            }

            ObjectMapper.Map<Product>(product);
            await _productRepository.UpdateAsync(product); // Lưu thay đổi

        }

        public async Task Delete(int id)
        {
            await _productRepository.DeleteAsync(id);
        }


        private async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }
            // Đường dẫn thư mục lưu ảnh: wwwroot/uploads/products
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/products");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            // Tạo tên file duy nhất
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            // Lưu file ảnh vào thư mục
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            // Lưu đường dẫn tương đối vào database
            return $"/uploads/products/{fileName}"; // Trả về đường dẫn để lưu vào database
        }

       
    }
}
