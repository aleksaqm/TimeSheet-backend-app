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
        public async Task<ActionResult<List<UpdateCategoryDto>>> GetAll() { 
            var results = await _categoryService.GetAllAsync();
            return Ok(results);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<UpdateCategoryDto>> GetById(Guid id) {
            var result = await _categoryService.GetByIdAsync(id);
            if (result == null)
                return BadRequest("Category with given ID doesn't exist");
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<UpdateCategoryDto>> Add(CreateCategoryDto categoryDto) { 
            var category = await _categoryService.AddAsync(categoryDto);
            return category == null ? BadRequest() : Ok(category);
        }

        [HttpPut]
        public async Task<ActionResult<UpdateCategoryDto>> Update(UpdateCategoryDto categoryDto) { 
            var category = await _categoryService.UpdateAsync(categoryDto);
            return category==null ? BadRequest("Category with given ID doesn't exist") : Ok(category);
        }

    }
}
