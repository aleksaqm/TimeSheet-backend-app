using System.ComponentModel.DataAnnotations.Schema;

namespace TimeSheet.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required Client Customer {  get; set; }
        [ForeignKey("TeamMember")]
        public required TeamMember Lead { get; set; }
        public required Status Status { get; set; }
    }
}
