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
            //parameters.SearchText ??= string.Empty;
            //parameters.FirstLetter ??= string.Empty;
            //var allClients = from c in _dbContext.Clients select c;
            //where c.Name.StartsWith(parameters.FirstLetter) && c.Name.Contains(parameters.SearchText)
            var query = _dbContext.Clients.AsQueryable();
            if (parameters.SearchText is not null)
            {
                query = query.Where(c => c.Name.Contains(parameters.SearchText));
            }
            if (parameters.FirstLetter is not null) 
            {
                query = query.Where(c => c.Name.StartsWith(parameters.FirstLetter));
            }

            var clients =
                PaginatedList<Client>.ToPagedList(query, parameters.PageNumber, parameters.PageSize);
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
