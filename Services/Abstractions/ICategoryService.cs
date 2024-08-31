using Domain.Helpers;
using Domain.QueryStrings;
using Shared;

namespace Service.Abstractions
{
    public interface ICategoryService
    {
        Task<CategoryResponse?> GetByIdAsync(Guid id);
        Task<PaginatedList<CategoryResponse>> GetAllAsync(QueryStringParameters parameters);
        Task<CategoryResponse?> AddAsync(CategoryCreateDto categoryDto);
        Task<CategoryResponse?> UpdateAsync(CategoryUpdateDto categoryDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
