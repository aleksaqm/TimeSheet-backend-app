using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public ProjectRepository(RepositoryDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            var projects = await _dbContext.Projects.Include(t => t.Status).Include(t => t.Customer).Include(t => t.Lead).ToArrayAsync();
            return projects;
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Projects
                .Include(t => t.Status)
                .Include(t => t.Customer)
                .Include(t => t.Lead)
                .SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(Project project)
        {
            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Project?> UpdateAsync(Project project)
        {
            var existingProject = await _dbContext.Projects.Include(t => t.Status).Include(t => t.Customer).Include(t => t.Lead).FirstOrDefaultAsync(t => t.Id == project.Id);
            if (existingProject == null)
            {
                return null;
            }
            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.CustomerId = project.CustomerId;
            existingProject.LeadId = project.LeadId;
            existingProject.Status.StatusName = project.Status.StatusName;
            await _dbContext.SaveChangesAsync();
            return existingProject;

        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingProject = await _dbContext.Projects.FindAsync(id);
            if (existingProject == null) return false;
            _dbContext.Projects.Remove(existingProject);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
