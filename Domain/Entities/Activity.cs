using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Activity
    {
        public Guid Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public required DateTime Date {  get; set; }
        public required Client Client { get; set; }
        public required Category Category { get; set; }
        public required Project Project { get; set; }
        public string? Description {  get; set; }
        public required double Hours { get; set; }
        public double? Overtime {  get; set; }
        public required TeamMember User { get; set; }
    }
}
