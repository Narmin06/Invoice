using EInvoice.Domain.Enum;
using EInvoice.Domain.Models.Common;
namespace EInvoice.Domain.Models;

public class Invoice : AuditableEntity, ISoftDeletable
{
    public Importer Importer { get; set; } = null!;
    public Exporter Exporter { get; set; } = null!;
    public InvoiceRequisites InvoiceRequisites { get; set; } = null!;
    public CircumstancesAffectingInvoice? CircumstancesAffectingInvoice { get; set; }

    public string PinCode { get; set; } = string.Empty;                        // Eslinde Tokenden goturmelidir. 
    public EInvoiceStatus Status { get; set; }

    public ICollection<Good> Goods { get; set; } = new List<Good>();

    public string FilePathUrl { get; set; } = string.Empty;

    public DateTime? DeletedTime { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<InvoiceFieldValue> InvoiceFieldValues { get; set; } = new List<InvoiceFieldValue>();
    public ICollection<InvoiceUpdateHistory> UpdateHistories { get; set; } = new List<InvoiceUpdateHistory>();
}