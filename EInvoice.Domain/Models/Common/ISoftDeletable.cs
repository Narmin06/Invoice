namespace EInvoice.Domain.Models.Common;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
    DateTime? DeletedTime { get; set; }
}