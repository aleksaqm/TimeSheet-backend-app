using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Services.Abstractions;
using Shared;

namespace Services.Implementations
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _repository;
        private readonly IMapper _mapper;

        public ActivityService(IActivityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivityResponse>> GetAllAsync()
        {
            var activities = await _repository.GetAllAsync();
            return _mapper.Map<List<ActivityResponse>>(activities);
        }

        public async Task<ActivityResponse?> GetByIdAsync(Guid id)
        {
            var activity = await _repository.GetByIdAsync(id);
            if (activity is null)
            {
                return null;
            }
            return _mapper.Map<ActivityResponse>(activity);
        }

        public async Task<IEnumerable<ActivityResponse>> GetForOneDay(DateTime day, Guid userId)
        {
            var activities = await _repository.GetForOneDay(day, userId);
            return _mapper.Map<List<ActivityResponse>>(activities);
        }

        public async Task<IEnumerable<WorkDayDto>> GetActivitiesForPeriod(DateTime startDate, DateTime endDate, Guid userId)
        {
            List<WorkDayDto> days = new List<WorkDayDto>();
            while (startDate.Date <= endDate.Date)
            {
                var activities = await _repository.GetForOneDay(startDate, userId);
                var totalHours = activities.Sum(x => x.Hours + (double)x.Overtime);
                days.Add(new WorkDayDto { Activities = _mapper.Map<List<ActivityResponse>>(activities), Date = startDate, TotalHours = totalHours }); //
                startDate = startDate.AddDays(1);
            }
            return days;
        }

        public async Task<DaysHoursResponse> GetHoursForPeriod(DateTime startDate, DateTime endDate, Guid userId)
        {
            double totalHours = 0;
            var days = new List<DayHours>();
            while (startDate.Date <= endDate.Date)
            {
                var activities = await _repository.GetForOneDay(startDate, userId);
                double hours = activities.Sum(x => x.Hours + (double)x.Overtime);
                days.Add(new DayHours { Date=startDate, Hours = hours });
                totalHours += hours;
                startDate = startDate.AddDays(1);
            }
            return new DaysHoursResponse { DayHours=days, TotalHours=totalHours};
        }

        public async Task<ActivityResponse?> AddAsync(ActivityCreateDto activityDto)
        {
            var activity = _mapper.Map<Activity>(activityDto);
            try
            {
                await _repository.AddAsync(activity);
                var fullActivity = await _repository.GetByIdAsync(activity.Id);
                return _mapper.Map<ActivityResponse>(fullActivity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ActivityResponse?> UpdateAsync(ActivityUpdateDto activityDto)
        {
            var activity = _mapper.Map<Activity>(activityDto);
            var existingActivity = await _repository.GetByIdAsync(activity.Id);
            if (existingActivity is null)
            {
                return null;
            }
            existingActivity.Date = activity.Date;
            existingActivity.CategoryId = activity.CategoryId;
            existingActivity.ClientId = activity.ClientId;
            existingActivity.ProjectId = activity.ProjectId;
            existingActivity.UserId = activity.UserId;
            existingActivity.Description = activity.Description;
            existingActivity.Hours = activity.Hours;
            existingActivity.Overtime = activity.Overtime;
            await _repository.UpdateAsync();
            return _mapper.Map<ActivityResponse>(existingActivity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
             return await _repository.DeleteAsync(id);
        }

        public async Task<ReportResponse> GetReportAsync(GetReportDto reportDto)
        {
            var activities = await _repository.GetForReport(reportDto);
            var reportDtos = _mapper.Map<List<ReportDto>>(activities);
            double reportTotalHours = reportDtos.Sum(report => report.Time);
            return new ReportResponse
            {
                Reports = reportDtos,
                ReportTotal = reportTotalHours
            };
        }
        
    }
}
