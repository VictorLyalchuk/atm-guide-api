using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Bank
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string? Name { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20, ErrorMessage = "Name can't be longer than 20 characters")]
        public string Phone { get; set; } = string.Empty;

        // Колекція користувачів
        public ICollection<User>? Users { get; set; }
    }
}
