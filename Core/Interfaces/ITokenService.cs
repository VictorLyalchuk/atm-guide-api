using Core.DTOs.User;
using Core.Entities;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateToken(User user, string role);
        Task<AccountResultDTO> RefreshTokenAsync(string oldToken);
    }
}
