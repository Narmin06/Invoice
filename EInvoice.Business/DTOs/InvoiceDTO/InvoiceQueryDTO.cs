using EInvoice.Business.DTOs.CommonDTO;
namespace EInvoice.Business.DTOs.InvoiceDTO;

public class InvoiceQueryDTO : BaseQueryDto
{
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
}