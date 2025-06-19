using Core.Entities;
using Core.Entities.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDTO loginDTO)
        {
            var result = await _accountService.LoginAsync(loginDTO);

            if (!result.Success)
                return BadRequest(new { errors = result.Errors });

            return Ok(new { token = result.Token });
        }

        [HttpPost("CreateUser")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateUserAsync(UserCreateDTO userCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.CreateUserAsync(userCreateDTO);
            if (!result.Success)
                return BadRequest(new { errors = result.Errors });
            return Ok();
        }

        [HttpPost("EditUser")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditUserAsync(UserEditDTO userEditDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.EditUserAsync(userEditDTO);
            if (!result.Success)
                return BadRequest(new { errors = result.Errors });
            return Ok();
        }

        [HttpDelete("DeleteUserByID/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteUserByIDAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.DeleteUserByIDAsync(id);
            if (!result.Success)
                return BadRequest(new { errors = result.Errors });
            return Ok();
        }

        [HttpGet("UsersQuantity")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UsersQuantity()
        {
            var quantity = await _accountService.UsersQuantity();
            return Ok(quantity);
        }

        [HttpGet("GetUserById/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            var user = await _accountService.GetUserByIdAsync(id);
            if (user == null)
            {
                return BadRequest(new { errors = user!.Errors });
            }
            return Ok(user.User);
        }

        [HttpGet("UsersByPage/{page}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UsersByPage(int page)
        {
            var users = await _accountService.UsersByPageAsync(page);
            if (users == null)
            {
                return BadRequest(new { errors = users!.Errors });
            }
            return Ok(users);
        }
    }
}
