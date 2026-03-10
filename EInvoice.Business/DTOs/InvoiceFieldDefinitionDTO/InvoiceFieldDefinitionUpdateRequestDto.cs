namespace EInvoice.Business.DTOs.InvoiceFieldDefinitionDTO;

public class InvoiceFieldDefinitionUpdateRequestDto
{
    public required string Label { get; set; }
    public bool IsRequired { get; set; }
}