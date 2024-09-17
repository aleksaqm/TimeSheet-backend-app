using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;
using System.ComponentModel.DataAnnotations;

namespace TimeSheet.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService service)
        {
            _activityService = service;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<ActivityResponse>>> GetAll()
        {
            var results = await _activityService.GetAllAsync();
            return Ok(results);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<ActivityResponse>> GetById(Guid id)
        {
            var result = await _activityService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Route("Days")]
        public async Task<ActionResult<List<WorkDayDto>>> GetActivitiesForPeriod([Required] DateTime startDate, [Required] DateTime endDate, [Required] Guid userId)
        {
            var results = await _activityService.GetActivitiesForPeriod(startDate, endDate, userId);
            return Ok(results);
        }

        [HttpGet]
        [Route("Hours")]
        public async Task<ActionResult<DaysHoursResponse>> GetHoursForPeriod([Required] DateTime startDate, [Required] DateTime endDate, [Required] Guid userId)
        {
            var result = await _activityService.GetHoursForPeriod(startDate, endDate, userId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ActivityResponse>> Add(ActivityCreateDto activityDto)
        {
            var activity = await _activityService.AddAsync(activityDto);
            return Ok(activity);
        }

        [HttpPut]
        public async Task<ActionResult<ActivityResponse>> Update(ActivityUpdateDto activityDto)
        {
            var activity = await _activityService.UpdateAsync(activityDto);
            return Ok(activity);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool success = await _activityService.DeleteAsync(id);
            return Ok();
        }

        

    }
}
