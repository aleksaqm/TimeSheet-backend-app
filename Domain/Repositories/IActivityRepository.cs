using Domain.Entities;

namespace Domain.Repositories
{
    public interface IActivityRepository
    {
        Task<Activity?> GetByIdAsync(Guid id);
        Task<IEnumerable<Activity>> GetAllAsync();
        Task AddAsync(Activity activity);
        Task UpdateAsync();
        Task<bool> DeleteAsync(Guid id);
    }
}
