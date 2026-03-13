using EInvoice.Domain.Enum;
using EInvoice.Domain.Models.Common;
namespace EInvoice.Domain.Models;

public class InvoiceRequisites : BaseEntity
{
    public required string InvoiceNumber { get; set; } = "Nomresiz";
    public required DateTime InvoiceDate { get; set; }
    public required ECurrency CurrencyCode { get; set; }
    public string ShortDeclarationNumber { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public string ContractNumberAndDate { get; set; } = string.Empty;
    public ETransportConditions TransportConditions { get; set; }
    public required EInvoicePurpose InvoicePurpose { get; set; }
    public required EPaymentConditions PaymentConditions { get; set; }
}