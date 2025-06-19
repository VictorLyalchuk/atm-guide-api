﻿namespace Core.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public string BankName { get; set; } = string.Empty;
        public string RegionName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
    }
}
