using Domain.Entities;
using Domain.Helpers;
using Domain.QueryStrings;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly RepositoryDbContext _dbContext;

        public ClientRepository(RepositoryDbContext context)
        {
            _dbContext = context;
        }

        public async Task<PaginatedList<Client>> GetAllAsync(QueryStringParameters parameters)
        {
            parameters.SearchText ??= string.Empty;
            parameters.FirstLetter ??= string.Empty;
            var allClients = from c in _dbContext.Clients
                                where c.Name.StartsWith(parameters.FirstLetter) && c.Name.Contains(parameters.SearchText)
                                select c;
            var clients =
                PaginatedList<Client>.ToPagedList(allClients, parameters.PageNumber, parameters.PageSize);
            return clients;
        }

        public async Task<Client?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Clients.FindAsync(id);
        }

        public async Task AddAsync(Client client)
        {
            await _dbContext.Clients.AddAsync(client);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existingClient = await _dbContext.Clients.FindAsync(id);
            if (existingClient is null)
            {
                return false;
            }
            _dbContext.Clients.Remove(existingClient);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
