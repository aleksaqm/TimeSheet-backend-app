using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ReportResponse
    {
        public required IEnumerable<ReportDto> Reports { get; set; }
        public double ReportTotalHours { get; set; }
    }
}
