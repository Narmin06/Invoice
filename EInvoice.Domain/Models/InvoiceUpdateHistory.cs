using EInvoice.Domain.Enum;
using EInvoice.Domain.Models.Common;
namespace EInvoice.Domain.Models;

public class InvoiceUpdateHistory : BaseEntity
{
    public int Order { get; set; }
    public DateTime UpdateTime { get; set; }
    public string PinCode { get; set; } = string.Empty;
    public EUpdateStatus StatusUpdate { get; set; }
    public string Note { get; set; } = string.Empty;

    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = null!;
}