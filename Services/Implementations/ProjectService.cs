using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Services.Abstractions;
using Shared;

namespace Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repository;
        private readonly IMapper _mapper;

        public ProjectService(IProjectRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllAsync()
        {
            var projects = await _repository.GetAllAsync();
            return _mapper.Map<List<ProjectDto>>(projects);
        }

        public async Task<ProjectDto?> GetByIdAsync(Guid id)
        {
            var project = await _repository.GetByIdAsync(id);
            if (project is null)
            {
                return null;
            }
            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<ProjectDto?> AddAsync(ProjectCreateDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            try
            {
                await _repository.AddAsync(project);
                var fullProject = await _repository.GetByIdAsync(project.Id);
                return _mapper.Map<ProjectDto>(fullProject);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ProjectDto?> UpdateAsync(ProjectUpdateDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            var existingProject = await _repository.GetByIdAsync(project.Id);
            if (existingProject is null)
            {
                return null;
            }
            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.CustomerId = project.CustomerId;
            existingProject.LeadId = project.LeadId;
            existingProject.Status.StatusName = project.Status.StatusName;
            await _repository.UpdateAsync();
            //for full return - _repository.GetByIdAsync(project.Id);
            return _mapper.Map<ProjectDto>(existingProject);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
