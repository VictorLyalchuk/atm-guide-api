using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTOs;

namespace Core.Interfaces
{
    public interface IAccountService
    {
        Task<string> Login(UserLoginDTO loginDTO);
        Task<string> RefreshTokenAsync(string oldToken);
        Task<List<UserDTO>> UsersyByPageAsync(int page);
        Task<int> UsersQuantity();
        Task CreateUserAsync(UserCreateDTO userCreateDTO);
        Task EditUserAsync(UserEditDTO userEditDTO);
        Task DeleteUserByIDAsync(string id);
        Task<UserDTO> GetUserByIdAsync(string id);
        Task Registration(UserRegistrationDTO registrationDTO);
    }
}
