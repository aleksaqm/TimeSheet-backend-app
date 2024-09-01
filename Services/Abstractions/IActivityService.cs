using Shared;

namespace Services.Abstractions
{
    public interface IActivityService
    {
        Task<ActivityResponse?> GetByIdAsync(Guid id);
        Task<IEnumerable<ActivityResponse>> GetAllAsync();
        Task<IEnumerable<ActivityResponse>> GetForOneDay(DateTime day, Guid userId);
        Task<IEnumerable<WorkDayDto>> GetActivitiesForPeriod(DateTime startDate, DateTime endDate, Guid userId);
        Task<DaysHoursResponse> GetHoursForPeriod(DateTime startDate, DateTime endDate, Guid userId);
        Task<ActivityResponse?> AddAsync(ActivityCreateDto activityDto);
        Task<ActivityResponse?> UpdateAsync(ActivityUpdateDto activityDto);
        Task<bool> DeleteAsync(Guid id);
        Task<ReportResponse> GetReportAsync(GetReportDto reportDto);
    }
}
