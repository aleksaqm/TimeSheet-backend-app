using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ReportPdfDto
    {
        public Guid? TeamMember { get; set; }
        public Guid? Client { get; set; }
        public Guid? Project { get; set; }
        public Guid? Category { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public double ReportTotalHours { get; set; }
        public required IEnumerable<ReportDto> Reports { get; set; }
    }
}
