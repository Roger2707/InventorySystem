using InventorySystem.Application.DTOs.PurchaseOrders;
using InventorySystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchaseOrderController : ControllerBase
{
    private readonly IPurchaseOrderService _purchaseOrderService;

    public PurchaseOrderController(IPurchaseOrderService purchaseOrderService)
    {
        _purchaseOrderService = purchaseOrderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PurchaseOrderDto>>> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _purchaseOrderService.GetAllAsync(cancellationToken);

        if (!result.IsSuccess)
        {
            return BadRequest(result.ErrorMessage);
        }

        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PurchaseOrderDto>> GetById(int id, CancellationToken cancellationToken = default)
    {
        var result = await _purchaseOrderService.GetByIdAsync(id, cancellationToken);

        if (!result.IsSuccess)
        {
            return NotFound(result.ErrorMessage);
        }

        return Ok(result.Data);
    }
}

