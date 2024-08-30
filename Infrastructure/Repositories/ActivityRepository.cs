using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<IEnumerable<Activity>> GetByMonth(int year, int month, Guid userId)
        {
            return await _dbContext.Activities
                .Where(a => EF.Property<int>(a, "Year") == year && EF.Property<int>(a, "Month") == month)
                .Where(a => a.UserId == userId)
                .Include(t => t.Client)
                .Include(t => t.Category)
                .Include(t => t.Project)
                .Include(t => t.User)
                    .ThenInclude(t => t.Status)
                .ToArrayAsync();
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

        public async Task AddAsync(Activity activity)
        {
            await _dbContext.Activities.AddAsync(activity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingActivity = await _dbContext.Activities.FindAsync(id);
            if (existingActivity == null)
            {
                return false;
            }
            _dbContext.Activities.Remove(existingActivity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        
    }
}
