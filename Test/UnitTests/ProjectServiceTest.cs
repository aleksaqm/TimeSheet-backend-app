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
    public class ProjectServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IProjectService _projectService;

        public ProjectServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _projectService = new ProjectService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ProjectExists_ReturnsProjectObject()
        {
            var projectId = Guid.NewGuid();
            var project = new Project { Id = projectId};
            var projectResponse = new ProjectResponse { Id = projectId };

            _unitOfWorkMock.Setup(u => u.ProjectRepository.GetByIdAsync(projectId)).ReturnsAsync(project);
            _mapperMock.Setup(m => m.Map<ProjectResponse>(project)).Returns(projectResponse);

            var result = await _projectService.GetByIdAsync(projectId);

            Assert.NotNull(result);
            Assert.Equal(projectId, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ClientNotFound_ThrowsNotFoundException()
        {
            var projectId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.ProjectRepository.GetByIdAsync(projectId)).ReturnsAsync(null as Project);

            await Assert.ThrowsAsync<ProjectNotFoundException>(() => _projectService.GetByIdAsync(projectId));
        }

        [Fact]
        public async Task UpdateAsync_ValidData_Success()
        {
            var projectId = Guid.NewGuid();
            var projectDto = new ProjectUpdateDto
            {
                Id = projectId,
                CustomerId = Guid.NewGuid(),
                LeadId = Guid.NewGuid(),
                Name = "Update",
                Status = "Active"
            };
            var project = new Project { Id = projectId, Name = "Update", Status = new Status { StatusName = "Active" } };
            var existingProject = new Project { Id = projectId, Name = "Stari", Status = new Status { StatusName = "Inactive" } };
            var projectResponse = new ProjectResponse{Id = projectId, Name = "Update", Status = "Active" };

            _mapperMock.Setup(m => m.Map<Project>(projectDto)).Returns(project);
            _unitOfWorkMock.Setup(u => u.ProjectRepository.GetByIdAsync(projectId)).ReturnsAsync(existingProject);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<ProjectResponse>(existingProject)).Returns(projectResponse);

            var result = await _projectService.UpdateAsync(projectDto);

            Assert.NotNull(result);
            Assert.Equal("Update", result.Name);
            Assert.Equal("Active", result.Status);
            Assert.Equal(projectId, result.Id);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_InvalidData_ThrowsNotFoundException()
        {
            var projectId = Guid.NewGuid();
            var projectDto = new ProjectUpdateDto
            {
                Id = projectId,
                CustomerId = Guid.NewGuid(),
                LeadId = Guid.NewGuid(),
                Name = "Update",
                Status = "Active"
            };
            var project = new Project { Id = projectId, Name = "Update", Status = new Status { StatusName = "Active" } };

            _mapperMock.Setup(m => m.Map<Project>(projectDto)).Returns(project);
            _unitOfWorkMock.Setup(u => u.ProjectRepository.GetByIdAsync(project.Id)).ReturnsAsync(null as Project);

            await Assert.ThrowsAsync<ProjectNotFoundException>(() => _projectService.UpdateAsync(projectDto));
        }

        [Fact]
        public async Task DeleteAsync_ValidId_Success()
        {
            var projectId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.ProjectRepository.DeleteAsync(projectId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _projectService.DeleteAsync(projectId);

            Assert.True(result);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_InvalidId_ThrowsNotFoundException()
        {
            var projectId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.ProjectRepository.DeleteAsync(projectId)).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<ProjectNotFoundException>(() => _projectService.DeleteAsync(projectId));
        }




    }
}
