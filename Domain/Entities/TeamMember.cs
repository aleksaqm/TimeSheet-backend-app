namespace Domain.Entities
{
    public class TeamMember
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public byte[]? Password { get; set; }
        public double HoursPerWeek { get; set; }
        public Role Role { get; set; }
        public Guid StatusId { get; set; }
        public Status Status { get; set; }
    }
}
