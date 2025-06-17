using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class CompanyContact
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "CompanyName is required")]
        [MaxLength(100, ErrorMessage = "CompanyName can't be longer than 100 characters")]
        public string CompanyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Contact Name is required")]
        [MaxLength(100, ErrorMessage = "Contact Name can't be longer than 100 characters")]
        public string ContactName { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Phone Number must be a valid phone number")]
        [MaxLength(20, ErrorMessage = "Phone Number can't be longer than 20 characters")]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email must be a valid email address")]
        [MaxLength(100, ErrorMessage = "Email can't be longer than 100 characters")]
        public string Email { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "Address can't be longer than 200 characters")]
        public string Address { get; set; } = string.Empty;

        [MaxLength(1000, ErrorMessage = "Description can't be longer than 1000 characters")]
        public string Description { get; set; } = string.Empty; 
    }
}
