using Domain.Entities;

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
