using EInvoice.Business.DTOs.CommonDTO;
using EInvoice.Domain.Enum;
namespace EInvoice.Business.DTOs.InvoiceDTO;

public class InvoiceUpdateResponseDTO :BaseEntityDto
{
    public int Order { get; set; } 
    public DateTime? UpdateTime { get; set; }
    public string PinCode { get; set; } = string.Empty;        
    public EUpdateStatus StatusUpdate { get; set; } = EUpdateStatus.HasBeenCorrected;
    public string Note { get; set; } = string.Empty;
}