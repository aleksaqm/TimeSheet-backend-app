namespace Shared
{
    public class ActivityResponse
    {
        public Guid Id { get; set; }
        public DateTime Date {  get; set; }
        public string Client {  get; set; }
        public string Project { get; set; }
        public string Category { get; set; }
        public string? Description { get; set; }
        public double Hours {  get; set; }
        public double? Overtime { get; set; }
        public TeamMemberUpdateDto User { get; set; }
    }
}
