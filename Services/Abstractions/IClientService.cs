using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IClientService
    {
        Task<ClientUpdateDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<ClientUpdateDto>> GetAllAsync();
        Task<ClientUpdateDto?> AddAsync(ClientCreateDto clientDto);
        Task<ClientUpdateDto?> UpdateAsync(ClientUpdateDto clientDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
