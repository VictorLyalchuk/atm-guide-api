namespace Core.DTOs.User
{
    public class UserDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
        public string BankId { get; set; } = string.Empty;
        public string RegionId { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool? IsBlocked { get; set; }
    }
}
