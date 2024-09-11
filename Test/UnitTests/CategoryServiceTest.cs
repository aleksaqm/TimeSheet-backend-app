using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.UnitOfWork;
using Microsoft.Identity.Client;
using Moq;
using Service.Abstractions;
using Services.Implementations;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Test.UnitTests
{
    public class CategoryServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ICategoryService _categoryService;

        public CategoryServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _categoryService = new CategoryService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_CategoryExists_ReturnsCategoryObject()
        {
            var categoryId = Guid.NewGuid();
            var category = new Category { Id = categoryId, Name = "Aleksa" };
            var categoryResponse = new CategoryResponse { Id = categoryId, Name =  "Aleksa" };

            _unitOfWorkMock.Setup(u => u.CategoryRepository.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mapperMock.Setup(m => m.Map<CategoryResponse>(category)).Returns(categoryResponse);

            var result = await _categoryService.GetByIdAsync(categoryId);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdAsync_CategoryNotFound_ThrowsNotFoundException()
        {
            var categoryId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.CategoryRepository.GetByIdAsync(categoryId)).ReturnsAsync(null as Category);

            await Assert.ThrowsAsync<CategoryNotFoundException>(() => _categoryService.GetByIdAsync(categoryId));
        }

        [Fact]
        public async Task AddAsync_ValidCategory_Success()
        {
            var categoryDto = new CategoryCreateDto { Name = "Aleksa" };
            var categoryId = Guid.NewGuid();
            var category = new Category { Id = categoryId, Name = "Aleksa" };
            var categoryResponse = new CategoryResponse { Id = categoryId, Name = "Aleksa" };

            _mapperMock.Setup(m => m.Map<Category>(categoryDto)).Returns(category);
            _unitOfWorkMock.Setup(u => u.CategoryRepository.AddAsync(category)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<CategoryResponse>(category)).Returns(categoryResponse);

            var result = await _categoryService.AddAsync(categoryDto);

            Assert.NotNull(result);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ValidData_Success()
        {
            var categoryId = Guid.NewGuid();
            var categoryDto = new CategoryUpdateDto { Id = categoryId ,Name = "Bogdan" };
            var category = new Category { Id = categoryId, Name = "Bogdan" };
            var existingCategory = new Category { Id = categoryId, Name = "Aleksa" };
            var categoryResponse = new CategoryResponse { Id = categoryId, Name = "Bogdan" };

            _mapperMock.Setup(m => m.Map<Category>(categoryDto)).Returns(category);
            _unitOfWorkMock.Setup(u => u.CategoryRepository.GetByIdAsync(category.Id)).ReturnsAsync(existingCategory);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<CategoryResponse>(existingCategory)).Returns(categoryResponse);

            var result = await _categoryService.UpdateAsync(categoryDto);

            Assert.NotNull(result);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_InvalidData_ThrowsNotFoundException()
        {
            var categoryId = Guid.NewGuid();
            var categoryDto = new CategoryUpdateDto { Id = categoryId, Name = "Bogdan" };
            var category = new Category { Id = categoryId, Name = "Bogdan" };

            _mapperMock.Setup(m => m.Map<Category>(categoryDto)).Returns(category);
            _unitOfWorkMock.Setup(u => u.CategoryRepository.GetByIdAsync(category.Id)).ReturnsAsync(null as Category);

            await Assert.ThrowsAsync<CategoryNotFoundException>(() => _categoryService.UpdateAsync(categoryDto));
        }

        [Fact]
        public async Task DeleteAsync_ValidId_Success()
        {
            var categoryId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.CategoryRepository.DeleteAsync(categoryId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _categoryService.DeleteAsync(categoryId);

            Assert.True(result);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_InvalidId_ThrowsNotFoundException()
        {
            var categoryId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.CategoryRepository.DeleteAsync(categoryId)).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<CategoryNotFoundException>(() => _categoryService.DeleteAsync(categoryId));
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);

        }


    }
}
