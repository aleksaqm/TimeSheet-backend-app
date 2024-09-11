using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.UnitOfWork;
using Moq;
using Services.Abstractions;
using Services.Implementations;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Services.Test.UnitTests
{
    public class ReportServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IReportService _reportService;

        public ReportServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _reportService = new ReportService(_unitOfWorkMock.Object, _mapperMock.Object); 
        }

        [Fact]
        public async Task GetReportAsync_InvalidDates_ThrowsInvalidDatesException()
        {
            var getReportDto = new GetReportDto { StartDate = DateTime.Now.AddDays(1), EndDate = DateTime.Now };
        
            await Assert.ThrowsAsync<InvalidDatesException>(() => _reportService.GetReportAsync(getReportDto));
        }

        [Fact]
        public async Task GetReportAsync_ValidData_ReturnsReport()
        {
            var getReportDto = new GetReportDto { StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };

            var activities = new List<Activity>();
            var reportDto1 = new ReportDto {Category = "Test", Client = "Test", Project = "Test", TeamMember = "Test" ,Time = 1 };
            var reportDto2 = new ReportDto { Category = "Test", Client = "Test", Project = "Test", TeamMember = "Test", Time = 2 };
            List<ReportDto> reportDtos = [reportDto1, reportDto2];

            _unitOfWorkMock.Setup(u => u.ActivityRepository.GetForReport(getReportDto));
            _mapperMock.Setup(m => m.Map<List<ReportDto>>(activities)).Returns(reportDtos);

            var result = await _reportService.GetReportAsync(getReportDto);

            Assert.NotNull(result);
            Assert.Equal(3, result.ReportTotalHours);
            Assert.Equal(reportDtos, result.Reports);
        }

    }
}
