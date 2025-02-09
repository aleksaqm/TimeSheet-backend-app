namespace Shared
{
    public class ActivityResponse
    {
        public Guid Id { get; set; }
        public DateTime Date {  get; set; }
        public Guid ClientId {  get; set; }
        public Guid ProjectId { get; set; }
        public Guid CategoryId { get; set; }
        public string? Description { get; set; }
        public double Hours {  get; set; }
        public double? Overtime { get; set; }
        public Guid UserId { get; set; }

    }
}
