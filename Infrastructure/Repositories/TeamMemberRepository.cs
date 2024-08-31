using Domain.Entities;
using Domain.Helpers;
using Domain.QueryStrings;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TeamMemberRepository : ITeamMemberRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public TeamMemberRepository(RepositoryDbContext context)
        {
            _dbContext = context;
        }

        public async Task<PaginatedList<TeamMember>> GetAllAsync(QueryStringParameters parameters)
        {
            //var allMembers = await _dbContext.TeamMembers
            //    .Where(a => a.Name.Split(" ").Any(n => n.StartsWith(parameters.FirstLetter)) && a.Name.Contains(parameters.SearchText))
            //    .Include(t => t.Status)
            //    .ToListAsync();
            var allMembers = await _dbContext.TeamMembers
                .Where(a => (a.Name.StartsWith(parameters.FirstLetter) ||
                             a.Name.Contains(" " + parameters.FirstLetter))
                            && a.Name.Contains(parameters.SearchText))
                .Include(t => t.Status)
                .ToListAsync();
            var allMembersQuearriable = allMembers.AsQueryable();
            var members =
                PaginatedList<TeamMember>.ToPagedList(allMembersQuearriable, parameters.PageNumber,
                    parameters.PageSize);
            return members;
        }

        public async Task<TeamMember?> GetByIdAsync(Guid id)
        {
            return await _dbContext.TeamMembers
                .Include(t => t.Status)
                .SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TeamMember>> GetActive()
        {
            return await _dbContext.TeamMembers
                .Where(t => t.Status.StatusName == "Active")
                .Include(t => t.Status)
                .ToArrayAsync();
        }

        public async Task AddAsync(TeamMember member)
        {
            await _dbContext.TeamMembers.AddAsync(member);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingMember = await _dbContext.TeamMembers.FindAsync(id);
            if (existingMember is null)
            {
                return false;
            }
            _dbContext.TeamMembers.Remove(existingMember);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        
    }
}
