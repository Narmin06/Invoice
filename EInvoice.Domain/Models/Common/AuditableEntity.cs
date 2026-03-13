namespace EInvoice.Domain.Models.Common;

public class AuditableEntity : BaseEntity, IAuditableEntity
{
    public bool IsActive { get; set; }

    public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    public DateTime UpdateTime { get; set; }
}