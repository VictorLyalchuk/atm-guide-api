using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class ProblemSolution
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Symptom is required")]
        [MaxLength(200, ErrorMessage = "Symptom can't be longer than 200 characters")]
        public string Symptom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Solution is required")]
        [MaxLength(200, ErrorMessage = "Solution can't be longer than 200 characters")]
        public string Solution { get; set; } = string.Empty;

        public DateTime Time { get; set; }

        // Навігація до Категорії
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
