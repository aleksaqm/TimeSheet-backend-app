﻿using Domain.Entities;
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
        Task<UpdateCategoryDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<UpdateCategoryDto>> GetAllAsync();
        Task<UpdateCategoryDto?> AddAsync(CreateCategoryDto categoryDto);
        Task<UpdateCategoryDto?> UpdateAsync(UpdateCategoryDto categoryDto);
        //UpdateCategoryDto? GetById(Guid id);
        //IEnumerable<UpdateCategoryDto> GetAll();
        //UpdateCategoryDto? Add(CreateCategoryDto categoryDto);
        //UpdateCategoryDto? Update(UpdateCategoryDto categoryDto);

    }
}
