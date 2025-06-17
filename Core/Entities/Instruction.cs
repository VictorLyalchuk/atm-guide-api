using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Instruction
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "HtmlContent is required")]
        public string HtmlContent { get; set; } = string.Empty;

        public DateTime Time { get; set; }

        // Навігація до Категорії
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        // Навігація до ATM
        public int? ATMId { get; set; }
        public ATM? ATM { get; set; }
    }
}
