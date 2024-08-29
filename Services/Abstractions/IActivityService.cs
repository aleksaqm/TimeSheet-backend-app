using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IActivityService
    {
        Task<ActivityDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ActivityDto>> GetAllAsync();
        Task<ActivityDto?> AddAsync(ActivityCreateDto activityDto);
        Task<ActivityDto?> UpdateAsync(ActivityUpdateDto activityDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
