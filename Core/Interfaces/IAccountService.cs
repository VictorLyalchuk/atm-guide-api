using Core.DTOs;
using Core.Entities.DTOs;

namespace Core.Interfaces
{
    public interface IAccountService
    {
        Task<AccountResultDTO> LoginAsync(UserLoginDTO loginDTO);
        Task<AccountResultDTO> CreateUserAsync(UserCreateDTO userCreateDTO);
        Task<AccountResultDTO> EditUserAsync(UserEditDTO userEditDTO);
        Task<AccountResultDTO> DeleteUserByIDAsync(string userId);
        Task<AccountResultDTO> GetUserByIdAsync(string userId);
        Task<AccountResultDTO> UsersByPageAsync(int page);
        Task<int> UsersQuantity();


    }
}
