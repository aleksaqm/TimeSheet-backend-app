using System.Data;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using Services.Abstractions;
using Shared;

namespace Services.Implementations
{
    public class ExcelService : IExcelService
    {
        private readonly IReportService _reportService;

        public ExcelService(IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<byte[]> ExportToExcel(GetReportDto reportDto)
        {
            var result = await _reportService.GetReportAsync(reportDto);
            var dataTable = new DataTable("Report");
            PopulateTable(dataTable, result.Reports);
            return GenerateBytes(dataTable);
        }

        private void PopulateTable(DataTable dataTable, IEnumerable<ReportDto> reports)
        {
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new("Date"),
                new ("Team Member"),
                new ("Project"),
                new("Category"),
                new("Description"),
                new("Time")
            });
            foreach (var report in reports)
            {
                dataTable.Rows.Add(report.Date.ToShortDateString(), report.TeamMember, report.Project,
                    report.Category, report.Description, report.Time);

            }
        }

        private byte[] GenerateBytes(DataTable dataTable)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }
    }
}
