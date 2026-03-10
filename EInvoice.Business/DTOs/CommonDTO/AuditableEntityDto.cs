namespace EInvoice.Business.DTOs.CommonDTO;

public class AuditableEntityDto : BaseEntityDto
{
    public DateTime CreateTime { get; set; }
    public DateTime? UpdateTime { get; set; }
    public bool IsActive { get; set; }
}