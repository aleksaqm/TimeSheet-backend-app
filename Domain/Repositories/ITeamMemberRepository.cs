﻿using Domain.Entities;
using Domain.Helpers;
using Domain.QueryStrings;

namespace Domain.Repositories
{
    public interface ITeamMemberRepository
    {
        Task<TeamMember?> GetByIdAsync(Guid id);
        Task<TeamMember?> GetByUsernameAsync(string username);
        Task<TeamMember?> GetByEmailAsync(string email);
        Task<PaginatedList<TeamMember>> GetAllAsync(QueryStringParameters parameters);
        Task<IEnumerable<TeamMember>> GetActive();
        Task AddAsync(TeamMember member);
        Task<bool> DeleteAsync(Guid id);
    }
}
