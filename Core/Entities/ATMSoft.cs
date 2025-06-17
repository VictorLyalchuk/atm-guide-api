using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class ATMSoft
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Version is required")]
        [MaxLength(20, ErrorMessage = "Version can't be longer than 20 characters")]
        public string Version { get; set; } = string.Empty;

        public DateTime? ReleaseDate { get; set; }

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        // Навігація до банкоматів
        public ICollection<ATM>? ATMs { get; set; }
    }
}
