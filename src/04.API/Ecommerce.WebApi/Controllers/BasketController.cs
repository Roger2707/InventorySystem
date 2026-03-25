using ECommerce.Application.DTOs.Baskets;
using ECommerce.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers
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
        public async Task<ActionResult<IEnumerable<BasketDto>>> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _basketService.GetAllAsync(cancellationToken);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BasketDto>> GetById(int id, CancellationToken cancellationToken = default)
        {
            var result = await _basketService.GetByIdAsync(id, cancellationToken);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> Create([FromBody] CreateBasketDto createDto, CancellationToken cancellationToken = default)
        {
            var result = await _basketService.CreateAsync(createDto, cancellationToken);
            if (!result.IsSuccess) return BadRequest(result.ErrorMessage);
            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result.Data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BasketDto>> Update(int id, [FromBody] UpdateBasketDto updateDto, CancellationToken cancellationToken = default)
        {
            var result = await _basketService.UpdateAsync(id, updateDto, cancellationToken);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var result = await _basketService.DeleteAsync(id, cancellationToken);
            if (!result.IsSuccess) return NotFound(result.ErrorMessage);
            return NoContent();
        }
    }
}
