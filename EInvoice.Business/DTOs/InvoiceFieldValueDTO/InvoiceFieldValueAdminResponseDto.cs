using EInvoice.Business.DTOs.CommonDTO;
namespace EInvoice.Business.DTOs.InvoiceFieldValueDTO;

public class InvoiceFieldValueAdminResponseDto : AuditableEntityDto, ISoftDeletableDto
{
    public string Value { get; set; } = string.Empty;
    public Guid InvoiceFieldValueId { get; set; }
    public Guid InvoiceId { get; set; }
    public Guid InvoiceFieldDefinitionId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeleteTime { get; set; }
}