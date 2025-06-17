using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class LogSession
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Event Name is required")]
        [MaxLength(100, ErrorMessage = "Event Name max length is 100 characters")]
        public string EventName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event Type is required")]
        [MaxLength(100, ErrorMessage = "Event Type max length is 100 characters")]
        public string EventType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Device is required")]
        [MaxLength(100, ErrorMessage = "Device max length is 100 characters")]
        public string? Device { get; set; } = string.Empty;

        [Required(ErrorMessage = "App Version is required")]
        [MaxLength(100, ErrorMessage = "App Version max length is 100 characters")]
        public string AppVersion { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "Data max length is 200 characters")]
        public string Data { get; set; } = string.Empty;

        [Required(ErrorMessage = "Time is required")]
        public DateTime Time { get; set; }

        // Навігація до сесії
        public AppSession? AppSession { get; set; }
        public int? AppSessionId { get; set; }

    }
}
