using EInvoice.Domain.Enum;
namespace EInvoice.Business.DTOs.CircumstancesAffectingInvoiceDTO;

public class CircumstancesAffectingInvoiceDto
{
    public bool IsCircumstancesAffectingInvoice { get; set; } = false;  
    public required EDegreeInfluenceInvoice DegreeInfluenceInvoice { get; set; }
    public required ETypeFunds TypeFunds { get; set; }
    public required string Explanation { get; set; }
    public required decimal AmountFunds { get; set; }
}