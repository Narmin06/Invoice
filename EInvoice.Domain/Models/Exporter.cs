using EInvoice.Domain.Enum;
using EInvoice.Domain.Models.Common;
namespace EInvoice.Domain.Models;

public class Exporter : BaseEntity
{
    public string Voen { get; set; } = string.Empty;
    public required string Name { get; set; }
    public Address Address { get; set; } = null!;

    public Guid InvoiceId { get; set; }


    public EExporterStatus Status {  get; set; }
}