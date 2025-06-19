namespace Core.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }

        public string BankName { get; set; }
        public string RegionName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
