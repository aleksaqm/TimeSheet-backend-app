using Shared;

namespace Services.Abstractions
{
    public interface ITeamMemberService
    {
        Task<TeamMemberDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<TeamMemberDto>> GetAllAsync();
        Task<TeamMemberDto?> AddAsync(TeamMemberCreateDto teamMemberDto);
        Task<TeamMemberDto?> UpdateAsync(TeamMemberDto teamMemberDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
