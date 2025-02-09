using Domain.Entities;
using Domain.Helpers;
using Domain.QueryStrings;

namespace Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid id);
        Task<PaginatedList<Category>> GetAllAsync(QueryStringParameters parameters);
        Task AddAsync(Category category);
        Task<bool> DeleteAsync(Guid id);
    }
}
