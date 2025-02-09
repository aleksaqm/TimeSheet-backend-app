using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;

namespace TimeSheet.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IPdfService _pdfService;
        private readonly IExcelService _excelService;

        public ReportController(IReportService service, IPdfService pdfService, IExcelService excelService)
        {
            _reportService = service;
            _pdfService = pdfService;
            _excelService = excelService;
        }

        [HttpGet]
        public async Task<ActionResult<ReportResponse>> GetReport([FromQuery] GetReportDto reportDto)
        {
            var result = await _reportService.GetReportAsync(reportDto);
            return Ok(result);
        }

        [HttpGet]
        [Route("Pdf")]
        public async Task<FileResult> GeneratePdf([FromQuery] GetReportDto reportDto)
        {
            var pdf = await _pdfService.GenerateReportPdf(reportDto);
            return File(pdf, "application/pdf", "report.pdf");
        }

        [HttpGet]
        [Route("Excel")]
        public async Task<FileResult> ExportToExcel([FromQuery] GetReportDto reportDto)
        {
            var excelFile = await _excelService.ExportToExcel(reportDto);
            return File(excelFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "report.xlsx");
        }

    }
}
