using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.UnitOfWork;
using Moq;
using Service.Abstractions;
using Services.Abstractions;
using Services.Implementations;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Test.UnitTests
{
    public class TeamMemberServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ITeamMemberService _teamMemberService;

        public TeamMemberServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _teamMemberService = new TeamMemberService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_TeamMemberExists_ReturnsTeamMemberObject()
        {
            var memberId = Guid.NewGuid();
            var member = new TeamMember { Id = memberId};
            var teamMemberResponse = new TeamMemberResponse{ Id = memberId};

            _unitOfWorkMock.Setup(u => u.TeamMemberRepository.GetByIdAsync(memberId)).ReturnsAsync(member);
            _mapperMock.Setup(m => m.Map<TeamMemberResponse>(member)).Returns(teamMemberResponse);

            var result = await _teamMemberService.GetByIdAsync(memberId);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdAsync_TeamMemberNotFound_ThrowsNotFoundException()
        {
            var memberId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.TeamMemberRepository.GetByIdAsync(memberId)).ReturnsAsync(null as TeamMember);

            await Assert.ThrowsAsync<TeamMemberNotFoundException>(() => _teamMemberService.GetByIdAsync(memberId));
        }

        [Fact]
        public async Task UpdateAsync_ValidData_Success()
        {
            var memberId = Guid.NewGuid();
            var memberDto = new TeamMemberUpdateDto
            {
                Id = memberId,
                Email = "Test",
                Name = "Test",
                HoursPerWeek = 1,
                Role = "Worker",
                Username = "Test",
                Status = "Active",
            };
            var member = new TeamMember { Id = memberId, Status = new Status { StatusName = "Active" } };
            var existingMember = new TeamMember { Id = memberId, Status = new Status { StatusName = "Active" } };
            var memberResponse = new TeamMemberResponse{ Id = memberId};

            _mapperMock.Setup(m => m.Map<TeamMember>(memberDto)).Returns(member);
            _unitOfWorkMock.Setup(u => u.TeamMemberRepository.GetByIdAsync(member.Id)).ReturnsAsync(existingMember);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<TeamMemberResponse>(existingMember)).Returns(memberResponse);

            var result = await _teamMemberService.UpdateAsync(memberDto);

            Assert.NotNull(result);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_InvalidData_ThrowsNotFoundException()
        {
            var memberId = Guid.NewGuid();
            var memberDto = new TeamMemberUpdateDto
            {
                Id = memberId,
                Email = "Test",
                Name = "Test",
                HoursPerWeek = 1,
                Role = "Worker",
                Username = "Test",
                Status = "Active",
            };
            var member = new TeamMember { Id = memberId};

            _mapperMock.Setup(m => m.Map<TeamMember>(memberDto)).Returns(member);
            _unitOfWorkMock.Setup(u => u.TeamMemberRepository.GetByIdAsync(member.Id)).ReturnsAsync(null as TeamMember);

            await Assert.ThrowsAsync<TeamMemberNotFoundException>(() => _teamMemberService.UpdateAsync(memberDto));
        }

        [Fact]
        public async Task DeleteAsync_ValidId_Success()
        {
            var memberId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.TeamMemberRepository.DeleteAsync(memberId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _teamMemberService.DeleteAsync(memberId);

            Assert.True(result);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_InvalidId_ThrowsNotFoundException()
        {
            var memberId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.TeamMemberRepository.DeleteAsync(memberId)).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<TeamMemberNotFoundException>(() => _teamMemberService.DeleteAsync(memberId));
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }


    }
}
