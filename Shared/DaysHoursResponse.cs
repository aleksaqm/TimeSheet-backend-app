using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class DaysHoursResponse
    {
        public required IEnumerable<DayHours> DayHours { get; set; }
        public required double TotalHours { get; set; }
    }


}
