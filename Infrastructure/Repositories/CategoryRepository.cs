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
        public Task<Category> GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
