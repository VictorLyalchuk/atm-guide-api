namespace Core.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? RegionId { get; set; }
        public int? BankId { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }
} 