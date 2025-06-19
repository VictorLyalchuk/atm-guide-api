using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // Маршрут для реєстрації
        [HttpPost("Registration")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Registration(UserRegistrationDTO registrationDTO)
        {
            await _accountService.Registration(registrationDTO);
            return Ok();
        }

        // Логін
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO loginDTO)
        {
            var token = await _accountService.Login(loginDTO);
            return Ok(new { token });
        }

        // Оновлення токена
        [HttpPut("refresh-token")]
        public async Task<IActionResult> RefreshToken(string oldToken)
        {
            var newToken = await _accountService.RefreshTokenAsync(oldToken);
            return Ok(new { newToken });
        }

        // Редагування користувача
        [HttpPost("Edit")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditUser(UserEditDTO userEditDTO)
        {
            await _accountService.EditUserAsync(userEditDTO);
            return Ok();
        }

        // Отримання користувачів по сторінках
        [HttpGet("UsersByPage/{page}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetUsersByPage(int page)
        {
            var users = await _accountService.UsersyByPageAsync(page);
            return Ok(users);
        }

        // Отримання користувача по ID
        [HttpGet("GetUserById/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _accountService.GetUserByIdAsync(id);
            return Ok(user);
        }

        // Видалення користувача по ID
        [HttpDelete("DeleteUserByID/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUserByID(string id)
        {
            await _accountService.DeleteUserByIDAsync(id);
            return NoContent();
        }
    }
}
