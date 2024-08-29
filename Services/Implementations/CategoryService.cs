using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Service.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<CategoryUpdateDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            return _mapper.Map<List<CategoryUpdateDto>>(categories);
        }

        public async Task<CategoryUpdateDto?> GetByIdAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return null;
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
