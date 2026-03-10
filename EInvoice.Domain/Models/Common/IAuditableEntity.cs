namespace EInvoice.Domain.Models.Common;

public interface IAuditableEntity
{
    bool IsActive { get; set; }

    DateTime CreateTime { get; set; }
    DateTime? UpdateTime { get; set; }
}