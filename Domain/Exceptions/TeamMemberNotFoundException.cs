using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class TeamMemberNotFoundException : Exception
    {
        public TeamMemberNotFoundException(string? message) : base(message)
        {
        }
    }
}
