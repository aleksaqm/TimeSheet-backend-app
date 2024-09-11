namespace Shared
{
    public class ProjectResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Customer { get; set; }
        public string Lead { get; set; }
        public string Status { get; set; }
    }
}
