using InventorySystem.Application.DTOs;
using InventorySystem.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    /// <summary>
    /// Get all warehouses
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<WarehouseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<WarehouseDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _warehouseService.GetAllAsync(cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Get warehouse by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(WarehouseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WarehouseDto>> GetById(int id, CancellationToken cancellationToken = default)
    {
        var result = await _warehouseService.GetByIdAsync(id, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return NotFound(result.ErrorMessage);
        }

        return Ok(result.Data);
    }

    /// <summary>
    /// Create a new warehouse
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(WarehouseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WarehouseDto>> Create([FromBody] CreateWarehouseDto createDto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _warehouseService.CreateAsync(createDto, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    /// <summary>
    /// Update an existing warehouse
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(WarehouseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WarehouseDto>> Update(int id, [FromBody] UpdateWarehouseDto updateDto, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _warehouseService.UpdateAsync(id, updateDto, cancellationToken);
        
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

    /// <summary>
    /// Delete a warehouse (soft delete)
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        var result = await _warehouseService.DeleteAsync(id, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return NotFound(result.ErrorMessage);
        }

        return NoContent();
    }

    /// <summary>
    /// Check if warehouse exists
    /// </summary>
    [HttpGet("{id}/exists")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<ActionResult<bool>> Exists(int id, CancellationToken cancellationToken = default)
    {
        var result = await _warehouseService.ExistsAsync(id, cancellationToken);
        return Ok(result.Data);
    }
}

