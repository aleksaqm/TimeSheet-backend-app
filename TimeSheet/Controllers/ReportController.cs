using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;

namespace TimeSheet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ReportController(IActivityService service)
        {
            _activityService = service;
        }

        [HttpGet]
        public async Task<ActionResult<ReportResponse>> GetReport([FromQuery] GetReportDto reportDto)
        {
            var result = await _activityService.GetReportAsync(reportDto);
            return Ok(result);
        }
    }
}
