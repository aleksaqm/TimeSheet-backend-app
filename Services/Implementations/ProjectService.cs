using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var result = await _repository.UpdateAsync(project);
            if (result == null)
                return null;
            return _mapper.Map<ProjectDto>(project);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
