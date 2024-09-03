using AutoMapper;
using Domain.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly ITeamMemberRepository _repository;
        private readonly IMapper _mapper;

        public AccountService(ITeamMemberRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<RegisterDto> Register(RegisterDto registerDto)
        {
            if (await _repository.GetByUsernameAsync(registerDto.Username) is not null)
            {
                throw new UsernameAlreadyTakenException("Username is already taken by another user. Try another username");
            }
            if (await _repository.GetByEmailAsync(registerDto.Email) is not null)
            {
                throw new EmailAlreadyExistsException("User with this email is already registered. Try another email");
            }

            var hmac = new HMACSHA512();
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));

            Role role;
            Enum.TryParse<Role>(registerDto.Role, out role);

            var teamMember = new TeamMember
            {
                Name = registerDto.Name,
                Username = registerDto.Username,
                Email = registerDto.Email,
                Password = passwordHash,
                Role = role,
                Status = new Status { StatusName = "Active" }
            };
            await _repository.AddAsync(teamMember);
            return registerDto;
        }
    }
}
