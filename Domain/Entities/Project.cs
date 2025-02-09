namespace Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid CustomerId { get; set; }
        public Client Customer {  get; set; }
        public Guid LeadId { get; set; }
        public TeamMember Lead { get; set; }
        public Guid StatusId { get; set; }
        public Status Status { get; set; }
    }
}
