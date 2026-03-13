using EInvoice.Business.DTOs.CommonDTO;
using EInvoice.Domain.Enum;
namespace EInvoice.Business.DTOs.InvoiceFieldDefinitionDTO;

public class InvoiceFieldDefinitionAdminResponseDto : AuditableEntityDto, ISoftDeletableDto
{
    public string Label { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public EFieldType FieldType { get; set; } = EFieldType.Text;

    public bool IsDeleted { get; set; }
    public DateTime? DeleteTime { get; set; }
}