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

        public async Task<IEnumerable<ActivityDto>> GetAllAsync()
        {
            var activities = await _repository.GetAllAsync();
            return _mapper.Map<List<ActivityDto>>(activities);
        }

        public async Task<ActivityDto?> GetByIdAsync(Guid id)
        {
            var activity = await _repository.GetByIdAsync(id);
            if (activity is null)
            {
                return null;
            }
            return _mapper.Map<ActivityDto>(activity);
        }

        public async Task<IEnumerable<ActivityDto>> GetForOneDay(DateTime day, Guid userId)
        {
            var activities = await _repository.GetForOneDay(day, userId);
            return _mapper.Map<List<ActivityDto>>(activities);
        }

        public async Task<IEnumerable<WorkDayDto>> GetActivitiesForPeriod(DateTime startDate, DateTime endDate, Guid userId)
        {
            List<WorkDayDto> days = new List<WorkDayDto>();
            while (startDate.Date <= endDate.Date)
            {
                var activities = await _repository.GetForOneDay(startDate, userId);
                days.Add(new WorkDayDto { Activities = _mapper.Map<List<ActivityDto>>(activities), Date = startDate, TotalHours = CalculateHours(activities) });
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
                var hours = CalculateHours(activities);
                days.Add(new DayHours { Date=startDate, Hours = hours });
                totalHours += hours;
                startDate = startDate.AddDays(1);
            }
            return new DaysHoursResponse { DayHours=days, TotalHours=totalHours};
        }

        public async Task<ActivityDto?> AddAsync(ActivityCreateDto activityDto)
        {
            var activity = _mapper.Map<Activity>(activityDto);
            try
            {
                await _repository.AddAsync(activity);
                var fullActivity = await _repository.GetByIdAsync(activity.Id);
                return _mapper.Map<ActivityDto>(fullActivity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ActivityDto?> UpdateAsync(ActivityUpdateDto activityDto)
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
            return _mapper.Map<ActivityDto>(existingActivity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
             return await _repository.DeleteAsync(id);
        }

        //private async Task<List<WorkDayDto>> CalculateDays(DateTime startDate, DateTime endDate, Guid userId)
        //{
        //    List<WorkDayDto> days = new List<WorkDayDto>();
        //    while (startDate.Date <= endDate.Date)
        //    {
        //        var activities = await _repository.GetForOneDay(startDate, userId);
        //        days.Add(new WorkDayDto { Activities = _mapper.Map<List<ActivityDto>>(activities), Date = startDate, TotalHours = CalculateHours(activities) });
        //    }
        //    return days;
        //}

        private double CalculateHours(IEnumerable<Activity> activities)
        {
            double hours = 0;
            foreach (var activity in activities)
            {
                hours += activity.Hours;
                if (activity.Overtime != null)
                {
                    hours += (double) activity.Overtime;
                }
            }
            return hours;
        }

        
    }
}
