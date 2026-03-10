using EInvoice.Business.DTOs.GoodDTO;
using EInvoice.Business.Services.Internal.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EInvoice.WebApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class GoodController : ControllerBase
{
    private readonly IGoodService _goodService;

    public GoodController(IGoodService goodService)
    {
        _goodService = goodService;
    }


    // Public Operations
    [HttpGet("moderator")]
    public async Task<IActionResult> GetAllPublicAsync(CancellationToken cancellationToken)
    {
        var result = await _goodService.GetAllPublicAsync(cancellationToken);
        return Ok(result);
    }


    // Admin Operations
    [HttpGet("admin")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var result = await _goodService.GetAllAsync(cancellationToken);
        return Ok(result);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _goodService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }


    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] GoodCreateRequestDTO goodDto, CancellationToken cancellationToken)
    {
        if (goodDto == null)
            return BadRequest("GoodDto cannot be null.");

        await _goodService.CreateAsync(goodDto, cancellationToken);
        return Ok(new { message = "Good created successfully." });
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] GoodUpdateRequestDTO goodDto, CancellationToken cancellationToken)
    {
        if (goodDto == null)
            return BadRequest("GoodDto cannot be null.");

        await _goodService.UpdateAsync(id, goodDto, cancellationToken);
        return Ok(new { message = "Good updated successfully." });
    }


    [HttpDelete("admin/{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _goodService.DeleteAsync(id, cancellationToken);
            return Ok(new { message = "Good deleted successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }


    [HttpPatch("softdelete/{id}")]
    public async Task<IActionResult> SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _goodService.SoftDeleteAsync(id, cancellationToken);
            return Ok(new { message = "Good soft deleted successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }


    [HttpPatch("recover/{id}")]
    public async Task<IActionResult> RecoverAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _goodService.RecoverAsync(id, cancellationToken);
            return Ok(new { message = "Good recovered successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }


    [HttpPatch("activate/{id}")]
    public async Task<IActionResult> ActiceAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _goodService.ActiceAsync(id, cancellationToken);
            return Ok(new { message = "Good activated successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }


    [HttpPatch("deactivate/{id}")]
    public async Task<IActionResult> DeActiveAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _goodService.DeActiveAsync(id, cancellationToken);
            return Ok(new { message = "Good deactiavted successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
