using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UsernameAlreadyTakenException : Exception
    {
        public UsernameAlreadyTakenException(string? message) : base(message)
        {
        }
    }
}
