using Domain.Entities;
using Domain.Helpers;
using Domain.QueryStrings;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

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
            var allCategories = await _dbContext.Categories
                .Where(a => a.Name.StartsWith(parameters.FirstLetter) && a.Name.Contains(parameters.SearchText))
                .ToListAsync();
            var allCategoriesQuarriable = allCategories.AsQueryable();
            var categories = PaginatedList<Category>.ToPagedList(allCategoriesQuarriable, parameters.PageNumber, parameters.PageSize);
            return categories;
        }

        public async Task<Category?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Categories.FindAsync(id);
        }

        public async Task AddAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingCategory = await _dbContext.Categories.FindAsync(id);
            if (existingCategory is null)
            {
                return false;
            }
            _dbContext.Categories.Remove(existingCategory);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
