using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class RequestReason
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Reason is required")]
        [MaxLength(100, ErrorMessage = "Reason can't be longer than 100 characters")]
        public string Reason { get; set; } = string.Empty;

        // Колекція запитів підтримки, пов'язаних з цією причиною
        public ICollection<SupportRequest>? SupportRequests { get; set; }
    }
}
