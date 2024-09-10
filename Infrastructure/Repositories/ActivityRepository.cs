using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Infrastructure.Repositories
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public ActivityRepository(RepositoryDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Activity>> GetAllAsync()
        {
            var activities = await _dbContext.Activities
                .Include(t => t.Client)
                .Include(t => t.Category)
                .Include(t => t.Project)
                .Include(t => t.User)
                    .ThenInclude(t => t.Status)
                .ToArrayAsync();
            return activities;
        }

        public async Task<Activity?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Activities
                .Include(t => t.Client)
                .Include(t => t.Category)
                .Include(t => t.Project)
                .Include(t => t.User).ThenInclude(t => t.Status)
                .SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Activity>> GetForOneDay(DateTime day, Guid userId)
        {
            return await _dbContext.Activities
                .Where(a => a.Date.Date == day.Date)
                .Where (a => a.UserId == userId)
                .Include(t => t.Client)
                .Include(t => t.Category)
                .Include(t => t.Project)
                .Include(t => t.User)
                    .ThenInclude(t => t.Status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Activity>> GetForPeriod(DateTime startDate, DateTime endDate, Guid userId)
        {
            var activities = await _dbContext.Activities
                .Where(a => a.Date.Date >= startDate && a.Date.Date <= endDate)
                .Include(t => t.Client)
                .Include(t => t.Category)
                .Include(t => t.Project)
                .Include(t => t.User)
                    .ThenInclude(t => t.Status)
                .ToArrayAsync();
            return activities;
        }

        public async Task AddAsync(Activity activity)
        {
            await _dbContext.Activities.AddAsync(activity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingActivity = await _dbContext.Activities.FindAsync(id);
            if (existingActivity == null)
            {
                return false;
            }
            _dbContext.Activities.Remove(existingActivity);
            return true;
        }

        public async Task<IEnumerable<Activity>> GetForReport(GetReportDto reportFilter)
        {
            var query = _dbContext.Activities.AsQueryable();
            if (reportFilter.TeamMemberId.HasValue)
            {
                query = query.Where(a => a.UserId == reportFilter.TeamMemberId.Value);
            }
            if (reportFilter.ClientId.HasValue)
            {
                query = query.Where(a => a.ClientId == reportFilter.ClientId.Value);
            }
            if (reportFilter.ProjectId.HasValue)
            {
                query = query.Where(a => a.ProjectId == reportFilter.ProjectId.Value);
            }
            if (reportFilter.CategoryId.HasValue)
            {
                query = query.Where(a => a.CategoryId == reportFilter.CategoryId.Value);
            }
            query = query.Where(a => a.Date >= reportFilter.StartDate && a.Date <= reportFilter.EndDate);
            query = query
                .Include(a => a.Client)
                .Include(a => a.Category)
                .Include(a => a.Project)
                .Include(a => a.User);
            return await query.ToListAsync();
        }
    }
}
