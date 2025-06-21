using AutoMapper;
using Core.DTOs.User;
using Core.Entities;
using Core.Interfaces;
using Core.Specification;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IWebHostEnvironment _env;

        public AccountService(UserManager<User> userManager, IMapper mapper, ITokenService tokenService, IWebHostEnvironment env, IEmailService emailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _tokenService = tokenService;
            _env = env;
            _emailService = emailService;
        }
        public async Task<AccountResultDTO> LoginAsync(UserLoginDTO loginDto)
        {
            var errors = new List<string>();
            
            if (loginDto == null)
            {
                errors.Add("Login data is required");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }
            
            if (loginDto.AuthType == "web")
            {
                if (string.IsNullOrWhiteSpace(loginDto.Email)) errors.Add("Email is required");

            }
            else if (loginDto.AuthType == "mobile")
            {
                if (string.IsNullOrWhiteSpace(loginDto.Email)) errors.Add("Login is required");
            }
            else
            {
                errors.Add("Invalid authentication type");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }
            if (string.IsNullOrWhiteSpace(loginDto.Password)) errors.Add("Password is required");
            if (errors.Count > 0) return new AccountResultDTO { Success = false, Errors = errors.ToArray() };


            var user = await _userManager.FindByNameAsync(loginDto.Email);

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

            if (userCreateDTO.Password == null || userCreateDTO.Password == "")
            {
                userCreateDTO.Password = GeneratePassword();
            }

            if (string.IsNullOrWhiteSpace(userCreateDTO.Login)) errors.Add("Login is required");

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

            var existingUserByLogin = await _userManager.FindByNameAsync(userCreateDTO.Login);

            if (existingUserByLogin != null)
            {
                errors.Add("Login already exists");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }

            var user = _mapper.Map<User>(userCreateDTO);

            user.UserName = userCreateDTO.Login;

            var result = await _userManager.CreateAsync(user, userCreateDTO.Password);

            if (!result.Succeeded)
            {
                errors.AddRange(result.Errors.Select(e => e.Description));
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }

            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, userCreateDTO.Role).Wait();
                await SendLoginAndPassword(userCreateDTO.Email, user.UserName, userCreateDTO.Password);
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

            if (userEditDTO.GenerateNewPassword ?? false)
            {
               
                var newPassword = GeneratePassword();

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordChangeResult = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (!passwordChangeResult.Succeeded)
                {
                    errors.AddRange(passwordChangeResult.Errors.Select(e => e.Description));
                    return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
                }
                else
                    await SendLoginAndPassword(user.Email, user.UserName, newPassword);

            }

            //if (!string.IsNullOrEmpty(userEditDTO.Password))
            //{
            //    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            //    var passwordChangeResult = await _userManager.ResetPasswordAsync(user, token, userEditDTO.Password);
            //    if (!passwordChangeResult.Succeeded)
            //    {
            //        errors.AddRange(passwordChangeResult.Errors.Select(e => e.Description));
            //        return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            //    }
            //}

            if (!string.IsNullOrWhiteSpace(userEditDTO.Login) && user.UserName != userEditDTO.Login)
            {
                var userWithSameLogin = await _userManager.FindByNameAsync(userEditDTO.Login);
                if (userWithSameLogin == null || userWithSameLogin.Id == user.Id)
                {
                    user.UserName = userEditDTO.Login;
                }
                else
                {
                    errors.Add("Цей логін вже використовується.");
                    return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
                }
            }

            if (!string.IsNullOrWhiteSpace(userEditDTO.FirstName)) user.FirstName = userEditDTO.FirstName;
            if (!string.IsNullOrWhiteSpace(userEditDTO.LastName)) user.LastName = userEditDTO.LastName;
            if (userEditDTO.RegionId.HasValue) user.RegionId = userEditDTO.RegionId;
            if (userEditDTO.BankId.HasValue) user.BankId = userEditDTO.BankId;
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
        private static string GeneratePassword(int length = 12)
        {
            const string letters1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string letters2 = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string allChars = letters1 + letters2 + digits;

            var random = new Random();
            var password = new char[length];


            password[0] = letters1[random.Next(letters1.Length)];
            password[1] = letters2[random.Next(letters2.Length)];
            password[2] = digits[random.Next(digits.Length)];

            for (int a = 3; a < length; a++)
            {
                password[a] = allChars[random.Next(allChars.Length)];
            }

            return new string(password.OrderBy(x => random.Next()).ToArray());
        }
        private async Task SendLoginAndPassword(string email, string login, string password)
        {
            string templateFilePath = Path.Combine(_env.WebRootPath, "email", "YourLogin.html");
            string emailBody = File.ReadAllText(templateFilePath);

            emailBody = emailBody.Replace("{login}", login);
            emailBody = emailBody.Replace("{password}", password);

            await _emailService.SendEmailAsync(email!, "Ваш логін та пароль", emailBody);
        }

    }
}