namespace Core.Entities.DTOs
{
    public class UserEditDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public int? BankId { get; set; }
        public int? RegionId { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
