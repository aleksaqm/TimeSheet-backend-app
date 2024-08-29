using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;

namespace TimeSheet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService service)
        {
            _clientService = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClientUpdateDto>>> GetAll()
        {
            var results = await _clientService.GetAllAsync();
            return Ok(results);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<ClientUpdateDto>> GetById(Guid id)
        {
            var result = await _clientService.GetByIdAsync(id);
            if (result == null)
                return BadRequest("Client with given ID doesn't exist");
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ClientUpdateDto>> Add(ClientCreateDto clientDto)
        {
            var client = await _clientService.AddAsync(clientDto);
            return client == null ? BadRequest() : Ok(client);
        }

        [HttpPut]
        public async Task<ActionResult<ClientUpdateDto>> Update(ClientUpdateDto clientDto)
        {
            var client = await _clientService.UpdateAsync(clientDto);
            return client == null ? BadRequest("Client with given ID doesn't exist") : Ok(client);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool success = await _clientService.DeleteAsync(id);
            if (success)
                return Ok();
            return BadRequest("Client with given ID doesn't exist");
        }

    }
}
