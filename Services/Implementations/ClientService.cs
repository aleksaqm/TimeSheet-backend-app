using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
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

        public async Task<IEnumerable<ClientUpdateDto>> GetAllAsync()
        {
            var clients = await _repository.GetAllAsync();
            return _mapper.Map<List<ClientUpdateDto>>(clients);
        }

        public async Task<ClientUpdateDto?> GetByIdAsync(Guid id)
        {
            var client = await _repository.GetByIdAsync(id);
            if (client is null)
            {
                return null;
            }
            return _mapper.Map<ClientUpdateDto>(client);
        }

        public async Task<ClientUpdateDto?> AddAsync(ClientCreateDto clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);
            try
            {
                await _repository.AddAsync(client);
                return _mapper.Map<ClientUpdateDto>(client);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ClientUpdateDto?> UpdateAsync(ClientUpdateDto clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);
            var existingClient = await _repository.GetByIdAsync(client.Id);
            if (existingClient is null)
            {
                return null;
            }
            existingClient.Name = clientDto.Name;
            existingClient.Address = clientDto.Address;
            existingClient.City = clientDto.City;
            existingClient.PostalCode = clientDto.PostalCode;
            existingClient.Country = clientDto.Country;
            await _repository.UpdateAsync();
            return _mapper.Map<ClientUpdateDto>(existingClient);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
