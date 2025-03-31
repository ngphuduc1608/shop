using Abp.Application.Services;
using Abp.Application.Services.Dto;
using proj_tt.Products.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj_tt.Products
{
    public interface IProductAppService:IApplicationService
    {
        Task<ListResultDto<ProductDto>> GetListProduct();

        System.Threading.Tasks.Task Create(ProductListDto input);

        System.Threading.Tasks.Task Update(UpdateProductDto input);

        System.Threading.Tasks.Task Delete(int id);

    }
}
