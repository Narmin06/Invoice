using EInvoice.Business.DTOs.AddressDTO;
using EInvoice.Business.DTOs.GoodDTO;
using EInvoice.Business.DTOs.InvoiceFieldValueDTO;
using EInvoice.Domain.Enum;
namespace EInvoice.Business.DTOs.InvoiceDTO;

public class InvoiceAdminResponseDTO
{
    public string PinCode { get; set; } = string.Empty;
    public EInvoiceStatus Status { get; set; } = EInvoiceStatus.Pending;


    // Exporter 
    public string? ExporterVoen { get; set; }
    public string ExporterName { get; set; } = string.Empty;  
    public AddressDto? ExporterAddress { get; set; }
    public EExporterStatus ExporterStatus { get; set; }


    // Sender -> Exporter cedvelinden gelir. Statuslari ferqlidi bir dene
    public string? SenderVoen { get; set; }              
    public string SenderName { get; set; } = string.Empty;
    public AddressDto? SenderAddress { get; set; }


    // Importer
    public string? ImporterVoen { get; set; }
    public string ImporterName { get; set; } = string.Empty;
    public AddressDto? ImporterAddress { get; set; }
    public EImporterStatus ImporterStatus { get; set; }


    // Recipient -> Importer cedvelinden gelir. Statuslari ferqlidi bir dene
    public string? RecipientVoen { get; set; }
    public string RecipientName { get; set; } = string.Empty;
    public AddressDto? RecipientAddress { get; set; }


    // Invoice Requisites
    public string InvoiceNumber { get; set; } = string.Empty; 
    public DateTime InvoiceDate { get; set; }
    public ECurrency CurrencyCode { get; set; }
    public string? ShortDeclarationNumber { get; set; }
    public string? Note { get; set; }
    public string? ContractNumberAndDate { get; set; }
    public ETransportConditions TransportConditions { get; set; }
    public EInvoicePurpose InvoicePurpose { get; set; }
    public EPaymentConditions PaymentConditions { get; set; }


    // Circumstances Affecting Invoice
    public  EDegreeInfluenceInvoice DegreeInfluenceInvoice { get; set; }
    public ETypeFunds TypeFunds { get; set; }
    public string Explanation { get; set; } = string.Empty;
    public decimal AmountFunds { get; set; }

    public string FileUrl { get; set; } = string.Empty;
    public IEnumerable<GoodAdminResponseDto> Goods { get; set; } = new List<GoodAdminResponseDto>();
    public IEnumerable<InvoiceFieldValueResponseDTO> FieldValues { get; set; } = new List<InvoiceFieldValueResponseDTO>();

    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
    public DateTime? DeleteTime { get; set; }
    public bool IsDeleted { get; set; } 
    public bool IsActive { get; set; }          
}