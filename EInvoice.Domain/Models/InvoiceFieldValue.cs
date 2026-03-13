using EInvoice.Domain.Models.Common;
namespace EInvoice.Domain.Models;

public class InvoiceFieldValue : AuditableEntity
{
    public string Value { get; set; } = null!;

    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = null!;

    public Guid InvoiceFieldDefinitionId { get; set; }
    public InvoiceFieldDefinition InvoiceFieldDefinition { get; set; } = null!;

}