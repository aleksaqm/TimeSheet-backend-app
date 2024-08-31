using Domain.QueryStrings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Abstractions;
using Shared;
using System.ComponentModel.DataAnnotations;

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
        public async Task<ActionResult<List<ProjectResponse>>> GetAll([FromQuery] QueryStringParameters parameters)
        {
            var results = await _projectService.GetAllAsync(parameters);
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
        public async Task<ActionResult<ProjectResponse>> GetById(Guid id)
        {
            var result = await _projectService.GetByIdAsync(id);
            if (result is null)
            {
                return BadRequest("Project with given ID doesn't exist");
            }
            return Ok(result);
        }


        [HttpPost]
        public async Task<ActionResult<ProjectResponse>> Add(ProjectCreateDto projectDto)
        {
            var project = await _projectService.AddAsync(projectDto);
            return project is null ? BadRequest() : Ok(project);
        }

        [HttpPut]
        public async Task<ActionResult<ProjectResponse>> Update(ProjectUpdateDto projectDto)
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
