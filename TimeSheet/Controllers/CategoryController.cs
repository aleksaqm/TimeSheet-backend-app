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
        public IActionResult GetAll() { 
            return Ok(_categoryService.GetAll());
        }

        [HttpPost]
        public IActionResult AddCategory(CategoryDto categoryDto) { 
            var category = _categoryService.Insert(categoryDto);
            if (category == null)
                return BadRequest("doesnt work");
            return Ok(category);
        }
    }
}
