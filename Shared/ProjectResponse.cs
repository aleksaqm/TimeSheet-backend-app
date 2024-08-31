namespace Shared
{
    public class ProjectResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Customer { get; set; }
        public required string Lead { get; set; }
        public required string Status { get; set; }
    }
}
