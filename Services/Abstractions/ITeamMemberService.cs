using Domain.Helpers;
using Domain.QueryStrings;
using Shared;

namespace Services.Abstractions
{
    public interface ITeamMemberService
    {
        Task<TeamMemberResponse?> GetByIdAsync(Guid id);
        Task<PaginatedList<TeamMemberResponse>> GetAllAsync(QueryStringParameters parameters);
        Task<IEnumerable<TeamMemberResponse>> GetActive();
        Task<TeamMemberResponse?> AddAsync(TeamMemberCreateDto teamMemberDto);
        Task<TeamMemberResponse?> UpdateAsync(TeamMemberUpdateDto teamMemberDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
