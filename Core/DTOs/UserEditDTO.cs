namespace Core.DTOs
{
    public class UserEditDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int? RegionId { get; set; }
        public int? BankId { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }
} 