using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Moq;
using Service.Abstractions;
using Services.Abstractions;
using Services.Implementations;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Test.UnitTests
{
    public class ClientServiceTest
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IClientService _clientService;

        public ClientServiceTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _clientService = new ClientService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetByidAsync_ClientExists_ReturnsClientObject()
        {
            var clientId = Guid.NewGuid();
            var client = new Client
            {
                Id = clientId,
                Name = "Test",
                Address = "Test",
                City = "Test",
                Country = "Test",
                PostalCode = "Test",
            };
            var clientResponse = new ClientResponse { Id = clientId };

            _unitOfWorkMock.Setup(u => u.ClientRepository.GetByIdAsync(clientId)).ReturnsAsync(client);
            _mapperMock.Setup(m => m.Map<ClientResponse>(client)).Returns(clientResponse);

            var result = await _clientService.GetByIdAsync(clientId);

            Assert.NotNull(result);
            Assert.Equal(clientId, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ClientNotFound_ThrowsNotFoundException()
        {
            var clientId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.ClientRepository.GetByIdAsync(clientId)).ReturnsAsync(null as Client);

            await Assert.ThrowsAsync<ClientNotFoundException>(() => _clientService.GetByIdAsync(clientId));
        }

        [Fact]
        public async Task UpdateAsync_ValidData_Success()
        {
            var clientId = Guid.NewGuid();
            var clientDto = new ClientUpdateDto 
            {
                Id = clientId,
                Address = "Updated", 
                City = "Updated", 
                Country = "Updated", 
                PostalCode = "Updated", 
                Name = "Updated"
            };
            var client = new Client
            {
                Id = clientId,
                Address = "Updated",
                City = "Updated",
                Country = "Updated",
                PostalCode = "Updated",
                Name = "Updated"
            };
            var existingClient = new Client
            {
                Id = clientId,
                Address = "Test",
                City = "Test",
                Country = "Test",
                PostalCode = "Test",
                Name = "Test"
            };
            var clientResponse = new ClientResponse
            {
                Id = clientId,
                Address = "Updated",
                City = "Updated",
                Country = "Updated",
                PostalCode = "Updated",
                Name = "Updated"
            };

            _mapperMock.Setup(m => m.Map<Client>(clientDto)).Returns(client);
            _unitOfWorkMock.Setup(u => u.ClientRepository.GetByIdAsync(clientId)).ReturnsAsync(existingClient);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<ClientResponse>(existingClient)).Returns(clientResponse);

            var result = await _clientService.UpdateAsync(clientDto);

            Assert.NotNull(result);
            Assert.Equal(clientDto.Name, result.Name);
            Assert.Equal(clientDto.Address, result.Address);
            Assert.Equal(clientDto.Country, result.Country);
            Assert.Equal(clientDto.City, result.City);
            Assert.Equal(clientDto.PostalCode, result.PostalCode);
            Assert.Equal(clientId, result.Id);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_InvalidData_ThrowsNotFoundException()
        {
            var clientId = Guid.NewGuid();
            var clientDto = new ClientUpdateDto
            {
                Id = clientId,
                Address = "Updated",
                City = "Updated",
                Country = "Updated",
                PostalCode = "Updated",
                Name = "Updated"
            };
            var client = new Client
            {
                Id = clientId,
                Address = "Updated",
                City = "Updated",
                Country = "Updated",
                PostalCode = "Updated",
                Name = "Updated"
            };

            _mapperMock.Setup(m => m.Map<Client>(clientDto)).Returns(client);
            _unitOfWorkMock.Setup(u => u.ClientRepository.GetByIdAsync(clientId)).ReturnsAsync(null as Client);

            await Assert.ThrowsAsync<ClientNotFoundException>(() => _clientService.UpdateAsync(clientDto));
        }

        [Fact]
        public async Task DeleteAsync_ValidId_Success()
        {
            var clientId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.ClientRepository.DeleteAsync(clientId)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            var result = await _clientService.DeleteAsync(clientId);

            Assert.True(result);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_InvalidId_ThrowsNotFoundException()
        {
            var clientId = Guid.NewGuid();

            _unitOfWorkMock.Setup(u => u.ClientRepository.DeleteAsync(clientId)).ReturnsAsync(false);
            _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

            await Assert.ThrowsAsync<ClientNotFoundException>(() => _clientService.DeleteAsync(clientId));
        }
    }
}
