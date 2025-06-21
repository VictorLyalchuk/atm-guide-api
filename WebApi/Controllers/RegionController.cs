using Core.DTOs.Region;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : Controller
    {
        private readonly IRegionService _regionService;

        public RegionController(IRegionService regionService)
        {
            _regionService = regionService;
        }
        [HttpGet("GetAllRegion")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _regionService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("RegionByPage/{page}")]
        public async Task<IActionResult> RegionByPageAsync(int page)
        {
            var result = await _regionService.RegionByPageAsync(page);
            return Ok(result);
        }

        [HttpGet("GetRegionByID/{id}")]
        public async Task<IActionResult> GetBankByIDAsync(int id)
        {
            var result = await _regionService.GetRegionByIDAsync(id);
            return Ok(result);
        }

        [HttpPost("CreateRegion")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateRegionDTO createRegionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _regionService.CreateAsync(createRegionDTO);
            return Ok();
        }

        [HttpDelete("DeleteRegionByID/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _regionService.DeleteAsync(id);
            return Ok();
        }

        [HttpPut("EditRegion")]
        public async Task<IActionResult> EditAsync([FromBody] EditRegionDTO editRegionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _regionService.EditAsync(editRegionDTO);
            return Ok();
        }
        [HttpGet("RegionQuantity")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RegionQuantityAsync()
        {
            var quantity = await _regionService.RegionQuantityAsync();
            return Ok(quantity);
        }
    }
}
