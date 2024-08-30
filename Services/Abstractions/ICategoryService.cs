using Domain.Helpers;
using Domain.QueryStrings;
using Shared;

namespace Service.Abstractions
{
    public interface ICategoryService
    {
        Task<CategoryUpdateDto?> GetByIdAsync(Guid id);
        Task<PaginatedList<CategoryUpdateDto>> GetAllAsync(QueryStringParameters parameters);
        Task<CategoryUpdateDto?> AddAsync(CategoryCreateDto categoryDto);
        Task<CategoryUpdateDto?> UpdateAsync(CategoryUpdateDto categoryDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
