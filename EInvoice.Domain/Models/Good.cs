using EInvoice.Domain.Models.Common;
namespace EInvoice.Domain.Models;

public class Good : AuditableEntity, ISoftDeletable
{
    public string GoodCode { get; set; } = string.Empty;
    public decimal Price { get; set; }   
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }

    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = null!;

    public DateTime? DeletedTime { get; set; }
    public bool IsDeleted { get; set; }
}