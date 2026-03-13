using EInvoice.Business.DTOs.AddressDTO;
namespace EInvoice.Business.DTOs;

public class ImporterDto
{
    public required string Voen { get; set; }
    public required string Name { get; set; }
    public required AddressDto Address { get; set; }
    public bool IsImporterDifferentFromRecipient { get; set; } //true olsa StatusImporter = 2 
}