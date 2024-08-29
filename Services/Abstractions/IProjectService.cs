using Shared;

namespace Services.Abstractions
{
    public interface IProjectService
    {
        Task<ProjectDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ProjectDto>> GetAllAsync();
        Task<ProjectDto?> AddAsync(ProjectCreateDto projectDto);
        Task<ProjectDto?> UpdateAsync(ProjectUpdateDto projectDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
