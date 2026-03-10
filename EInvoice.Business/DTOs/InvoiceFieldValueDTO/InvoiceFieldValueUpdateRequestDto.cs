namespace EInvoice.Business.DTOs.InvoiceFieldValueDTO;

public class InvoiceFieldValueUpdateRequestDto
{
    public string? Value { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid InvoiceFieldDefinitionId { get; set; }
}