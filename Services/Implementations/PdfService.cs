using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Services.Abstractions;
using Shared;

namespace Services.Implementations
{
    public class PdfService : IPdfService
    {
        public byte[] GenerateReportPdf(ReportPdfDto reportPdfDto)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Column(column =>
                    {
                        column.Item().Text("Report").FontSize(20).Bold().AlignCenter();
                        column.Item().PaddingBottom(10).Text($"Team Member: {reportPdfDto.TeamMember}").FontSize(14);
                        column.Item().PaddingBottom(10).Text($"Client: {reportPdfDto.Client}").FontSize(14);
                        column.Item().PaddingBottom(10).Text($"Project: {reportPdfDto.Project}").FontSize(14);
                        column.Item().PaddingBottom(10).Text($"Category: {reportPdfDto.Category}").FontSize(14);
                        column.Item().PaddingBottom(10).Text($"Report Period: {reportPdfDto.StartDate.ToShortDateString()} - {reportPdfDto.EndDate.ToShortDateString()}").FontSize(14);
                    });

                    page.Content().Element(ComposeContent);

                    void ComposeContent(IContainer container)
                    {
                        container.Column(column =>
                        {
                            column.Spacing(5);

                            column.Item().Text($"Total Hours: {reportPdfDto.ReportTotalHours}").FontSize(14);

                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Date");
                                    header.Cell().Element(CellStyle).Text("Client");
                                    header.Cell().Element(CellStyle).Text("Project");
                                    header.Cell().Element(CellStyle).Text("Category");
                                    header.Cell().Element(CellStyle).Text("Description");
                                    header.Cell().Element(CellStyle).Text("Time");
                                    header.Cell().Element(CellStyle).Text("Team Member");

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                    }
                                });

                                foreach (var report in reportPdfDto.Reports)
                                {
                                    table.Cell().Element(CellStyle).Text(report.Date.ToShortDateString());
                                    table.Cell().Element(CellStyle).Text(report.Client);
                                    table.Cell().Element(CellStyle).Text(report.Project);
                                    table.Cell().Element(CellStyle).Text(report.Category);
                                    table.Cell().Element(CellStyle).Text(report.Description ?? string.Empty);
                                    table.Cell().Element(CellStyle).Text(report.Time.ToString());
                                    table.Cell().Element(CellStyle).Text(report.TeamMember);

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Grey.Lighten2);
                                    }
                                }
                            });
                        });
                    }

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
                });
            });

            return document.GeneratePdf();
        }


    }
}
