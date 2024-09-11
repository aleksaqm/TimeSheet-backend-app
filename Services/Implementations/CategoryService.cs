using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Helpers;
using Domain.QueryStrings;
using Domain.Repositories;
using Infrastructure.UnitOfWork;
using Service.Abstractions;
using Services.Converters;
using Shared;

namespace Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<CategoryResponse>> GetAllAsync(QueryStringParameters parameters)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllAsync(parameters);
            var mapped = _mapper.Map<PaginatedList<CategoryResponse>>(categories);
            PaginatedListConverter<Category, CategoryResponse>.Convert(categories, mapped);
            return mapped;
        }

        public async Task<CategoryResponse?> GetByIdAsync(Guid id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category is null)
            {
                throw new CategoryNotFoundException("Category with given ID doesnt exist");
            }
            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<CategoryResponse?> AddAsync(CategoryCreateDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            await _unitOfWork.CategoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<CategoryResponse?> UpdateAsync(CategoryUpdateDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            var existingCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(category.Id);
            if (existingCategory is null)
            {
                throw new CategoryNotFoundException("Category with given ID doesnt exist");
            }
            existingCategory.Name = category.Name;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CategoryResponse>(existingCategory);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            bool success = await _unitOfWork.CategoryRepository.DeleteAsync(id);
            if (success)
            {
                 await _unitOfWork.SaveChangesAsync();
                return true;
            }
            throw new CategoryNotFoundException("Category with given ID doesnt exist");
        }
    }
}
