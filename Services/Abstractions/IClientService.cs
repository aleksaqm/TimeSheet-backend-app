using Domain.Helpers;
using Domain.QueryStrings;
using Shared;

namespace Services.Abstractions
{
    public interface IClientService
    {
        Task<ClientResponse?> GetByIdAsync(Guid id);
        Task<PaginatedList<ClientResponse>> GetAllAsync(QueryStringParameters parameters);
        Task<ClientResponse?> AddAsync(ClientCreateDto clientDto);
        Task<ClientResponse?> UpdateAsync(ClientUpdateDto clientDto);
        Task<bool> DeleteAsync(Guid id);
    }
}
