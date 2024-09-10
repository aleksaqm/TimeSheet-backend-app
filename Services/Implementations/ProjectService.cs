using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Helpers;
using Domain.QueryStrings;
using Domain.Repositories;
using Infrastructure.UnitOfWork;
using Services.Abstractions;
using Services.Converters;
using Shared;

namespace Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ProjectResponse>> GetAllAsync(QueryStringParameters parameters)
        {
            var projects = await _unitOfWork.ProjectRepository.GetAllAsync(parameters);
            var mapped = _mapper.Map<PaginatedList<ProjectResponse>>(projects);
            PaginatedListConverter<Project, ProjectResponse>.Convert(projects, mapped);
            return mapped;
        }

        public async Task<ProjectResponse?> GetByIdAsync(Guid id)
        {
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
            if (project is null)
            {
                throw new ProjectNotFoundException("Project with given ID doesnt exist");
            }
            return _mapper.Map<ProjectResponse>(project);
        }

        public async Task<IEnumerable<ProjectResponse>> GetByStatus(string status)
        {
            var projects = await _unitOfWork.ProjectRepository.GetByStatus(status);
            return _mapper.Map<List<ProjectResponse>>(projects);
        }

        public async Task<ProjectResponse?> AddAsync(ProjectCreateDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            await _unitOfWork.ProjectRepository.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();
            var fullProject = await _unitOfWork.ProjectRepository.GetByIdAsync(project.Id);
            return _mapper.Map<ProjectResponse>(fullProject);
        }

        public async Task<ProjectResponse?> UpdateAsync(ProjectUpdateDto projectDto)
        {
            var project = _mapper.Map<Project>(projectDto);
            var existingProject = await _unitOfWork.ProjectRepository.GetByIdAsync(project.Id);
            if (existingProject is null)
            {
                throw new ProjectNotFoundException("Project with given ID doesnt exist");
            }
            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.CustomerId = project.CustomerId;
            existingProject.LeadId = project.LeadId;
            existingProject.Status.StatusName = project.Status.StatusName;
            await _unitOfWork.SaveChangesAsync();
            //for full return - _repository.GetByIdAsync(project.Id);
            return _mapper.Map<ProjectResponse>(existingProject);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            bool success = await _unitOfWork.ProjectRepository.DeleteAsync(id);
            if (success)
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            throw new ProjectNotFoundException("Project with given ID doesnt exist");
        }

        
    }
}
