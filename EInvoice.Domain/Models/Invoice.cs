using EInvoice.Domain.Enum;
using EInvoice.Domain.Models.Common;
namespace EInvoice.Domain.Models;

public class Invoice : AuditableEntity, ISoftDeletable
{
    public Importer Importer { get; set; } = null!;

    public Exporter Exporter { get; set; } = null!;
    public InvoiceRequisites? InvoiceRequisites { get; set; }
    public CircumstancesAffectingInvoice CircumstancesAffectingInvoice { get; set; } = null!;


    public EInvoiceStatus Status { get; set; }

    public ICollection<Good> Goods { get; set; } = new List<Good>();

    public string FilePathUrl { get; set; } = string.Empty;

    public DateTime? DeletedTime { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<InvoiceFieldValue> FieldValues { get; set; } = new List<InvoiceFieldValue>();
}



