using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class AppSession
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name max length is 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "StartedAt is required")]
        public DateTime StartedAt { get; set; }

        [Required(ErrorMessage = "EndedAt is required")]
        public DateTime EndedAt { get; set; }

        // Список логів сесії
        public ICollection<LogSession>? LogSessions { get; set; }

        // Навігація до ATM Model
        public ATM? ATM { get; set; }
        public int? ATMId { get; set; }

        // Навігація до User
        public User? User { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
