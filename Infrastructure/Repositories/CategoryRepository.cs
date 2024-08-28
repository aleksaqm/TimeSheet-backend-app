using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public CategoryRepository(RepositoryDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var categories = await _dbContext.Categories.ToArrayAsync();
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

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory = await _dbContext.Categories.FindAsync(category.Id);
            if (existingCategory == null)
            {
                return null;
            }
            _dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
            await _dbContext.SaveChangesAsync();
            return existingCategory;
        }
        //public IEnumerable<Category> GetAll()
        //{
        //    var categories = _dbContext.Categories.ToArray();
        //    return categories;
        //}

        //public Category? GetById(Guid id)
        //{
        //    return _dbContext.Categories.Find(id);
        //}

        //public void Add(Category category)
        //{
        //    _dbContext.Categories.Add(category);
        //    _dbContext.SaveChanges();
        //}

        //public Category? Update(Category category) {
        //    var existingCategory = _dbContext.Categories.Find(category.Id);
        //    if (existingCategory == null)
        //    {
        //        return null;
        //    }
        //    _dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
        //    _dbContext.SaveChanges();
        //    return existingCategory;
        //}
    }
}
