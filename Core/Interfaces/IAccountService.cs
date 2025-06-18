using System.Threading.Tasks;
using Core.DTOs;

namespace Core.Interfaces
{
    public interface IAccountService
    {
        Task<AccountResultDTO> RegisterAsync(UserRegistrationDTO registrationDto);
        Task<AccountResultDTO> LoginAsync(UserLoginDTO loginDto);
        Task<AccountResultDTO> EditProfileAsync(UserEditDTO editDto, string userId);
        Task<AccountResultDTO> GetProfileAsync(string userId);
    }
} 