namespace EInvoice.Business.DTOs.InvoiceFieldValueDTO;

public class InvoiceFieldValueResponseDTO
{
    public string Value { get; set; } = null!;
    public string FieldDefinitionName { get; set; } = null!;
    public Guid InvoiceFieldDefinitionId { get; set; }
}