using System.Data;
using ClosedXML.Excel;
using Services.Abstractions;
using Shared;

namespace Services.Implementations
{
    public class ExcelService : IExcelService
    {
        public byte[] ExportToExcel(IEnumerable<ReportDto> reports)
        {
            var dataTable = new DataTable("Report");
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
