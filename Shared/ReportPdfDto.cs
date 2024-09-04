using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ReportPdfDto
    {
        public string TeamMember { get; set; }
        public string Client { get; set; }
        public string Project { get; set; }
        public string Category { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public double ReportTotalHours { get; set; }
        public required IEnumerable<ReportDto> Reports { get; set; }
    }
}
