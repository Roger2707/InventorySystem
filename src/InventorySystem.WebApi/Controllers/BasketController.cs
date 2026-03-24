using InventorySystem.Application.DTOs.Baskets;
using InventorySystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<BasketDto>>> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _basketService.GetAllAsync(cancellationToken);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<BasketDto>> GetById(int id, CancellationToken cancellationToken = default)
        {
            var result = await _basketService.GetByIdAsync(id, cancellationToken);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BasketDto>> Create([FromBody] CreateBasketDto createDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _basketService.CreateAsync(createDto, cancellationToken);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<BasketDto>> Update(int id, [FromBody] UpdateBasketDto updateDto, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _basketService.UpdateAsync(id, updateDto, cancellationToken);
            if (!result.IsSuccess)
            {
                if (result.ErrorMessage?.Contains("not found") == true)
                {
                    return NotFound(result.ErrorMessage);
                }

                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await _basketService.DeleteAsync(id, cancellationToken);
            if (!result.IsSuccess)
            {
                return NotFound(result.ErrorMessage);
            }

            return NoContent();
        }

        [HttpGet("{id}/exists")]
        [Authorize]
        public async Task<ActionResult<bool>> Exists(int id, CancellationToken cancellationToken = default)
        {
            var result = await _basketService.ExistsAsync(id, cancellationToken);
            return Ok(result.Data);
        }
    }
}
