using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Infrastructure.UnitOfWork;
using Services.Abstractions;
using Services.Updaters;
using Shared;

namespace Services.Implementations
{
    public class ActivityService : IActivityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ActivityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivityResponse>> GetAllAsync()
        {
            var activities = await _unitOfWork.ActivityRepository.GetAllAsync();
            return _mapper.Map<List<ActivityResponse>>(activities);
        }

        public async Task<ActivityResponse?> GetByIdAsync(Guid id)
        {
            var activity = await _unitOfWork.ActivityRepository.GetByIdAsync(id);
            if (activity is null)
            {
                throw new ActivityNotFoundException("Activity with given ID doesn't exist");
            }
            return _mapper.Map<ActivityResponse>(activity);
        }

        public async Task<IEnumerable<ActivityResponse>> GetForOneDay(DateTime day, Guid userId)
        {
            var activities = await _unitOfWork.ActivityRepository.GetForOneDay(day, userId);
            return _mapper.Map<List<ActivityResponse>>(activities);
        }

        public async Task<IEnumerable<WorkDayDto>> GetActivitiesForPeriod(DateTime startDate, DateTime endDate, Guid userId)
        {
            CheckDates(startDate, endDate);
            List<WorkDayDto> days = new List<WorkDayDto>();
            while (startDate.Date <= endDate.Date)
            {
                var activities = await _unitOfWork.ActivityRepository.GetForOneDay(startDate, userId);
                var totalHours = activities.Sum(x => x.Hours + (double)x.Overtime);
                days.Add(new WorkDayDto { Activities = _mapper.Map<List<ActivityResponse>>(activities), Date = startDate, TotalHours = totalHours }); //
                startDate = startDate.AddDays(1);
            }
            return days;
        }

        public async Task<DaysHoursResponse> GetHoursForPeriod(DateTime startDate, DateTime endDate, Guid userId)
        {
            CheckDates(startDate, endDate);
            double totalHours = 0;
            var days = new List<DayHours>();
            while (startDate.Date <= endDate.Date)
            {
                var activities = await _unitOfWork.ActivityRepository.GetForOneDay(startDate, userId);
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
            await _unitOfWork.ActivityRepository.AddAsync(activity);
            var fullActivity = await _unitOfWork.ActivityRepository.GetByIdAsync(activity.Id);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ActivityResponse>(fullActivity);
        }

        public async Task<ActivityResponse?> UpdateAsync(ActivityUpdateDto activityDto)
        {
            var activity = _mapper.Map<Activity>(activityDto);
            var existingActivity = await _unitOfWork.ActivityRepository.GetByIdAsync(activity.Id);
            if (existingActivity is null)
            {
                throw new ActivityNotFoundException("Activity with given ID doesnt exist");
            }
            ActivityUpdater.Update(activity, existingActivity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ActivityResponse>(existingActivity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            bool success = await _unitOfWork.ActivityRepository.DeleteAsync(id);
            if (success)
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            throw new ActivityNotFoundException("Activity with given ID doesnt exist");
        }

        private void CheckDates(DateTime startDate, DateTime endDate)
        {
            if (startDate.Date > endDate.Date)
            {
                throw new InvalidDatesException("Start date is after end date");
            }
        }
        
    }
}
