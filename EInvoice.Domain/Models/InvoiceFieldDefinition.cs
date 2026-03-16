using EInvoice.Domain.Enum;
using EInvoice.Domain.Models.Common;
namespace EInvoice.Domain.Models;

public class InvoiceFieldDefinition : AuditableEntity, ISoftDeletable
{
    public string Label { get; set; } = null!;
    public bool IsRequired { get; set; } = true;
    public EFieldType FieldType { get; set; } = EFieldType.Text;    // Sahə tipi: text, number...

    public ICollection<InvoiceFieldValue> InvoiceFieldValues { get; set; } = new List<InvoiceFieldValue>();

    public bool IsDeleted { get; set; }
    public DateTime? DeletedTime { get; set; }
}