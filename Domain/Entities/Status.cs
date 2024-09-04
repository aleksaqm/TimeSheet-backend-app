namespace Domain.Entities
{
    public class Status
    {
        public Guid Id { get; set; }
        public required string StatusName { get; set; }
    }
}
