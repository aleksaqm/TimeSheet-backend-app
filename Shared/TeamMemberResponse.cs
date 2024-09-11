using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class TeamMemberResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public double HoursPerWeek { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    }
}
