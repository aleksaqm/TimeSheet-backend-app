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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMemberController : ControllerBase
    {
        private readonly ITeamMemberService _teamMemberService;

        public TeamMemberController(ITeamMemberService service)
        {
            _teamMemberService = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<TeamMemberResponse>>> GetAll([FromQuery] QueryStringParameters parameters)
        {
            var results = await _teamMemberService.GetAllAsync(parameters);
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

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<TeamMemberResponse>> GetById(Guid id)
        {
            var result = await _teamMemberService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("Active")]
        public async Task<ActionResult<List<TeamMemberResponse>>> GetActive()
        {
            var results = await _teamMemberService.GetActive();
            return Ok(results);
        }

        [HttpPost]
        public async Task<ActionResult<TeamMemberResponse>> Add(TeamMemberCreateDto teamMemberDto)
        {
            var member = await _teamMemberService.AddAsync(teamMemberDto);
            return Ok(member);
        }

        [HttpPut]
        public async Task<ActionResult<TeamMemberResponse>> Update(TeamMemberUpdateDto teamMemberDto)
        {
            var member = await _teamMemberService.UpdateAsync(teamMemberDto);
            return Ok(member);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool success = await _teamMemberService.DeleteAsync(id);
            return Ok();
        }
    }
}
