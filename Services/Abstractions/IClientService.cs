using Shared;

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
