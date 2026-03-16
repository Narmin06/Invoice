using EInvoice.Domain.Enum;
namespace EInvoice.Business.DTOs.InvoiceFieldDefinitionDTO;

public class InvoiceFieldDefinitionCreateRequestDto
{
    public required string Label { get; set; }
    public bool IsRequired { get; set; }
    public EFieldType FieldType { get; set; } = EFieldType.Text;
    //public Guid InvoiceId { get; set; }
}