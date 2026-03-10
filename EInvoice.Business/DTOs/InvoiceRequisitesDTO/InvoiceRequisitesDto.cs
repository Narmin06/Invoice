using EInvoice.Domain.Enum;
namespace EInvoice.Business.DTOs.InvoiceRequisites;

public class InvoiceRequisitesDto
{
    public required string InvoiceNumber { get; set; } = "Nomresiz";
    public required DateTime InvoiceDate { get; set; }
    public required ECurrency CurrencyCode { get; set; }
    public string? ShortDeclarationNumber { get; set; }
    public string? Note { get; set; }
    public string? ContractNumberAndDate { get; set; }
    public ETransportConditions TransportConditions { get; set; }
    public required EInvoicePurpose InvoicePurpose { get; set; }
    public required EPaymentConditions PaymentConditions { get; set; }
}