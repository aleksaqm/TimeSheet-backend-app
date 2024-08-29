using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IActivityRepository
    {
        Task<Activity?> GetByIdAsync(Guid id);
        Task<IEnumerable<Activity>> GetAllAsync();
        Task AddAsync(Activity activity);
        Task UpdateAsync();
        Task<bool> DeleteAsync(Guid id);
    }
}
