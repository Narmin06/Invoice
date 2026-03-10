namespace EInvoice.Business.DTOs.InvoiceFieldDefinitionDTO;

public class InvoiceFieldDefinitionCreateRequestDto
{
    public required string Label { get; set; }
    public bool IsRequired { get; set; }
    public Guid? DocumentTypeId { get; set; }
}