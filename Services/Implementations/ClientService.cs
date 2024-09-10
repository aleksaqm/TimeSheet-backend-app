using AutoMapper;
using Domain.Entities;
using Domain.Helpers;
using Domain.QueryStrings;
using Domain.Repositories;
using Domain.Exceptions;
using Services.Abstractions;
using Shared;
using Infrastructure.UnitOfWork;
using Services.Converters;

namespace Services.Implementations
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClientService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ClientResponse>> GetAllAsync(QueryStringParameters parameters)
        {
            var clients = await _unitOfWork.ClientRepository.GetAllAsync(parameters);
            var mapped = _mapper.Map<PaginatedList<ClientResponse>>(clients);
            PaginatedListConverter<Client, ClientResponse>.Convert(clients, mapped);
            return mapped;
        }

        public async Task<ClientResponse?> GetByIdAsync(Guid id)
        {
            var client = await _unitOfWork.ClientRepository.GetByIdAsync(id);
            if (client is null)
            {
                throw new ClientNotFoundException("Client with given ID doesnt exist.");
            }
            return _mapper.Map<ClientResponse>(client);
        }

        public async Task<ClientResponse?> AddAsync(ClientCreateDto clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);
            await _unitOfWork.ClientRepository.AddAsync(client);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ClientResponse>(client);
        }

        public async Task<ClientResponse?> UpdateAsync(ClientUpdateDto clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);
            var existingClient = await _unitOfWork.ClientRepository.GetByIdAsync(client.Id);
            if (existingClient is null)
            {
                throw new ClientNotFoundException("Client with given ID doesnt exist.");
            }
            existingClient.Name = clientDto.Name;
            existingClient.Address = clientDto.Address;
            existingClient.City = clientDto.City;
            existingClient.PostalCode = clientDto.PostalCode;
            existingClient.Country = clientDto.Country;
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<ClientResponse>(existingClient);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            bool success = await _unitOfWork.ClientRepository.DeleteAsync(id);
            if (success)
            {
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            throw new CategoryNotFoundException("Category with given ID doesnt exist");
        }
    }
}
