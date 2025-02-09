namespace Shared
{
    public class ClientCreateDto
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required string PostalCode { get; set; }
        public required string Country { get; set; }
    }
}
