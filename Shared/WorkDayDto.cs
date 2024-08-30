using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class WorkDayDto
    {
        public required DateTime Date {  get; set; }
        public required double Hours { get; set; }
    }
}
