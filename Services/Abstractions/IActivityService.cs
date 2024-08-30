using Shared;

namespace Services.Abstractions
{
    public interface IActivityService
    {
        Task<ActivityDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ActivityDto>> GetAllAsync();
        Task<IEnumerable<WorkDayDto>> GetByMonth(int year, int month, Guid userId);
        Task<IEnumerable<ActivityDto>> GetForOneDay(DateTime day, Guid userId);
        Task<ActivityDto?> AddAsync(ActivityCreateDto activityDto);
        Task<ActivityDto?> UpdateAsync(ActivityUpdateDto activityDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
