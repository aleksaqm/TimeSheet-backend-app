using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ActivityDto
    {
        public Guid Id { get; set; }
        public DateTime Date {  get; set; }
        public required string Client {  get; set; }
        public required string Project { get; set; }
        public required string Category { get; set; }
        public string? Description { get; set; }
        public double Hours {  get; set; }
        public double? Overtime { get; set; }
        public required TeamMemberDto User { get; set; }
    }
}
