using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

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

        public async Task UpdateAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingProject = await _dbContext.Projects.FindAsync(id);
            if (existingProject is null) 
            {
                return false;
            };
            _dbContext.Projects.Remove(existingProject);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
