namespace Core.DTOs
{
    public class UserCreateDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }

        public int? BankId { get; set; }
        public int? RegionId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
