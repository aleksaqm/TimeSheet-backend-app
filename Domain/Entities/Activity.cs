namespace Domain.Entities
{
    public class Activity
    {
        public Guid Id { get; set; }
        public DateTime Date {  get; set; }
        public Guid ClientId { get; set; }
        public Client Client { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public string? Description {  get; set; }
        public double Hours { get; set; }
        public double? Overtime {  get; set; }
        public Guid UserId { get; set; }
        public TeamMember User { get; set; }
    }
}
