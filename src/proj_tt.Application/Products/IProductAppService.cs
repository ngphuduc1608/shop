using Abp.Application.Services;
using Abp.Application.Services.Dto;
using proj_tt.Products.Dto;
using System.Threading.Tasks;

namespace proj_tt.Products
{
    public interface IProductAppService : IApplicationService
    {

        Task<ProductDto> GetProducts(int id);

        Task<PagedResultDto<ProductDto>> GetProductPaged(PagedProductDto input);

        System.Threading.Tasks.Task Create(ProductListDto input);

        System.Threading.Tasks.Task Update(UpdateProductDto input);

        System.Threading.Tasks.Task Delete(int id);

    }
}
