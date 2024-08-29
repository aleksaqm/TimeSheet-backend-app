using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ActivityCreateDto
    {
        public DateTime Date {  get; set; }
        public Guid ClientId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid ProjectId { get; set; }
        public string? Description { get; set; }
        public double Hours { get; set; }
        public double? Overtime { get; set; }
        public Guid UserId { get; set; }
    }
}
