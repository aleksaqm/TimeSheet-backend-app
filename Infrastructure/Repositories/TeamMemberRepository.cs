﻿using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TeamMemberRepository : ITeamMemberRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public TeamMemberRepository(RepositoryDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<TeamMember>> GetAllAsync()
        {
            var members = await _dbContext.TeamMembers.Include(t => t.Status).ToArrayAsync();
            return members;
        }

        public async Task<TeamMember?> GetByIdAsync(Guid id)
        {
            return await _dbContext.TeamMembers
                .Include(t => t.Status)
                .SingleOrDefaultAsync(t => t.Id == id);
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
            if (existingMember == null)
                return false;
            _dbContext.TeamMembers.Remove(existingMember);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
