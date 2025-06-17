using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string? Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "ContentType is required")]
        [MaxLength(100, ErrorMessage = "ContentType can't be longer than 100 characters")]
        // "Instruction", "ProblemSolution", "ATMErrorCode"
        public string ContentType { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "Description can't be longer than 200 characters")]
        public string Description { get; set; } = string.Empty;

    }
}
