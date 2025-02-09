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
    public class ActivityServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IActivityService _activityService;

        public ActivityServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _activityService = new ActivityService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ActivtyExists_ReturnsActivityObject()
        {
            var activityId = Guid.NewGuid();
            var activity = new Activity { Id = activityId};
            var activityResponse = new ActivityResponse { Id = activityId };

            _unitOfWorkMock.Setup(u => u.ActivityRepository.GetByIdAsync(activityId)).ReturnsAsync(activity);
            _mapperMock.Setup(m => m.Map<ActivityResponse>(activity)).Returns(activityResponse);

            var result = await _activityService.GetByIdAsync(activityId);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdAsync_CategoryNotFound_ThrowsNotFoundException()
        {
            var activityId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.ActivityRepository.GetByIdAsync(activityId)).ReturnsAsync(null as Activity);

            await Assert.ThrowsAsync<ActivityNotFoundException>(() => _activityService.GetByIdAsync(activityId));
        }

        [Fact]
        public async Task UpdateAsync_ValidData_Success()
        {
            var activityId = Guid.NewGuid();
            var activityDto = new ActivityUpdateDto { Id = activityId};
            var activity = new Activity{ Id = activityId};
            var existingActivity = new Activity { Id = activityId};
            var activityResponse = new ActivityResponse { Id = activityId};

            _mapperMock.Setup(m => m.Map<Activity>(activityDto)).Returns(activity);
            _unitOfWorkMock.Setup(u => u.ActivityRepository.GetByIdAsync(activity.Id)).ReturnsAsync(existingActivity);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<ActivityResponse>(existingActivity)).Returns(activityResponse);

            var result = await _activityService.UpdateAsync(activityDto);

            Assert.NotNull(result);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_InvalidData_ThrowsNotFoundException()
        {
            var activityId = Guid.NewGuid();
            var activityDto = new ActivityUpdateDto { Id = activityId };
            var activity = new Activity { Id = activityId };

            _mapperMock.Setup(m => m.Map<Activity>(activityDto)).Returns(activity);
            _unitOfWorkMock.Setup(u => u.ActivityRepository.GetByIdAsync(activity.Id)).ReturnsAsync(null as Activity);

            await Assert.ThrowsAsync<ActivityNotFoundException>(() => _activityService.UpdateAsync(activityDto));
        }

        [Fact]
        public async Task DeleteAsync_ValidId_Success()
        {
            var activityId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.ActivityRepository.DeleteAsync(activityId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _activityService.DeleteAsync(activityId);

            Assert.True(result);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_InvalidId_ThrowsNotFoundException()
        {
            var activityId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.ActivityRepository.DeleteAsync(activityId)).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<ActivityNotFoundException>(() => _activityService.DeleteAsync(activityId));
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);

        }


    }
}
