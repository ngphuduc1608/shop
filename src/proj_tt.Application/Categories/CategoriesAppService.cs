
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using proj_tt.Categories.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;


namespace proj_tt.Categories
{
    public class CategoriesAppService : proj_ttAppServiceBase, ICategoriesAppService
    {

        private readonly IRepository<Category> _categoryRepository;

        public CategoriesAppService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task Create(CreateCategoriesDto input)
        {
            var category= ObjectMapper.Map<Category>(input);
            await _categoryRepository.InsertAsync(category);
        }

        public async Task Delete(int id)
        {
           await _categoryRepository.DeleteAsync(id);
        }

        public async Task<PagedResultDto<CategoriesDto>> GetAllCategories(PagedCategoriesDto input)
        {
            var categories= _categoryRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                categories = categories.Where(p => p.NameCategory.Contains(input.Keyword));
            }

            var count = await categories.CountAsync();

            input.Sorting = "CreationTime DESC";

            var items = await categories.PageBy(input.SkipCount,input.MaxResultCount).OrderBy(input.Sorting).ToListAsync();

            return new PagedResultDto<CategoriesDto> { TotalCount = count, Items = ObjectMapper.Map<List<CategoriesDto>>(items) };
        }

        public async Task<CategoriesDto> GetCategories(int id )
        {
            var category=await _categoryRepository.GetAsync(id);
            return ObjectMapper.Map<CategoriesDto>(category);
        }

        public async Task Update(CreateCategoriesDto input)
        {
            var category = await _categoryRepository.GetAsync(input.Id);
            ObjectMapper.Map(input, category);
            await _categoryRepository.UpdateAsync(category);
        }
    }
}
