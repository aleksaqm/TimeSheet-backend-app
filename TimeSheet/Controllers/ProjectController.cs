using Domain.QueryStrings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Abstractions;
using Shared;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<ProjectResponse>>> GetAll([FromQuery] QueryStringParameters parameters)
        {
            var results = await _projectService.GetAllAsync(parameters);
            var metadata = new
            {
                results.PageSize,
                results.CurrentPage,
                results.HasNext,
                results.HasPrevious
            };

            Response.Headers.Append("Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(results);
        }

        [Authorize]
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<ProjectResponse>> GetById(Guid id)
        {
            var result = await _projectService.GetByIdAsync(id);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ProjectResponse>> Add(ProjectCreateDto projectDto)
        {
            var project = await _projectService.AddAsync(projectDto);
            return Ok(project);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<ProjectResponse>> Update(ProjectUpdateDto projectDto)
        {
            var project = await _projectService.UpdateAsync(projectDto);
            return Ok(project);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool success = await _projectService.DeleteAsync(id);
            return Ok();
        }



    }
}
