using Shared;

namespace Services.Abstractions
{
    public interface IActivityService
    {
        Task<ActivityDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ActivityDto>> GetAllAsync();
        Task<IEnumerable<ActivityDto>> GetForOneDay(DateTime day, Guid userId);
        Task<IEnumerable<WorkDayDto>> GetActivitiesForPeriod(DateTime startDate, DateTime endDate, Guid userId);
        Task<DaysHoursResponse> GetHoursForPeriod(DateTime startDate, DateTime endDate, Guid userId);
        Task<ActivityDto?> AddAsync(ActivityCreateDto activityDto);
        Task<ActivityDto?> UpdateAsync(ActivityUpdateDto activityDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
