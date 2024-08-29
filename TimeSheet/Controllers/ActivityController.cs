using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Services.Implementations;
using Shared;
using System.ComponentModel.DataAnnotations;

namespace TimeSheet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService service)
        {
            _activityService = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ActivityDto>>> GetAll()
        {
            var results = await _activityService.GetAllAsync();
            return Ok(results);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<ActivityDto>> GetById(Guid id)
        {
            var result = await _activityService.GetByIdAsync(id);
            if (result == null)
                return BadRequest("Activity with given ID doesn't exist");
            return Ok(result);
        }

        [HttpGet]
        [Route("ByMonth")]
        public async Task<ActionResult<List<ActivityDto>>> GetByMonth([Required] int year, [Required] int month)
        {
            var results = await _activityService.GetAllAsync();
            return Ok(results);
        }

        [HttpPost]
        public async Task<ActionResult<ActivityDto>> Add(ActivityCreateDto activityDto)
        {
            var activity = await _activityService.AddAsync(activityDto);
            return activity == null ? BadRequest() : Ok(activity);
        }

        [HttpPut]
        public async Task<ActionResult<ActivityDto>> Update(ActivityUpdateDto activityDto)
        {
            var activity = await _activityService.UpdateAsync(activityDto);
            return activity == null ? BadRequest("Activity with given ID doesn't exist") : Ok(activity);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool success = await _activityService.DeleteAsync(id);
            if (success)
                return Ok();
            return BadRequest("Activity with given ID doesn't exist");
        }



    }
}
