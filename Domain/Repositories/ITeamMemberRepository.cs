using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface ITeamMemberRepository
    {
        Task<TeamMember?> GetByIdAsync(Guid id);
        Task<IEnumerable<TeamMember>> GetAllAsync();
        Task AddAsync(TeamMember member);
        Task<TeamMember?> UpdateAsync(TeamMember member);
        Task<bool> DeleteAsync(Guid id);
    }
}
