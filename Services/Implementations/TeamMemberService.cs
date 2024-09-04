using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Helpers;
using Domain.QueryStrings;
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

        public async Task<PaginatedList<TeamMemberResponse>> GetAllAsync(QueryStringParameters parameters)
        {
            var members = await _repository.GetAllAsync(parameters);
            var mapped = _mapper.Map<PaginatedList<TeamMemberResponse>>(members);
            mapped.CurrentPage = members.CurrentPage;
            mapped.TotalCount = members.TotalCount;
            mapped.PageSize = members.PageSize;
            mapped.TotalPages = members.TotalPages;
            return mapped;
        }

        public async Task<TeamMemberResponse?> GetByIdAsync(Guid id)
        {
            var member = await _repository.GetByIdAsync(id);
            if (member is null)
            {
                throw new TeamMemberNotFoundException("Team member with given ID doesnt exist");
            }
            return _mapper.Map<TeamMemberResponse>(member);
        }

        public async Task<IEnumerable<TeamMemberResponse>> GetActive()
        {
            var members = await _repository.GetActive();
            return _mapper.Map<List<TeamMemberResponse>>(members);
        }

        public async Task<TeamMemberResponse?> AddAsync(TeamMemberCreateDto teamMemberDto)
        {
            var member = _mapper.Map<TeamMember>(teamMemberDto);
            await _repository.AddAsync(member);
            return _mapper.Map<TeamMemberResponse>(member);
        }

        public async Task<TeamMemberResponse?> UpdateAsync(TeamMemberUpdateDto teamMemberDto)
        {
            var member = _mapper.Map<TeamMember>(teamMemberDto);
            var existingMember = await _repository.GetByIdAsync(member.Id);
            if (existingMember is null)
            {
                throw new TeamMemberNotFoundException("Team member with given ID doesnt exist");
            }
            existingMember.Name = member.Name;
            existingMember.Username = member.Username;
            existingMember.Email = member.Email;
            existingMember.HoursPerWeek = member.HoursPerWeek;
            existingMember.Status.StatusName = member.Status.StatusName;
            existingMember.Role = member.Role;
            await _repository.UpdateAsync();
            return _mapper.Map<TeamMemberResponse>(existingMember);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            bool success = await _repository.DeleteAsync(id);
            if (success)
            {
                return true;
            }
            throw new TeamMemberNotFoundException("Team member with given ID doesnt exist");
        }


    }
}
