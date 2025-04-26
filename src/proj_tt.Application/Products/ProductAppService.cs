using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using proj_tt.Products.Dto;
using System;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;

using System.Threading.Tasks;

namespace proj_tt.Products
{
    //[AbpAuthorize]
    public class ProductAppService : proj_ttAppServiceBase, IProductAppService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRepository<Product> _productRepository;
        private readonly string _imageRootPath;

        public ProductAppService(IRepository<Product> productRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
            _imageRootPath = Path.Combine("D:", "upload", "Product");
        }
        //[AbpAuthorize(PermissionNames.Pages_Products_Create)]
        public async System.Threading.Tasks.Task Create(ProductListDto input)
        {
            // Xử lý upload ảnh nếu có
            string imagePath = null;
            if (input.ImageUrl != null && input.ImageUrl.Length > 0)
            {

                imagePath = await SaveImageAsync(input.ImageUrl);

            }
            // Map DTO sang entity 
            var product = new Product(
                input.Name.Trim(),
                input.Price,
                imagePath,
                input.Discount,
                input.CategoryId,
                input.ProductionDate
            );

            // Thêm sản phẩm vào database
            await _productRepository.InsertAsync(product);
        }

        //[AbpAuthorize]
        //[AbpAuthorize(PermissionNames.Pages_Products)]

        // phan trang product
        public async Task<PagedResultDto<ProductDto>> GetProductPaged(PagedProductDto input)
        {
            //input.MaxResultCount = input.MaxResultCount > 0 ? input.MaxResultCount : 10;
            input.Normalize();
            var products = _productRepository.GetAllIncluding(p => p.Category);

            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                var keyword = input.Keyword.ToLower().Trim();
                products = products.Where(p =>
                    p.Name.ToLower().Contains(keyword) ||
                    p.Price.ToString().Contains(keyword) ||
                    p.Discount.ToString().Contains(keyword) ||
                    p.CreationTime.ToString().Contains(keyword) ||
                    p.LastModificationTime.ToString().Contains(keyword) ||
                    p.Category.NameCategory.ToLower().Contains(keyword)
                );
            }



            if (input.StartDate.HasValue)
            {
                products = products.Where(x => x.ProductionDate >= input.StartDate.Value);
            }
            if (input.EndDate.HasValue)
            {
                products = products.Where(x => x.ProductionDate <= input.EndDate.Value);
            }

            if (input.MinPrice.HasValue)
            {
                products = products.Where(x => x.Price >= input.MinPrice.Value);

            }
            if (input.MaxPrice.HasValue)
            {
                products = products.Where(x => x.Price <= input.MaxPrice.Value);
            }

            if (input.CategoryIds != null && input.CategoryIds.Any())
            {
                var selectedIds = input.CategoryIds.Select(id => Convert.ToInt32(id)).ToList();
                products = products.Where(x => x.CategoryId.HasValue && selectedIds.Contains(x.CategoryId.Value));
            }

            var count = await products.CountAsync();

            var items = await products.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            var result = items.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Discount = p.Discount,
                CategoryId = p.CategoryId ?? 0,
                NameCategory = p.Category != null ? p.Category.NameCategory : "",
                CreationTime = p.CreationTime,
                LastModificationTime = p.LastModificationTime,
                ProductionDate = p.ProductionDate,
            }).ToList();

            return new PagedResultDto<ProductDto>(count, result);
        }


        //[AbpAuthorize(PermissionNames.Pages_Products_Edit)]
        public async Task Update(UpdateProductDto input)
        {
            var product = await _productRepository.GetAsync(input.Id);

            product.Name = input.Name.Trim();
            product.Price = input.Price;
            product.Discount = input.Discount;
            product.CategoryId = input.CategoryId;
            product.ProductionDate = input.ProductionDate;


            if (input.ImageUrl != null)
            {
                product.ImageUrl = await SaveImageAsync(input.ImageUrl);
            }
            else
            {
                product.ImageUrl = input.ExistingImageUrl; // Giữ ảnh cũ nếu không có ảnh mới
            }

            await _productRepository.UpdateAsync(product);

        }
        //[AbpAuthorize(PermissionNames.Pages_Products_Delete)]
        public async Task Delete(int id)
        {

            await _productRepository.DeleteAsync(id);
        }

        //private async Task<string> SaveImageAsync(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return null;
        //    }
        //    // Đường dẫn thư mục lưu ảnh: wwwroot/uploads/products
        //    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");
        //    if (!Directory.Exists(uploadsFolder))
        //    {
        //        Directory.CreateDirectory(uploadsFolder);
        //    }
        //    // Tạo tên file duy nhất
        //    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //    var filePath = Path.Combine(uploadsFolder, fileName);
        //    // Lưu file ảnh vào thư mục
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }
        //    // Lưu đường dẫn tương đối vào database
        //    //return $"/images/products/{fileName}"; // Trả về đường dẫn để lưu vào database


        //    // Lấy base URL từ configuration
        //    var baseUrl = _configuration["App:BaseUrl"];
        //    if (string.IsNullOrEmpty((string)baseUrl))
        //    {
        //        throw new UserFriendlyException("BaseUrl is not configured in appsettings.json");
        //    }

        //    // Trả về đường dẫn tuyệt đối
        //    return $"{baseUrl}/images/products/{fileName}";

        //}

        //private async Task<string> SaveImageAsync(IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return null;
        //    }
        //    // Đường dẫn thư mục lưu ảnh: wwwroot/uploads/products
        //    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/products");
        //    if (!Directory.Exists(uploadsFolder))
        //    {
        //        Directory.CreateDirectory(uploadsFolder);
        //    }
        //    // Tạo tên file duy nhất
        //    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        //    var filePath = Path.Combine(uploadsFolder, fileName);
        //    // Lưu file ảnh vào thư mục
        //    using (var stream = new FileStream(filePath, FileMode.Create))
        //    {
        //        await file.CopyToAsync(stream);
        //    }
        //    // Lưu đường dẫn tương đối vào database
        //    return $"/uploads/products/{fileName}"; // Trả về đường dẫn để lưu vào database
        //}


        private async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var fileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
            var savePath = Path.Combine(_imageRootPath, fileName);

            // Tạo thư mục nếu chưa tồn tại
            Directory.CreateDirectory(_imageRootPath);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Trả về đường dẫn tương đối để truy cập từ trình duyệt
            return "/upload/Product/" + fileName;
        }

        //[AbpAuthorize]
        //[AbpAuthorize(PermissionNames.Pages_Products)]

        public async Task<ProductDto> GetProducts(int id)
        {

            var product = await _productRepository.GetAsync(id);
            return ObjectMapper.Map<ProductDto>(product);
        }
    }
}
