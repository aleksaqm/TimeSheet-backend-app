using AutoMapper;
using Domain.Entities;
using Domain.Helpers;
using Domain.QueryStrings;
using Domain.Repositories;
using Domain.Exceptions;
using Services.Abstractions;
using Shared;

namespace Services.Implementations
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;
        private readonly IMapper _mapper;

        public ClientService(IClientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ClientResponse>> GetAllAsync(QueryStringParameters parameters)
        {
            var clients = await _repository.GetAllAsync(parameters);
            var mapped = _mapper.Map<PaginatedList<ClientResponse>>(clients);
            mapped.CurrentPage = clients.CurrentPage;
            mapped.HasNext = clients.HasNext;
            mapped.PageSize = clients.PageSize;
            return mapped;
        }

        public async Task<ClientResponse?> GetByIdAsync(Guid id)
        {
            var client = await _repository.GetByIdAsync(id);
            if (client is null)
            {
                throw new ClientNotFoundException("Client with given ID doesnt exist.");
            }
            return _mapper.Map<ClientResponse>(client);
        }

        public async Task<ClientResponse?> AddAsync(ClientCreateDto clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);
            await _repository.AddAsync(client);
            return _mapper.Map<ClientResponse>(client);
        }

        public async Task<ClientResponse?> UpdateAsync(ClientUpdateDto clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);
            var existingClient = await _repository.GetByIdAsync(client.Id);
            if (existingClient is null)
            {
                throw new ClientNotFoundException("Client with given ID doesnt exist.");
            }
            existingClient.Name = clientDto.Name;
            existingClient.Address = clientDto.Address;
            existingClient.City = clientDto.City;
            existingClient.PostalCode = clientDto.PostalCode;
            existingClient.Country = clientDto.Country;
            await _repository.UpdateAsync();
            return _mapper.Map<ClientResponse>(existingClient);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            bool success = await _repository.DeleteAsync(id);
            if (success)
            {
                return true;
            }
            throw new CategoryNotFoundException("Category with given ID doesnt exist");
        }
    }
}
