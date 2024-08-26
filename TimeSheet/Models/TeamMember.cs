namespace TimeSheet.Models
{
    public class TeamMember
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required double HoursPerWeek { get; set; }
        public required Role Role { get; set; }
        public Status? Status { get; set; }
    }
}
