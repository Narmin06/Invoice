using EInvoice.Domain.Enum;
using EInvoice.Domain.Models.Common;
namespace EInvoice.Domain.Models;

public class Importer : BaseEntity
{
    public required string Voen { get; set; }
    public required string Name { get; set; }
    public Address Address { get; set; } = null!;

    public Guid InvoiceId { get; set; }

    public EImporterStatus Status { get; set; }

    // Importer Dtodaki IsImporterDifferentFromRecipient == true olarsa Yuxaridaki entitilerden elave olaraq yene eyni entitilerden ibaret xanalar doldurulmalidir. Eks halda her iki datalar beraber olur.yeniden doldurmaga ehtiyac yoxdur.
}