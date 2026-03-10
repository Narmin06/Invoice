using EInvoice.Business.DTOs.AddressDTO;
namespace EInvoice.Business.DTOs.ExporterDTO;

public class ExporterDto
{
    public string? Voen { get; set; }
    public required string Name { get; set; }
    public required AddressDto Address { get; set; }
    public bool IsExporterDifferentFromSender { get; set; } = false;
}