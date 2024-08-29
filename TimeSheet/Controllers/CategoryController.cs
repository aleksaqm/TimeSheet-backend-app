using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared;

namespace TimeSheet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService service)
        {
            _categoryService = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryUpdateDto>>> GetAll() { 
            var results = await _categoryService.GetAllAsync();
            return Ok(results);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<CategoryUpdateDto>> GetById(Guid id) {
            var result = await _categoryService.GetByIdAsync(id);
            if (result == null)
                return BadRequest("Category with given ID doesn't exist");
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryUpdateDto>> Add(CategoryCreateDto categoryDto) { 
            var category = await _categoryService.AddAsync(categoryDto);
            return category == null ? BadRequest() : Ok(category);
        }

        [HttpPut]
        public async Task<ActionResult<CategoryUpdateDto>> Update(CategoryUpdateDto categoryDto) { 
            var category = await _categoryService.UpdateAsync(categoryDto);
            return category==null ? BadRequest("Category with given ID doesn't exist") : Ok(category);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id) { 
            bool success = await _categoryService.DeleteAsync(id);
            if (success)
                return Ok();
            return BadRequest("Category with given ID doesn't exist");
        }

    }
}
