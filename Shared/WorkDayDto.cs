using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class WorkDayDto
    {
        public required DateTime date {  get; set; }
        public required double totalHours { get; set; }
        public required List<ActivityResponse> activities { get; set; }
    }
}
