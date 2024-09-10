using AutoMapper;
using Domain.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Services.Abstractions;
using Shared;
using System.Text;
using System.Security.Cryptography;
using Infrastructure.UnitOfWork;


namespace Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountService(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            var user = await _unitOfWork.TeamMemberRepository.GetByEmailAsync(loginDto.Email);
            if (user is null)
            {
                throw new InvalidLoginCredentialsException("User with given email doesnt exist");
            }
            if (user.Password is [] && user.PasswordSalt is [])
            {
                await UpdatePassword(user, loginDto.Password);
                return _tokenService.CreateToken(user);
            }
            var hashFn = new HMACSHA512(user.PasswordSalt);
            var passwordHash = hashFn.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != user.Password[i])
                {
                    throw new InvalidLoginCredentialsException("Incorrect password");
                }
            }

            return _tokenService.CreateToken(user);
        }

        private async Task UpdatePassword(TeamMember user, string password)
        {
            var hashFn = new HMACSHA512();
            var passwordHash = hashFn.ComputeHash(Encoding.UTF8.GetBytes(password));

            user.Password = passwordHash;
            user.PasswordSalt = hashFn.Key;

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<RegisterDto> Register(RegisterDto registerDto)
        {
            if (await _unitOfWork.TeamMemberRepository.GetByUsernameAsync(registerDto.Username) is not null)
            {
                throw new UsernameAlreadyTakenException("Username is already taken by another user. Try another username");
            }
            if (await _unitOfWork.TeamMemberRepository.GetByEmailAsync(registerDto.Email) is not null)
            {
                throw new EmailAlreadyExistsException("User with this email is already registered. Try another email");
            }

            var hashFn = new HMACSHA512();
            var passwordHash = hashFn.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));

            var teamMember = new TeamMember
            {
                Name = registerDto.Name,
                Username = registerDto.Username,
                Email = registerDto.Email,
                Password = passwordHash,
                PasswordSalt = hashFn.Key,
                Role = registerDto.Role,
                Status = new Status { StatusName = "Active" }
            };
            await _unitOfWork.TeamMemberRepository.AddAsync(teamMember);
            await _unitOfWork.SaveChangesAsync();
            return registerDto;
        }
    }
}
