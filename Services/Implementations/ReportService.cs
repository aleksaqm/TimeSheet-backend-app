using AutoMapper;
using Domain.Exceptions;
using Domain.Repositories;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IActivityRepository _repository;
        private readonly IMapper _mapper;

        public ReportService(IActivityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ReportResponse> GetReportAsync(GetReportDto reportDto)
        {
            CheckDates(reportDto.StartDate, reportDto.EndDate);
            var activities = await _repository.GetForReport(reportDto);
            var reportDtos = _mapper.Map<List<ReportDto>>(activities);
            double reportTotalHours = reportDtos.Sum(report => report.Time);
            return new ReportResponse
            {
                Reports = reportDtos,
                ReportTotal = reportTotalHours
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
