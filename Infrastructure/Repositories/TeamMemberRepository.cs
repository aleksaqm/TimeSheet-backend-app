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
            var query = _dbContext.TeamMembers.Where(t => t.Status.StatusName == "Active").AsQueryable();
            if (parameters.SearchText is not null)
            {
                query = query.Where(t => t.Name.Contains(parameters.SearchText));
            }
            if (parameters.FirstLetter is not null)
            {
                query = query.Where(t => t.Name.StartsWith(parameters.FirstLetter) ||
                                         t.Name.Contains(" " + parameters.FirstLetter));
            }

            query = query.Include(t => t.Status);

            var members =
                PaginatedList<TeamMember>.ToPagedList(query, parameters.PageNumber,
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
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingMember = await _dbContext.TeamMembers.FindAsync(id);
            if (existingMember is null)
            {
                return false;
            }
            _dbContext.TeamMembers.Remove(existingMember);
            return true;
        }

        public async Task<TeamMember?> GetByUsernameAsync(string username)
        {
            return await _dbContext.TeamMembers
                .Include(t => t.Status)
                .SingleOrDefaultAsync(t => t.Username == username);
        }

        public async Task<TeamMember?> GetByEmailAsync(string email)
        {
            return await _dbContext.TeamMembers
                .Include(t => t.Status)
                .SingleOrDefaultAsync(t => t.Email == email);
        }
    }
}
