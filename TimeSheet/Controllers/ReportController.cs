using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;

namespace TimeSheet.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService service)
        {
            _reportService = service;
        }

        [HttpGet]
        public async Task<ActionResult<ReportResponse>> GetReport([FromQuery] GetReportDto reportDto)
        {
            var result = await _reportService.GetReportAsync(reportDto);
            return Ok(result);
        }
    }
}
