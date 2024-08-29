using Domain.Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface ICategoryService
    {
        Task<CategoryUpdateDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<CategoryUpdateDto>> GetAllAsync();
        Task<CategoryUpdateDto?> AddAsync(CategoryCreateDto categoryDto);
        Task<CategoryUpdateDto?> UpdateAsync(CategoryUpdateDto categoryDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
