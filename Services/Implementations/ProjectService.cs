using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Helpers;
using Domain.QueryStrings;
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

        public async Task<PaginatedList<ProjectResponse>> GetAllAsync(QueryStringParameters parameters)
        {
            var projects = await _repository.GetAllAsync(parameters);
            var mapped = _mapper.Map<PaginatedList<ProjectResponse>>(projects);
            mapped.CurrentPage = projects.CurrentPage;
            mapped.HasNext = projects.HasNext;
            mapped.PageSize = projects.PageSize;
            return mapped;
        }

        public async Task<ProjectResponse?> GetByIdAsync(Guid id)
        {
            var project = await _repository.GetByIdAsync(id);
            if (project is null)
            {
                throw new ProjectNotFoundException("Project with given ID doesnt exist");
            }
            return _mapper.Map<ProjectResponse>(project);
        }

        public async Task<IEnumerable<ProjectResponse>> GetByStatus(string status)
        {
            var projects = await _repository.GetByStatus(status);
            return _mapper.Map<List<ProjectResponse>>(projects);
        }

        public async Task<ProjectResponse?> AddAsync(ProjectCreateDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            await _repository.AddAsync(project);
            var fullProject = await _repository.GetByIdAsync(project.Id);
            return _mapper.Map<ProjectResponse>(fullProject);
        }

        public async Task<ProjectResponse?> UpdateAsync(ProjectUpdateDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            var existingProject = await _repository.GetByIdAsync(project.Id);
            if (existingProject is null)
            {
                throw new ProjectNotFoundException("Project with given ID doesnt exist");
            }
            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.CustomerId = project.CustomerId;
            existingProject.LeadId = project.LeadId;
            existingProject.Status.StatusName = project.Status.StatusName;
            await _repository.UpdateAsync();
            //for full return - _repository.GetByIdAsync(project.Id);
            return _mapper.Map<ProjectResponse>(existingProject);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            bool success = await _repository.DeleteAsync(id);
            if (success)
            {
                return true;
            }
            throw new ProjectNotFoundException("Project with given ID doesnt exist");
        }

        
    }
}
