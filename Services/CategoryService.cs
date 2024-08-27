using Domain.Entities;
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

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<CategoryDto> GetAll()
        {
            var categories = _repository.GetAll();
            List<CategoryDto> result = new List<CategoryDto>();
            foreach (var category in categories) { 
                result.Add(new CategoryDto{
                    Id = category.Id,
                    Name = category.Name,
                });
            }
            return result;
        }

        public Task<CategoryDto> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Category Insert(CategoryDto categoryDto)
        {
            var category = new Category{
                Name = categoryDto.Name,
            };
            try
            {
                _repository.Insert(category);
                return category;
            } catch (Exception ex) {
                return null;
            }

        }
    }
}
