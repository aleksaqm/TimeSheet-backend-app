using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Services.Abstractions;
using Shared;

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
            if (member is null)
            {
                return null;
            }
            return _mapper.Map<TeamMemberDto>(member);
        }

        public async Task<TeamMemberDto?> AddAsync(TeamMemberCreateDto teamMemberDto)
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
            var existingMember = await _repository.GetByIdAsync(member.Id);
            if (existingMember is null)
            {
                return null;
            }
            existingMember.Name = member.Name;
            existingMember.Username = member.Username;
            existingMember.Email = member.Email;
            existingMember.HoursPerWeek = member.HoursPerWeek;
            existingMember.Status.StatusName = member.Status.StatusName;
            existingMember.Role = member.Role;
            await _repository.UpdateAsync();
            return _mapper.Map<TeamMemberDto>(existingMember);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
