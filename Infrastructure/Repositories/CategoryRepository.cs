using Domain.Entities;
using Domain.Helpers;
using Domain.QueryStrings;
using Domain.Repositories;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public CategoryRepository(RepositoryDbContext context)
        {
            _dbContext = context;
        }

        public async Task<PaginatedList<Category>> GetAllAsync(QueryStringParameters parameters)
        {
            var query = _dbContext.Categories.AsQueryable();
            if (parameters.SearchText is not null)
            {
                query = query.Where(c => c.Name.Contains(parameters.SearchText));
            }
            if (parameters.FirstLetter is not null)
            {
                query = query.Where(c => c.Name.StartsWith(parameters.FirstLetter));
            }
            var categories = PaginatedList<Category>.ToPagedList(query, parameters.PageNumber, parameters.PageSize);
            return categories;
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Categories.FindAsync(id);
        }

        public async Task AddAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingCategory = await _dbContext.Categories.FindAsync(id);
            if (existingCategory is null)
            {
                return false;
            }
            _dbContext.Categories.Remove(existingCategory);
            return true;
        }
    }
}
