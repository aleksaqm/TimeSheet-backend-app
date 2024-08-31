using Domain.QueryStrings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<ActionResult<List<ClientResponse>>> GetAll([FromQuery] QueryStringParameters parameters)
        {
            var results = await _clientService.GetAllAsync(parameters);
            var metadata = new
            {
                results.TotalCount,
                results.PageSize,
                results.CurrentPage,
                results.HasNext,
                results.HasPrevious
            };

            Response.Headers.Append("Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(results);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<ClientResponse>> GetById(Guid id)
        {
            var result = await _clientService.GetByIdAsync(id);
            if (result is null)
            {
                return BadRequest("Client with given ID doesn't exist");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ClientResponse>> Add(ClientCreateDto clientDto)
        {
            var client = await _clientService.AddAsync(clientDto);
            return client is null ? BadRequest() : Ok(client);
        }

        [HttpPut]
        public async Task<ActionResult<ClientResponse>> Update(ClientUpdateDto clientDto)
        {
            var client = await _clientService.UpdateAsync(clientDto);
            return client is null ? BadRequest("Client with given ID doesn't exist") : Ok(client);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool success = await _clientService.DeleteAsync(id);
            if (success)
            {
                return Ok();
            }
            return BadRequest("Client with given ID doesn't exist");
        }

    }
}
