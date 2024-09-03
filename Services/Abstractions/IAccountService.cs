using Domain.Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IAccountService
    {
        Task<RegisterDto> Register(RegisterDto registerDto);
        Task<string> Login(LoginDto loginDto);
    }
}
