using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;

namespace TimeSheet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService service)
        {
            _projectService = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectDto>>> GetAll()
        {
            var results = await _projectService.GetAllAsync();
            return Ok(results);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<ProjectDto>> GetById(Guid id)
        {
            var result = await _projectService.GetByIdAsync(id);
            if (result is null)
            {
                return BadRequest("Project with given ID doesn't exist");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectDto>> Add(ProjectCreateDto projectDto)
        {
            var project = await _projectService.AddAsync(projectDto);
            return project is null ? BadRequest() : Ok(project);
        }

        [HttpPut]
        public async Task<ActionResult<ProjectDto>> Update(ProjectUpdateDto projectDto)
        {
            var project = await _projectService.UpdateAsync(projectDto);
            return project is null ? BadRequest("Project with given ID doesn't exist") : Ok(project);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool success = await _projectService.DeleteAsync(id);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Project with given ID doesn't exist");
        }



    }
}
