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
        Task<CategoryDto> GetById(Guid id);
        IEnumerable<CategoryDto> GetAll();
        Category Insert(CategoryDto categoryDto);
    }
}
