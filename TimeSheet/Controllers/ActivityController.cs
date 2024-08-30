using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
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
            if (result is null)
            {
                return BadRequest("Activity with given ID doesn't exist");
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("Month")]
        public async Task<ActionResult<List<WorkDayDto>>> GetByMonth([Required] int year, [Required] int month, [Required] Guid userId)
        {
            var results = await _activityService.GetByMonth(year, month, userId);
            return Ok(results);
        }

        [HttpGet]
        [Route("Day")]
        public async Task<ActionResult<List<ActivityDto>>> GetForOnaDay([Required] DateTime day, [Required] Guid userId)
        {
            var results = await _activityService.GetForOneDay(day, userId);
            return Ok(results);
        }

        [HttpPost]
        public async Task<ActionResult<ActivityDto>> Add(ActivityCreateDto activityDto)
        {
            var activity = await _activityService.AddAsync(activityDto);
            return activity is null ? BadRequest() : Ok(activity);
        }

        [HttpPut]
        public async Task<ActionResult<ActivityDto>> Update(ActivityUpdateDto activityDto)
        {
            var activity = await _activityService.UpdateAsync(activityDto);
            return activity is null ? BadRequest("Activity with given ID doesn't exist") : Ok(activity);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool success = await _activityService.DeleteAsync(id);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Activity with given ID doesn't exist");
        }



    }
}
