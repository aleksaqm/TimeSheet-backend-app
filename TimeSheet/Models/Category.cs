using System.ComponentModel.DataAnnotations;

namespace TimeSheet.Models
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
