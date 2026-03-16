using EInvoice.Domain.Enum;
namespace EInvoice.Business.DTOs.InvoiceFieldDefinitionDTO;

public class InvoiceFieldDefinitionResponseDto
{
    public string Label { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public EFieldType FieldType { get; set; } = EFieldType.Text;
}