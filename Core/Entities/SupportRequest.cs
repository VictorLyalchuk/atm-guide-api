using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class SupportRequest
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title can't be longer than 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Message is required")]
        [MaxLength(300, ErrorMessage = "Message can't be longer than 300 characters")]
        public string Message { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public DateTime Time { get; set; }

        // Навігація до ATM Model
        public ATM? ATM { get; set; }
        public int? ATMId { get; set; }

        // Навігація до User
        public User? User { get; set; }
        public string UserId { get; set; } = string.Empty;

        // Зв’язок з категорією проблем
        public int? RequestReasonId { get; set; }
        public RequestReason? RequestReason { get; set; }
    }
}
