using InventorySystem.Application.DTOs.GoodsReceipts;
using InventorySystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GoodsReceiptController : ControllerBase
{
    private readonly IGoodsReceiptService _goodsReceiptService;

    public GoodsReceiptController(IGoodsReceiptService goodsReceiptService)
    {
        _goodsReceiptService = goodsReceiptService;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<GoodsReceiptDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _goodsReceiptService.GetAllAsync(cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<GoodsReceiptDto>> GetById(int id, CancellationToken cancellationToken = default)
    {
        var result = await _goodsReceiptService.GetByIdAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result.ErrorMessage);
        }

        return Ok(result.Data);
    }
}

