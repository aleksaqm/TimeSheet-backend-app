using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
