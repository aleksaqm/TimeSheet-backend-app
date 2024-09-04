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
        public async Task<ActionResult<List<CategoryResponse>>> GetAll([FromQuery] QueryStringParameters parameters) { 
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
        public async Task<ActionResult<CategoryResponse>> GetById(Guid id) {
            var result = await _categoryService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponse>> Add(CategoryCreateDto categoryDto) { 
            var category = await _categoryService.AddAsync(categoryDto);
            return Ok(category);
        }

        [HttpPut]
        public async Task<ActionResult<CategoryResponse>> Update(CategoryUpdateDto categoryDto) { 
            var category = await _categoryService.UpdateAsync(categoryDto);
            return Ok(category);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id) { 
            var success = await _categoryService.DeleteAsync(id);
            return Ok();
        }

    }
}
