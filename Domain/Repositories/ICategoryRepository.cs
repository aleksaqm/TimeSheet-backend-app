using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid id);
        Task<IEnumerable<Category>> GetAllAsync();
        Task AddAsync(Category category);
        Task<Category?> UpdateAsync(Category category);
        //Category? GetById(Guid id);
        //IEnumerable<Category> GetAll();
        //void Add(Category category);
        //public Category? Update(Category category);
    }
}
