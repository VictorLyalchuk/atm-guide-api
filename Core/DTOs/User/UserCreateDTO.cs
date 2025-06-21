namespace Core.DTOs.User
{
    public class UserCreateDTO
    {
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int? BankId { get; set; }
        public int? RegionId { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
