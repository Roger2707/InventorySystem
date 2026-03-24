using InventorySystem.Application.Interfaces.Cache;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RedisController : ControllerBase
{
    private readonly ICacheService _cacheService;

    public RedisController(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public class CacheRequest
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    [HttpPost("set")]
    public async Task<IActionResult> SetCache([FromBody] CacheRequest request)
    {
        await _cacheService.SetAsync(request.Key, request.Value, TimeSpan.FromMinutes(5));
        return Ok(new { message = $"Đã lưu {request.Key} thành công vào Redis!" });
    }

    [HttpGet("get/{key}")]
    public async Task<IActionResult> GetCache(string key)
    {
        var value = await _cacheService.GetAsync<string>(key);

        if (value == null)
            return NotFound(new { message = "Không tìm thấy Key này hoặc đã hết hạn" });

        return Ok(new { key = key, value = value });
    }
}

