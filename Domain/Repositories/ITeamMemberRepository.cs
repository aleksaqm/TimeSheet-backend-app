using Domain.Entities;
using Domain.Helpers;
using Domain.QueryStrings;

namespace Domain.Repositories
{
    public interface ITeamMemberRepository
    {
        Task<TeamMember?> GetByIdAsync(Guid id);
        Task<PaginatedList<TeamMember>> GetAllAsync(QueryStringParameters parameters);
        Task<IEnumerable<TeamMember>> GetActive();
        Task AddAsync(TeamMember member);
        Task UpdateAsync();
        Task<bool> DeleteAsync(Guid id);
    }
}
