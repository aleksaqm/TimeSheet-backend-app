using Domain.Entities;
using Domain.Repositories;
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

        public IEnumerable<Category> GetAll()
        {
            var categories = _dbContext.Categories.ToArray();
            return categories;
        }

        public Task<Category> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Category category)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
        }
    }
}
