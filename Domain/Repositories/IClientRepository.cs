using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IClientRepository
    {
        Task<Client?> GetByIdAsync(Guid id);
        Task<IEnumerable<Client>> GetAllAsync();
        Task AddAsync(Client client);
        Task UpdateAsync();
        Task<bool> DeleteAsync(Guid id);
    }
}
