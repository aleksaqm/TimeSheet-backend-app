using Domain.Entities;
using Domain.Helpers;
using Domain.QueryStrings;

namespace Domain.Repositories
{
    public interface IClientRepository
    {
        Task<Client?> GetByIdAsync(Guid id);
        Task<PaginatedList<Client>> GetAllAsync(QueryStringParameters parameters);
        Task AddAsync(Client client);
        Task<bool> DeleteAsync(Guid id);
    }
}
