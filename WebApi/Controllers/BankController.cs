using Core.DTOs.Bank;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : Controller
    {
        private readonly IBankService _bankService;

        public BankController(IBankService bankService)
        {
           _bankService = bankService;
        }
        [HttpGet("GetAllBank")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _bankService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("BankByPage/{page}")]
        public async Task<IActionResult> BankByPageAsync(int page)
        {
            var result = await _bankService.BankByPageAsync(page);
            return Ok(result);
        }

        [HttpGet("GetBankByID/{id}")]
        public async Task<IActionResult> GetBankByIDAsync(int id)
        {
            var result = await _bankService.GetBankByIDAsync(id);
            return Ok(result);
        }

        [HttpPost("CreateBank")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateBankDTO createBankDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _bankService.CreateAsync(createBankDTO);
            return Ok();
        }

        [HttpDelete("DeleteBankByID/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _bankService.DeleteAsync(id);
            return Ok();
        }

        [HttpPut("EditBank")]
        public async Task<IActionResult> EditAsync([FromBody] EditBankDTO editBankDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _bankService.EditAsync(editBankDTO);
            return Ok();
        }
        [HttpGet("BankQuantity")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RegionQuantityAsync()
        {
            var quantity = await _bankService.BankQuantityAsync();
            return Ok(quantity);
        }
    }
}
