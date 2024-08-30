using AutoMapper;
using Domain.Entities;
using Domain.Helpers;
using Domain.QueryStrings;
using Domain.Repositories;
using Service.Abstractions;
using Shared;

namespace Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<CategoryUpdateDto>> GetAllAsync(QueryStringParameters parameters)
        {
            var categories = await _repository.GetAllAsync(parameters);
            var mapped = _mapper.Map<PaginatedList<CategoryUpdateDto>>(categories);
            mapped.CurrentPage = categories.CurrentPage;
            mapped.TotalCount = categories.TotalCount;
            mapped.PageSize = categories.PageSize;
            mapped.TotalPages = categories.TotalPages;
            return mapped;
        }

        public async Task<CategoryUpdateDto?> GetByIdAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category is null)
            {
                return null;
            }
            return _mapper.Map<CategoryUpdateDto>(category);
        }

        public async Task<CategoryUpdateDto?> AddAsync(CategoryCreateDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            try
            {
                await _repository.AddAsync(category);
                return _mapper.Map<CategoryUpdateDto>(category);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<CategoryUpdateDto?> UpdateAsync(CategoryUpdateDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            var existingCategory = await _repository.GetByIdAsync(category.Id);
            if (existingCategory is null)
            {
                return null;
            }
            existingCategory.Name = category.Name;
            await _repository.UpdateAsync();
            return _mapper.Map<CategoryUpdateDto>(existingCategory);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
