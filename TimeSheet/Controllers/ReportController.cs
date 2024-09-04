using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;

namespace TimeSheet.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IPdfService _pdfService;

        public ReportController(IReportService service, IPdfService pdfService)
        {
            _reportService = service;
            _pdfService = pdfService;
        }

        [HttpGet]
        public async Task<ActionResult<ReportResponse>> GetReport([FromQuery] GetReportDto reportDto)
        {
            var result = await _reportService.GetReportAsync(reportDto);
            return Ok(result);
        }

        [HttpPost]
        [Route("Pdf")]
        public FileResult GeneratePdf([FromBody] ReportPdfDto report)
        {
            var pdf = _pdfService.GenerateReportPdf(report);
            return File(pdf, "application/pdf", "report.pdf");
        }

    }
}
