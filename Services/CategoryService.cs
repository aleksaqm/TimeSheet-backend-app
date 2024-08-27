using Domain.Repositories;
using Service.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        public Task<CategoryDto> GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
