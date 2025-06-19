using Core.Entities.DTOs;
using Core.Interfaces;
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
    }
}
