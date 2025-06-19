using Core.Entities.DTOs;

namespace Core.DTOs
{
    public class AccountResultDTO
    {
        public bool Success { get; set; }
        public string[] Errors { get; set; } = Array.Empty<string>();
        public UserDTO? User { get; set; }
    }
} 