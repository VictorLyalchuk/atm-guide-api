using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class ATMErrorCode
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "ErrorCode is required")]
        [MaxLength(100, ErrorMessage = "ErrorCode can't be longer than 100 characters")]
        public string ErrorCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(400, ErrorMessage = "Description can't be longer than 400 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "RecommendedAction is required")]
        [MaxLength(400, ErrorMessage = "RecommendedAction can't be longer than 400 characters")]
        public string RecommendedAction { get; set; } = string.Empty;

        public DateTime Time { get; set; }

        // Навігація до Категорії
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        // Навігація до ATM
        public int? ATMId { get; set; }
        public ATM? ATM { get; set; }
    }
}
