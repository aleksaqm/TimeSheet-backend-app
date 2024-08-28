using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Service.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UpdateCategoryDto>> GetAllAsync()
        {
            var categories = await _repository.GetAllAsync();
            return _mapper.Map<List<UpdateCategoryDto>>(categories);
        }

        public async Task<UpdateCategoryDto?> GetByIdAsync(Guid id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return null;
            return _mapper.Map<UpdateCategoryDto>(category);
        }

        public async Task<UpdateCategoryDto?> AddAsync(CreateCategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            try
            {
                await _repository.AddAsync(category);
                return _mapper.Map<UpdateCategoryDto>(category);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<UpdateCategoryDto?> UpdateAsync(UpdateCategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            var result = await _repository.UpdateAsync(category);
            if (result == null)
                return null;
            return _mapper.Map<UpdateCategoryDto>(result);
        }

        //public IEnumerable<UpdateCategoryDto> GetAll()
        //{
        //    var categories = _repository.GetAll();
        //    var result = _mapper.Map<List<UpdateCategoryDto>>(categories);
        //    return result;
        //}

        //public UpdateCategoryDto? GetById(Guid id)
        //{
        //    var category = _repository.GetById(id);
        //    if (category == null) 
        //        return null;
        //    return _mapper.Map<UpdateCategoryDto>(category);
        //}

        //public UpdateCategoryDto? Add(CreateCategoryDto categoryDto)
        //{
        //    var category = _mapper.Map<Category>(categoryDto);
        //    try
        //    {
        //        _repository.Add(category);
        //        return _mapper.Map<UpdateCategoryDto>(category);
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //public UpdateCategoryDto? Update(UpdateCategoryDto categoryDto)
        //{
        //    var category = _mapper.Map<Category>(categoryDto);
        //    var result = _repository.Update(category);
        //    if (result == null)
        //        return null;
        //    return _mapper.Map<UpdateCategoryDto>(result);
        //}
    }
}
