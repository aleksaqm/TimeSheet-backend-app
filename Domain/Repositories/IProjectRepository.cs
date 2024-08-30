using Domain.Entities;

namespace Domain.Repositories
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(Guid id);
        Task<IEnumerable<Project>> GetAllAsync();
        Task<IEnumerable<Project>> GetByStatus(string status);
        Task AddAsync(Project project);
        Task UpdateAsync();
        Task<bool> DeleteAsync(Guid id);
    }
}
