using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Helpers;
using Domain.QueryStrings;
using Domain.Repositories;
using Infrastructure.UnitOfWork;
using Services.Abstractions;
using Services.Converters;
using Shared;

namespace Services.Implementations
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeamMemberService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TeamMemberResponse>> GetAllAsync(QueryStringParameters parameters)
        {
            var members = await _unitOfWork.TeamMemberRepository.GetAllAsync(parameters);
            var mapped = _mapper.Map<PaginatedList<TeamMemberResponse>>(members);
            PaginatedListConverter<TeamMember, TeamMemberResponse>.Convert(members, mapped);
            return mapped;
        }

        public async Task<TeamMemberResponse?> GetByIdAsync(Guid id)
        {
            var member = await _unitOfWork.TeamMemberRepository.GetByIdAsync(id);
            if (member is null)
            {
                throw new TeamMemberNotFoundException("Team member with given ID doesnt exist");
            }
            return _mapper.Map<TeamMemberResponse>(member);
        }

        public async Task<IEnumerable<TeamMemberResponse>> GetActive()
        {
            var members = await _unitOfWork.TeamMemberRepository.GetActive();
            return _mapper.Map<List<TeamMemberResponse>>(members);
        }

        public async Task<TeamMemberResponse?> AddAsync(TeamMemberCreateDto teamMemberDto)
        {
            var member = _mapper.Map<TeamMember>(teamMemberDto);
            await _unitOfWork.TeamMemberRepository.AddAsync(member);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TeamMemberResponse>(member);
        }

        public async Task<TeamMemberResponse?> UpdateAsync(TeamMemberUpdateDto teamMemberDto)
        {
            var member = _mapper.Map<TeamMember>(teamMemberDto);
            var existingMember = await _unitOfWork.TeamMemberRepository.GetByIdAsync(member.Id);
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
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TeamMemberResponse>(existingMember);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            bool success = await _unitOfWork.TeamMemberRepository.DeleteAsync(id);
            if (success)
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            throw new TeamMemberNotFoundException("Team member with given ID doesnt exist");
        }


    }
}
