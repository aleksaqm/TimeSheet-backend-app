namespace Shared
{
    public class ProjectCreateDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required Guid CustomerId { get; set; }
        public required Guid LeadId {  get; set; }
    }
}
