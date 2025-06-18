using System.Threading.Tasks;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AccountService(UserManager<User> userManager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<AccountResultDTO> RegisterAsync(UserRegistrationDTO registrationDto)
        {
            var errors = new List<string>();
            if (registrationDto == null)
            {
                errors.Add("Registration data is required");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }
            if (string.IsNullOrWhiteSpace(registrationDto.Email)) errors.Add("Email is required");
            if (string.IsNullOrWhiteSpace(registrationDto.Password)) errors.Add("Password is required");
            if (string.IsNullOrWhiteSpace(registrationDto.FirstName)) errors.Add("First name is required");
            if (string.IsNullOrWhiteSpace(registrationDto.LastName)) errors.Add("Last name is required");
            if (errors.Count > 0) return new AccountResultDTO { Success = false, Errors = errors.ToArray() };

            var existingUser = await _userManager.FindByEmailAsync(registrationDto.Email);
            if (existingUser != null)
            {
                errors.Add("Email already exists");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }
            var user = _mapper.Map<User>(registrationDto);
            var result = await _userManager.CreateAsync(user, registrationDto.Password);
            if (!result.Succeeded)
            {
                errors.AddRange(result.Errors.Select(e => e.Description));
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }
            var userDto = _mapper.Map<UserDTO>(user);
            return new AccountResultDTO { Success = true, User = userDto };
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
            var userDto = _mapper.Map<UserDTO>(user);
            return new AccountResultDTO { Success = true, User = userDto };
        }

        public async Task<AccountResultDTO> EditProfileAsync(UserEditDTO editDto, string userId)
        {
            var errors = new List<string>();
            if (editDto == null)
            {
                errors.Add("Edit data is required");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                errors.Add("User not found");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }
            if (!string.IsNullOrWhiteSpace(editDto.FirstName)) user.FirstName = editDto.FirstName;
            if (!string.IsNullOrWhiteSpace(editDto.LastName)) user.LastName = editDto.LastName;
            if (editDto.RegionId.HasValue) user.RegionId = editDto.RegionId;
            if (editDto.BankId.HasValue) user.BankId = editDto.BankId;
            if (!string.IsNullOrWhiteSpace(editDto.ImagePath)) user.ImagePath = editDto.ImagePath;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                errors.AddRange(result.Errors.Select(e => e.Description));
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }
            var userDto = _mapper.Map<UserDTO>(user);
            return new AccountResultDTO { Success = true, User = userDto };
        }

        public async Task<AccountResultDTO> GetProfileAsync(string userId)
        {
            var errors = new List<string>();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                errors.Add("User not found");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }
            var userDto = _mapper.Map<UserDTO>(user);
            return new AccountResultDTO { Success = true, User = userDto };
        }
    }
} 