using Domain.Entities;
using Domain.Helpers;
using Domain.QueryStrings;

namespace Domain.Repositories
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(Guid id);
        Task<PaginatedList<Project>> GetAllAsync(QueryStringParameters parameters);
        Task<IEnumerable<Project>> GetByStatus(string status);
        Task AddAsync(Project project);
        Task UpdateAsync();
        Task<bool> DeleteAsync(Guid id);
    }
}
