using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _repository;
        private readonly IMapper _mapper;

        public ActivityService(IActivityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ActivityDto>> GetAllAsync()
        {
            var activities = await _repository.GetAllAsync();
            return _mapper.Map<List<ActivityDto>>(activities);
        }

        public async Task<ActivityDto?> GetByIdAsync(Guid id)
        {
            var activity = await _repository.GetByIdAsync(id);
            if (activity is null)
            {
                return null;
            }
            return _mapper.Map<ActivityDto>(activity);
        }

        public async Task<ActivityDto?> AddAsync(ActivityCreateDto activityDto)
        {
            var activity = _mapper.Map<Activity>(activityDto);
            try
            {
                await _repository.AddAsync(activity);
                var fullActivity = await _repository.GetByIdAsync(activity.Id);
                return _mapper.Map<ActivityDto>(fullActivity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ActivityDto?> UpdateAsync(ActivityUpdateDto activityDto)
        {
            var activity = _mapper.Map<Activity>(activityDto);
            var existingActivity = await _repository.GetByIdAsync(activity.Id);
            if (existingActivity is null)
            {
                return null;
            }
            existingActivity.Date = activity.Date;
            existingActivity.CategoryId = activity.CategoryId;
            existingActivity.ClientId = activity.ClientId;
            existingActivity.ProjectId = activity.ProjectId;
            existingActivity.UserId = activity.UserId;
            existingActivity.Description = activity.Description;
            existingActivity.Hours = activity.Hours;
            existingActivity.Overtime = activity.Overtime;
            await _repository.UpdateAsync();
            return _mapper.Map<ActivityDto>(existingActivity);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
             return await _repository.DeleteAsync(id);
        }
    }
}
