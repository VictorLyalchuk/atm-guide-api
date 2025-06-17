using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class ATMModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name max length is 50 characters")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50, ErrorMessage = "Manufacturer max length is 50 characters")]
        public string Manufacturer { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "Description max length is 200 characters")]
        public string Description { get; set; } = string.Empty;

        public DateTime? ReleaseDate { get; set; }

        // колекція АТМ, що використовують цю модель
        public ICollection<ATM>? ATMs { get; set; }
    }
}
