using Domain.QueryStrings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<ActionResult<List<CategoryUpdateDto>>> GetAll([FromQuery] QueryStringParameters parameters) { 
            var results = await _categoryService.GetAllAsync(parameters);

            var metadata = new
            {
                results.TotalCount,
                results.PageSize,
                results.CurrentPage,
                results.HasNext,
                results.HasPrevious
            };

            Response.Headers.Append("Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(results);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<CategoryUpdateDto>> GetById(Guid id) {
            var result = await _categoryService.GetByIdAsync(id);
            if (result is null)
            {
                return BadRequest("Category with given ID doesn't exist");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryUpdateDto>> Add(CategoryCreateDto categoryDto) { 
            var category = await _categoryService.AddAsync(categoryDto);
            return category is null ? BadRequest() : Ok(category);
        }

        [HttpPut]
        public async Task<ActionResult<CategoryUpdateDto>> Update(CategoryUpdateDto categoryDto) { 
            var category = await _categoryService.UpdateAsync(categoryDto);
            return category is null ? BadRequest("Category with given ID doesn't exist") : Ok(category);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id) { 
            bool success = await _categoryService.DeleteAsync(id);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Category with given ID doesn't exist");
        }

    }
}
