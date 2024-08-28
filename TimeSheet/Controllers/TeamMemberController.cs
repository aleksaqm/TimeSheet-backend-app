using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;

namespace TimeSheet.Controllers
{
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
        public async Task<ActionResult<List<TeamMemberDto>>> GetAll()
        {
            var results = await _teamMemberService.GetAllAsync();
            return Ok(results);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<TeamMemberDto>> GetById(Guid id)
        {
            var result = await _teamMemberService.GetByIdAsync(id);
            if (result == null)
                return BadRequest("Team member with given ID doesn't exist");
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<TeamMemberDto>> Add(CreateTeamMemberDto teamMemberDto)
        {
            var member = await _teamMemberService.AddAsync(teamMemberDto);
            return member == null ? BadRequest() : Ok(member);
        }

        [HttpPut]
        public async Task<ActionResult<TeamMemberDto>> Update(TeamMemberDto teamMemberDto)
        {
            var member = await _teamMemberService.UpdateAsync(teamMemberDto);
            return member == null ? BadRequest("Team member with given ID doesn't exist") : Ok(member);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool success = await _teamMemberService.DeleteAsync(id);
            if (success)
                return Ok();
            return BadRequest("Team member with given ID doesn't exist");
        }
    }
}
