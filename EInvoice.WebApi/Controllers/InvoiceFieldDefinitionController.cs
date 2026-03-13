using EInvoice.Business.DTOs.CommonDTO;
using EInvoice.Business.DTOs.InvoiceFieldDefinitionDTO;
using EInvoice.Business.Services.Internal.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EInvoice.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceFieldDefinitionController : ControllerBase
{
    private readonly IInvoiceFieldDefinitionService _invoiceFieldDefinitionService;

    public InvoiceFieldDefinitionController(IInvoiceFieldDefinitionService invoiceFieldDefinitionService)
    {
        _invoiceFieldDefinitionService = invoiceFieldDefinitionService;
    }


    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] InvoiceFieldDefinitionCreateRequestDto invoiceDto, CancellationToken cancellationToken)
    {
        if (invoiceDto == null)
            return BadRequest("Invalid invoice field definition data.");

        await _invoiceFieldDefinitionService.CreateAsync(invoiceDto, cancellationToken);
        return Ok(new { message = "Invoice field definition created successfully." });
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] InvoiceFieldDefinitionUpdateRequestDto invoiceDto, CancellationToken cancellationToken)
    {
        if (invoiceDto == null)
            return BadRequest("Invalid invoice field definition data.");

        await _invoiceFieldDefinitionService.UpdateAsync(id, invoiceDto, cancellationToken);
        return Ok(new { message = "Invoice field definition updated successfully." });
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _invoiceFieldDefinitionService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpGet("admin")]
    public async Task<IActionResult> GetAllAdminAsync([FromQuery] BaseQueryDto queryDto, CancellationToken cancellationToken)
    {
        if (queryDto == null)
            return BadRequest("Query parameters are missing.");

        var result = await _invoiceFieldDefinitionService.GetAllAsync(queryDto, cancellationToken);
        return Ok(result);
    }


    [HttpGet("moderator")]
    public async Task<IActionResult> GetAllPublicAsync([FromQuery] BaseQueryDto queryDto, CancellationToken cancellationToken)
    {
        if (queryDto == null)
            return BadRequest("Query parameters are missing.");

        var result = await _invoiceFieldDefinitionService.GetAllPublicAsync(queryDto, cancellationToken);
        return Ok(result);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _invoiceFieldDefinitionService.DeleteAsync(id, cancellationToken);
            return Ok(new { message = "Invoice field definition deleted successfully." });
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
            await _invoiceFieldDefinitionService.SoftDeleteAsync(id, cancellationToken);
            return Ok(new { message = "Invoice field definition soft deleted successfully." });
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
            await _invoiceFieldDefinitionService.RecoverAsync(id, cancellationToken);
            return Ok(new { message = "Invoice field definition recovered successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}