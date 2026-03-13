using EInvoice.Business.DTOs.InvoiceFieldValueDTO;
using EInvoice.Domain.Enum;
namespace EInvoice.Business.DTOs.InvoiceDTO;

public class InvoiceResponseDTO 
{
    public string PinCode { get; set; } = string.Empty;
    public DateTime CreateTime { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;           //InvoiceRequisites dan gelir
    public DateTime InvoiceDate { get; set; }                           //InvoiceRequisites dan gelir
    public string ExporterName { get; set; } = string.Empty;            //Exporter dan gelir
    public string ImporterName { get; set; } = string.Empty;            //Importer dan gelir
    public string GoodsCount { get; set; } = string.Empty;              //Invoice dan gelir
    public string TotalAmount { get; set; } = string.Empty;             // GoodDto dan gelir
    public EInvoiceStatus Status { get; set; } = EInvoiceStatus.Pending;

    public IEnumerable<InvoiceFieldValueResponseDTO> FieldValues { get; set; } = new List<InvoiceFieldValueResponseDTO>();
}