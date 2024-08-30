using Shared;

namespace Services.Abstractions
{
    public interface IProjectService
    {
        Task<ProjectDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProjectDto>> GetAllAsync();
        Task<IEnumerable<ProjectDto>> GetByStatus(string status);
        Task<ProjectDto?> AddAsync(ProjectCreateDto projectDto);
        Task<ProjectDto?> UpdateAsync(ProjectUpdateDto projectDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
