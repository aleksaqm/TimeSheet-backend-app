using System.ComponentModel.DataAnnotations.Schema;

namespace TimeSheet.Models
{
    public class Status
    {
        public Guid Id { get; set; }
        public required string StatusName { get; set; }
    }
}
