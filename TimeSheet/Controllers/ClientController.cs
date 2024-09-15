using Domain.QueryStrings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Abstractions;
using Shared;

namespace TimeSheet.Controllers
{
    //[Authorize]
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
                results.PageSize,
                results.CurrentPage,
                results.HasNext,
                results.HasPrevious
            };

            Response.Headers.Append("pagination", JsonConvert.SerializeObject(metadata));
            return Ok(results);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<ClientResponse>> GetById(Guid id)
        {
            var result = await _clientService.GetByIdAsync(id);
            return Ok(result);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<ClientResponse>> Add(ClientCreateDto clientDto)
        {
            var client = await _clientService.AddAsync(clientDto);
            return Ok(client);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<ClientResponse>> Update(ClientUpdateDto clientDto)
        {
            var client = await _clientService.UpdateAsync(clientDto);
            return Ok(client);
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool success = await _clientService.DeleteAsync(id);
            return Ok();
        }

    }
}
