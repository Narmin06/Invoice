namespace EInvoice.Business.DTOs.InvoiceFieldValueDTO;

public class InvoiceFieldValueCreateRequestDto
{
    public string Value { get; set; } = string.Empty;
    public Guid InvoiceFieldDefinitionId { get; set; }
}