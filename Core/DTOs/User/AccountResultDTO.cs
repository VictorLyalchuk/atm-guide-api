namespace Core.DTOs.User
{
    public class AccountResultDTO
    {
        public bool Success { get; set; }
        public string[] Errors { get; set; } = Array.Empty<string>();
        public UserDTO? User { get; set; }
        public string Token { get; set; } = string.Empty;
        public List<UserDTO>? Users { get; set; }
    }
} 