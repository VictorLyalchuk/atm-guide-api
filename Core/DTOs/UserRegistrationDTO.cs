namespace Core.DTOs
{
    public class UserRegistrationDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? RegionId { get; set; }
        public int? BankId { get; set; }
    }
} 