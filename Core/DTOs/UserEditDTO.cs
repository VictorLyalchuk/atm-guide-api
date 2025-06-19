namespace Core.DTOs
{
    public class UserEditDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ImagePath { get; set; }

        public string Role { get; set; }

        public int? BankId { get; set; }
        public int? RegionId { get; set; }
        public string PhoneNumber { get; set; }

    }
}
