using Domain.Helpers;
using Domain.QueryStrings;
using Shared;

namespace Services.Abstractions
{
    public interface IProjectService
    {
        Task<ProjectResponse?> GetByIdAsync(Guid id);
        Task<PaginatedList<ProjectResponse>> GetAllAsync(QueryStringParameters parameters);
        Task<IEnumerable<ProjectResponse>> GetByStatus(string status);
        Task<ProjectResponse?> AddAsync(ProjectCreateDto projectDto);
        Task<ProjectResponse?> UpdateAsync(ProjectUpdateDto projectDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
