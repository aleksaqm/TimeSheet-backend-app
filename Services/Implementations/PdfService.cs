using System.Text;
using DocumentFormat.OpenXml.Drawing.Charts;
using Domain.Repositories;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Services.Abstractions;
using Shared;

namespace Services.Implementations
{
    public class PdfService : IPdfService
    {
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IReportService _reportService;

        private string _teamMemberName = "All";
        private string _clientName = "All";
        private string _projectName = "All";
        private string _categoryName = "All";

        public PdfService(ITeamMemberRepository teamMemberRepo, IClientRepository clientRepo, IProjectRepository projectRepo, ICategoryRepository categoryRepo, IReportService reportService)
        {
            _teamMemberRepository = teamMemberRepo;   
            _clientRepository = clientRepo;
            _projectRepository = projectRepo;
            _categoryRepository = categoryRepo;
            _reportService = reportService;
        }

        public async Task<byte[]> GenerateReportPdf(GetReportDto reportPdfDto)
        {
            await GetNames(reportPdfDto);
            var result = await _reportService.GetReportAsync(reportPdfDto);


            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    SetPageSettings(page);

                    SetPageHeader(page, reportPdfDto.StartDate, reportPdfDto.EndDate);

                    page.Content().Element(ComposeContent);

                    void ComposeContent(IContainer container)
                    {
                        container.Column(column =>
                        {
                            column.Spacing(5);

                            column.Item().PaddingBottom(30).Text($"Total Hours: {result.ReportTotalHours}").FontSize(14);

                            column.Item().Table(table =>
                            {
                                SetTableColumnsDefinition(table);

                                SetTableHeader(table);

                                PopulateTable(table, result.Reports);
                            });
                        });
                    }

                    SetPageFooter(page);
                });
            });

            return document.GeneratePdf();
        }

        private void SetPageSettings(PageDescriptor page)
        {
            page.Size(PageSizes.A4);
            page.Margin(2, Unit.Centimetre);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(12));
        }

        private void SetPageHeader(PageDescriptor page, DateTime startDate, DateTime endDate)
        {
            page.Header().Column(column =>
            {
                column.Item().PaddingBottom(50).Text("Report").FontSize(20).Bold().AlignCenter();
                column.Item().PaddingBottom(30).Text($"Team Member: {_teamMemberName}").FontSize(14);
                column.Item().PaddingBottom(30).Text($"Client: {_clientName}").FontSize(14);
                column.Item().PaddingBottom(30).Text($"Project: {_projectName}").FontSize(14);
                column.Item().PaddingBottom(30).Text($"Category: {_categoryName}").FontSize(14);
                column.Item().PaddingBottom(30).Text($"Report Period: {startDate.ToShortDateString()} - {endDate.ToShortDateString()}").FontSize(14);
            });
        }

        private void SetTableColumnsDefinition(TableDescriptor table)
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
        }

        private void SetTableHeader(TableDescriptor table)
        {
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
                    return container.DefaultTextStyle(x => x.SemiBold()).Padding(3).BorderBottom(2).BorderRight(1).BorderColor(Colors.Black);
                }
            });
        }

        private void PopulateTable(TableDescriptor table, IEnumerable<ReportDto> reports)
        {
            foreach (var report in reports)
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
                    return container.Padding(3).BorderBottom(1).BorderRight(1).BorderColor(Colors.Grey.Lighten2);
                }
            }
        }

        private void SetPageFooter(PageDescriptor page)
        {
            page.Footer().AlignCenter().Text(x =>
            {
                x.Span("Page ");
                x.CurrentPageNumber();
                x.Span(" of ");
                x.TotalPages();
            });
        }

        private async Task GetNames(GetReportDto report)
        {
            if (report.TeamMemberId.HasValue)
            {
                var member = await _teamMemberRepository.GetByIdAsync((Guid)report.TeamMemberId);
                if (member is not null)
                {
                    _teamMemberName = member.Name;
                }
            }
            if (report.ClientId.HasValue)
            {
                var client = await _clientRepository.GetByIdAsync((Guid)report.ClientId);
                if (client is not null)
                {
                    _clientName = client.Name;
                }
            }
            if (report.ProjectId.HasValue)
            {
                var project = await _projectRepository.GetByIdAsync((Guid)report.ProjectId);
                if (project is not null)
                {
                    _projectName = project.Name;
                }
            }
            if (report.CategoryId.HasValue)
            {
                var category = await _categoryRepository.GetByIdAsync((Guid)report.CategoryId);
                if (category is not null)
                {
                    _categoryName = category.Name;
                }
            }
            
        }

    }
}
