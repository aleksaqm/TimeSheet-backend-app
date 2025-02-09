using AutoMapper;
using Domain.Exceptions;
using Domain.Repositories;
using Infrastructure.UnitOfWork;
using Services.Abstractions;
using Shared;

namespace Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReportResponse> GetReportAsync(GetReportDto reportDto)
        {
            CheckDates(reportDto.StartDate, reportDto.EndDate);
            var activities = await _unitOfWork.ActivityRepository.GetForReport(reportDto);
            var reportDtos = _mapper.Map<List<ReportDto>>(activities);
            double reportTotalHours = reportDtos.Sum(report => report.Time);
            return new ReportResponse
            {
                Reports = reportDtos,
                ReportTotalHours = reportTotalHours
            };
        }


        public static void CheckDates(DateTime startDate, DateTime endDate)
        {
            if (startDate.Date > endDate.Date)
            {
                throw new InvalidDatesException("Start date is after end date");
            }
        }
    }
}
