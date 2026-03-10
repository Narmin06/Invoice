using EInvoice.Business.DTOs.CommonDTO;
namespace EInvoice.Business.DTOs.InvoiceFieldDefinitionDTO;

public class InvoiceFieldDefinitionAdminResponseDto : AuditableEntityDto, ISoftDeletableDto
{
    public string Label { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public Guid InvoiceFieldDefinitionId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeleteTime { get; set; }
}