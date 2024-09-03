using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared;

namespace TimeSheet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService service)
        {
            _accountService = service;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegisterDto>> Register(RegisterDto registerDto)
        {
            var result = await _accountService.Register(registerDto);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(LoginDto loginDto)
        {
            var result = await _accountService.Login(loginDto);
            return Ok(result);
        }



    }
}
