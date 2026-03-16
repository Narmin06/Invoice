using EInvoice.Business.DTOs.InvoiceDTO;
using EInvoice.Business.DTOs.InvoiceDTOl;
using EInvoice.Business.DTOs.InvoiceFieldValueDTO;
using EInvoice.Business.Services.Internal.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace EInvoice.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;
    public InvoiceController(IInvoiceService invoiceService) 
    {
        _invoiceService = invoiceService;
    }


    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] InvoiceCreateRequestDTO dto, CancellationToken cancellationToken)
    {
        if (dto == null)
            return BadRequest("Invalid invoice data.");

        if (dto.File == null)
            return BadRequest("File is required.");

        if (!string.IsNullOrWhiteSpace(dto.FieldValueJson))
        {
            var fieldValues = JsonConvert.DeserializeObject<IEnumerable<InvoiceFieldValueCreateRequestDto>>(dto.FieldValueJson);
            dto.FieldValues = fieldValues?.ToList();
        }
        await _invoiceService.CreateAsync(dto, cancellationToken);
        return Ok(new { message = "Invoice created successfully." });
    }


    //[HttpPut("{id:guid}")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, [FromForm] InvoiceUpdateRequestDTO dto, CancellationToken cancellationToken)
    {
        if (dto == null)
            return BadRequest("Invalid invoice data.");

        if (dto.File == null)
            return BadRequest("File is required.");

        if (!string.IsNullOrWhiteSpace(dto.FieldValueJson))
        {
            var fieldValues = JsonConvert.DeserializeObject<IEnumerable<InvoiceFieldValueCreateRequestDto>>(dto.FieldValueJson);
            dto.FieldValues = fieldValues?.ToList() ?? new List<InvoiceFieldValueCreateRequestDto>();
        }

        await _invoiceService.UpdateAsync(id, dto, cancellationToken);
        return Ok(new { message = "Invoice updated successfully." });
    }



    [HttpGet("moderator")]
    public async Task<IActionResult> GetAllPublicAsync([FromQuery] InvoiceQueryDTO dto, CancellationToken cancellationToken)
    {
        if (dto == null)
            return BadRequest("Query parameters are missing.");

        var result = await _invoiceService.GetAllPublicAsync(dto, cancellationToken);
        return Ok(result);
    }


    [HttpGet("admin")]
    public async Task<IActionResult> GetAllAsync([FromQuery] InvoiceQueryDTO dto, CancellationToken cancellationToken)
    {
        if (dto == null)
            return BadRequest("Query parameters are missing.");

        var result = await _invoiceService.GetAllAsync(dto, cancellationToken);
        return Ok(result);
    }

 
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _invoiceService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _invoiceService.DeleteAsync(id, cancellationToken);
            return Ok(new { message = "Invoice deleted successfully." });
        } 
        catch (Exception ex) 
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }


    [HttpPatch("{id}/soft-delete")]
    public async Task<IActionResult> SoftDeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _invoiceService.SoftDeleteAsync(id, cancellationToken);
            return Ok(new { message = "Invoice soft deleted successfully." });
        }
        catch (Exception ex) 
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }


    [HttpPatch("{id}/recover")]
    public async Task<IActionResult> RecoverAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _invoiceService.RecoverAsync(id, cancellationToken);
            return Ok(new { message = "Invoice recovered successfully." });
        }
        catch(Exception ex) 
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }


    [HttpPatch("{id}/activate")]
    public async Task<IActionResult> ActivateAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _invoiceService.ActivateAsync(id, cancellationToken);
            return Ok(new { message = "Invoice activated successfully." });
        }
        catch (Exception ex)
        { 
            return StatusCode(500, new { message = ex.Message });
        }
    }


    [HttpPatch("{id}/deactivate")]
    public async Task<IActionResult> DeactivateAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        { 
            await _invoiceService.DeactivateAsync(id, cancellationToken);
            return Ok(new { message = "Invoice deactivated successfully." });
        }
        catch (Exception ex) 
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }


    [HttpPatch("{id}/change-status")]
    public async Task<IActionResult> ChangeStatusAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _invoiceService.ChangeStatusAsync(id, cancellationToken);
            return Ok(new { message = "Invoice status changed successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}