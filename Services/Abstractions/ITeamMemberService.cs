using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
