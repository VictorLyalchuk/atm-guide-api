using Core.DTOs;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public TokenService(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<AccountResultDTO> RefreshTokenAsync(string oldToken)
        {
            var errors = new List<string>();
            var principal = GetPrincipalFromToken(oldToken);
            if (principal == null)
            {
                errors.Add("Token not found");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }

            // Отримання ідентифікатора користувача з старого токену
            var userId = principal.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            if (userId == null)
            {
                errors.Add("User not found");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }

            // Знаходження користувача за ідентифікатором
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                errors.Add("User not found");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }

            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (role == null)
            {
                errors.Add("User role not found");
                return new AccountResultDTO { Success = false, Errors = errors.ToArray() };
            }

            // Генерація нового токену
            var newToken = await GenerateToken(user, role);

            //return newToken;
            return new AccountResultDTO { Success = true, Token = newToken };
        }
        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false
                };
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }
        public async Task <string> GenerateToken(User user, string role)
        {
            var claimsList = new List<Claim>()
            {
                new Claim("Email", user.Email),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("Role", role!),

                new Claim("Id", user.Id),

                //new Claim("BankName", BankName),
                //new Claim("RegionName", RegionName),
            };
            if (!string.IsNullOrEmpty(user.ImagePath))
            {
                claimsList.Add(new Claim("ImagePath", user.ImagePath));
            }

            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                claimsList.Add(new Claim("PhoneNumber", user.PhoneNumber));
            }
            var jwtOptions = _configuration.GetSection("Jwt").Get<JwtOptions>();
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions!.Key));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claimsList,
                expires: DateTime.Now.AddMinutes(jwtOptions.LifeTime),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
