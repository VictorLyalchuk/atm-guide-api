using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Entities.DTOs;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Identity;

namespace Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountService(UserManager<User> userManager, IMapper mapper, ITokenService tokenService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
        }
        public async Task<AccountResultDTO> LoginAsync(UserLoginDTO loginDto)
        {
            var errors = new List<string>();
            if (loginDto == null)
            {
                errors.Add("Login data is required");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }
            if (string.IsNullOrWhiteSpace(loginDto.Email)) errors.Add("Email is required");
            if (string.IsNullOrWhiteSpace(loginDto.Password)) errors.Add("Password is required");
            if (errors.Count > 0) return new AccountResultDTO { Success = false, Errors = errors.ToArray() };

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                errors.Add("User not found");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!passwordValid)
            {
                errors.Add("Invalid password");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            var token = await _tokenService.GenerateToken(user, role!);
            return new AccountResultDTO { Success = true, Token = token };
        }

        public async Task<AccountResultDTO> CreateUserAsync(UserCreateDTO userCreateDTO)
        {
            var errors = new List<string>();
            if (userCreateDTO == null)
            {
                errors.Add("Registration data is required");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }
            if (string.IsNullOrWhiteSpace(userCreateDTO.Email)) errors.Add("Email is required");
            if (string.IsNullOrWhiteSpace(userCreateDTO.Password)) errors.Add("Password is required");
            if (string.IsNullOrWhiteSpace(userCreateDTO.FirstName)) errors.Add("First name is required");
            if (string.IsNullOrWhiteSpace(userCreateDTO.LastName)) errors.Add("Last name is required");
            if (errors.Count > 0) return new AccountResultDTO { Success = false, Errors = errors.ToArray() };

            var existingUser = await _userManager.FindByEmailAsync(userCreateDTO.Email);
            if (existingUser != null)
            {
                errors.Add("Email already exists");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }

            var user = _mapper.Map<User>(userCreateDTO);
            var result = await _userManager.CreateAsync(user, userCreateDTO.Password);

            if (!result.Succeeded)
            {
                errors.AddRange(result.Errors.Select(e => e.Description));
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, userCreateDTO.Role).Wait();
            }

            return new AccountResultDTO { Success = true };
        }

        public async Task<AccountResultDTO> EditUserAsync(UserEditDTO userEditDTO)
        {
            var errors = new List<string>();
            if (userEditDTO == null)
            {
                errors.Add("Edit data is required");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }
            var user = await _userManager.FindByIdAsync(userEditDTO.Id);
            if (user == null)
            {
                errors.Add("User not found");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }

            // Якщо змінився Email - скидаємо підтвердження і оновлюємо Email
            if (!string.IsNullOrWhiteSpace(userEditDTO.Email) && user.Email != userEditDTO.Email)
            {
                user.Email = userEditDTO.Email;
                user.EmailConfirmed = false;
            }

            // Оновлення пароля 
            if (!string.IsNullOrEmpty(userEditDTO.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordChangeResult = await _userManager.ResetPasswordAsync(user, token, userEditDTO.Password);
                if (!passwordChangeResult.Succeeded)
                {
                    errors.AddRange(passwordChangeResult.Errors.Select(e => e.Description));
                    return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
                }
            }


            if (!string.IsNullOrWhiteSpace(userEditDTO.FirstName)) user.FirstName = userEditDTO.FirstName;
            if (!string.IsNullOrWhiteSpace(userEditDTO.LastName)) user.LastName = userEditDTO.LastName;
            if (userEditDTO.RegionId.HasValue) user.RegionId = userEditDTO.RegionId;
            if (userEditDTO.BankId.HasValue) user.BankId = userEditDTO.BankId;
            if (!string.IsNullOrWhiteSpace(userEditDTO.ImagePath)) user.ImagePath = userEditDTO.ImagePath;
            if (!string.IsNullOrWhiteSpace(userEditDTO.PhoneNumber)) user.PhoneNumber = userEditDTO.PhoneNumber;

            // Блокування користувача
            if (userEditDTO.IsBlocked.HasValue)
            {
                user.LockoutEnd = userEditDTO.IsBlocked.Value ? DateTime.UtcNow.AddYears(100) : (DateTime?)null;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                errors.AddRange(result.Errors.Select(e => e.Description));
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }

            // Оновлення ролі (тільки якщо роль змінилась)
            if (!string.IsNullOrWhiteSpace(userEditDTO.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                if (!currentRoles.Contains(userEditDTO.Role))
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    if (!removeResult.Succeeded)
                    {
                        errors.AddRange(removeResult.Errors.Select(e => e.Description));
                        return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
                    }

                    var addResult = await _userManager.AddToRoleAsync(user, userEditDTO.Role);
                    if (!addResult.Succeeded)
                    {
                        errors.AddRange(addResult.Errors.Select(e => e.Description));
                        return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
                    }
                }
            }

            return new AccountResultDTO { Success = true };
        }

        public async Task<AccountResultDTO> GetUserByIdAsync(string userId)
        {
            var errors = new List<string>();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                errors.Add("User not found");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }
            var userDto = _mapper.Map<UserDTO>(user);

            var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (currentRole != null)
                userDto.Role = currentRole;

            bool isUserLockedOut = await _userManager.IsLockedOutAsync(user);
            userDto.IsBlocked = isUserLockedOut;

            return new AccountResultDTO { Success = true, User = userDto };
        }

        public async Task<AccountResultDTO> UsersByPageAsync(int page)
        {
            var errors = new List<string>();

            var usersQuery = _userManager.Users.UsersByPage(page);

            var users = usersQuery.ToList();
            if (users == null || !users.Any())
            {
                errors.Add("Users not found on the requested page.");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }
            var userDtos = new List<UserDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync((User)user);
                var userDto = _mapper.Map<UserDTO>(user);
                userDto.Role = roles.FirstOrDefault()!;
                userDtos.Add(userDto);
            }
            return new AccountResultDTO { Success = true, Users = userDtos };
        }

        public async Task<int> UsersQuantity()
        {
            return _userManager.Users.Count();
        }

        public async Task<AccountResultDTO> DeleteUserByIDAsync(string userId)
        {
            var errors = new List<string>();

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                errors.Add("Users not found on the requested page.");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }

            if (user != null)
            {
                //if (user.ImagePath != null)
                //{
                //    await _filesService.DeleteUserImageAsync(user.ImagePath!);
                //}
                await _userManager.DeleteAsync(user);
            }
            return new AccountResultDTO { Success = true };
        }

    }
}