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
        public ActionResult<List<UpdateCategoryDto>> GetAll() { 
            return Ok(_categoryService.GetAllAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public ActionResult<UpdateCategoryDto> GetById(Guid id) {
            var result = _categoryService.GetByIdAsync(id);
            if (result == null)
                return BadRequest("Category with given ID doesn't exist");
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<UpdateCategoryDto> Add(CreateCategoryDto categoryDto) { 
            var category = _categoryService.AddAsync(categoryDto);
            return category == null ? BadRequest() : Ok(category);
        }

        [HttpPut]
        public ActionResult<UpdateCategoryDto> Update(UpdateCategoryDto categoryDto) { 
            var category = _categoryService.UpdateAsync(categoryDto);
            return category==null ? BadRequest("Category with given ID doesn't exist") : Ok(category);
        }

    }
}
