using EInvoice.Business.DTOs.CommonDTO;
using EInvoice.Business.DTOs.CommonDTO.EnumDTO;
namespace EInvoice.Business.DTOs.InvoiceDTO;

public class InvoiceQueryDTO : BaseQueryDto
{
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }

    public ESubjectType SubjectType { get; set; }
    public string Voen { get; set; } = string.Empty;          
    public string Name { get; set; } = string.Empty;          
    public string InvoiceNo { get; set; } = string.Empty;  
    public DateTime InvoiceDate { get; set; }  
    public string PinCode { get; set; }  = string.Empty;
    public string NumberOfShortDeclaration { get; set; } = string.Empty;
}