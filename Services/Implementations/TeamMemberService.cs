using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly ITeamMemberRepository _repository;
        private readonly IMapper _mapper;

        public TeamMemberService(ITeamMemberRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TeamMemberDto>> GetAllAsync()
        {
            var members = await _repository.GetAllAsync();
            return _mapper.Map<List<TeamMemberDto>>(members);
        }

        public async Task<TeamMemberDto?> GetByIdAsync(Guid id)
        {
            var member = await _repository.GetByIdAsync(id);
            Console.WriteLine(member.Status.ToString());
            if (member == null)
                return null;
            return _mapper.Map<TeamMemberDto>(member);
        }

        public async Task<TeamMemberDto?> AddAsync(CreateTeamMemberDto teamMemberDto)
        {
            var member = _mapper.Map<TeamMember>(teamMemberDto);
            try
            {
                await _repository.AddAsync(member);
                return _mapper.Map<TeamMemberDto>(member);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<TeamMemberDto?> UpdateAsync(TeamMemberDto teamMemberDto)
        {
            var member = _mapper.Map<TeamMember>(teamMemberDto);
            var result = await _repository.UpdateAsync(member);
            if (result == null)
                return null;
            return _mapper.Map<TeamMemberDto>(member);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
