using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;
using Moq;
using NuGet.Frameworks;
using Service.Abstractions;
using Services.Abstractions;
using Services.Implementations;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Test.UnitTests
{
    public class AccountServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly IAccountService _accountService;

        public AccountServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _tokenServiceMock = new Mock<ITokenService>();
            _accountService = new AccountService(_unitOfWorkMock.Object, _tokenServiceMock.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            var hashFn = new HMACSHA512([]);
            var passwordHash = hashFn.ComputeHash(Encoding.UTF8.GetBytes("password"));
            var loginDto = new LoginDto { Email = "perovic@gmail.com", Password = "password" };
            var user = new TeamMember { Password = passwordHash, PasswordSalt = [] };

            _unitOfWorkMock.Setup(u => u.TeamMemberRepository.GetByEmailAsync(loginDto.Email)).ReturnsAsync(user);
            _tokenServiceMock.Setup(t => t.CreateToken(user)).Returns("token");

            var result = await _accountService.Login(loginDto);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Login_InvalidEmail_ThrowsInvalidCredentialsException()
        {
            var loginDto = new LoginDto { Email = "perovic@gmail.com", Password = "password" };
            _unitOfWorkMock.Setup(u => u.TeamMemberRepository.GetByEmailAsync(loginDto.Email)).ReturnsAsync(null as TeamMember);

            await Assert.ThrowsAsync<InvalidLoginCredentialsException>(() => _accountService.Login(loginDto));
        }

        [Fact]
        public async Task Login_InvalidPassword_ThrowsInvalidCredentialsException()
        {
            var hashFn = new HMACSHA512([]);
            var passwordHash = hashFn.ComputeHash(Encoding.UTF8.GetBytes("truePassword"));
            var loginDto = new LoginDto { Email = "perovic@gmail.com", Password = "invalidPassword" };
            var user = new TeamMember { Password = passwordHash, PasswordSalt = [] };

            _unitOfWorkMock.Setup(u => u.TeamMemberRepository.GetByEmailAsync(loginDto.Email)).ReturnsAsync(user);

            await Assert.ThrowsAsync<InvalidLoginCredentialsException>(() => _accountService.Login(loginDto));
        }

        [Fact]
        public async Task Register_ValidData_Success()
        {
            var registerDto = new RegisterDto
            {
                Name = "test",
                Username = "test",
                Email = "test@gmail.com",
                Password = "pass",
                Role = Role.Worker
            };

            _unitOfWorkMock.Setup(u => u.TeamMemberRepository.GetByUsernameAsync(registerDto.Username)).ReturnsAsync(null as TeamMember);
            _unitOfWorkMock.Setup(u => u.TeamMemberRepository.GetByEmailAsync(registerDto.Email)).ReturnsAsync(null as TeamMember);
            _unitOfWorkMock.Setup(u => u.TeamMemberRepository.AddAsync(It.IsAny<TeamMember>())).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _accountService.Register(registerDto);

            Assert.NotNull(result);
            Assert.Equal(registerDto, result);
        }

        [Fact]
        public async Task Register_UsernameExists_ThrowsUsernameTakenException()
        {
            var registerDto = new RegisterDto
            {
                Name = "test",
                Username = "test",
                Email = "test@gmail.com",
                Password = "pass",
                Role = Role.Worker
            };

            _unitOfWorkMock.Setup(u => u.TeamMemberRepository.GetByUsernameAsync(registerDto.Username)).ReturnsAsync(new TeamMember());

            await Assert.ThrowsAsync<UsernameAlreadyTakenException>(() => _accountService.Register(registerDto));
        }

        [Fact]
        public async Task Register_EmailExists_ThrowsEmailTakenException()
        {
            var registerDto = new RegisterDto
            {
                Name = "test",
                Username = "test",
                Email = "test@gmail.com",
                Password = "pass",
                Role = Role.Worker
            };

            _unitOfWorkMock.Setup(u => u.TeamMemberRepository.GetByUsernameAsync(registerDto.Username)).ReturnsAsync(null as TeamMember);
            _unitOfWorkMock.Setup(u => u.TeamMemberRepository.GetByEmailAsync(registerDto.Email)).ReturnsAsync(new TeamMember());

            await Assert.ThrowsAsync<EmailAlreadyExistsException>(() => _accountService.Register(registerDto));
        }

    }
}
