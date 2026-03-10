using EInvoice.Domain.Enum;
namespace EInvoice.Domain.Models;

public class CircumstancesAffectingInvoice 
{
    public required EDegreeInfluenceInvoice DegreeInfluenceInvoice { get; set; }  
    public required ETypeFunds TypeFunds { get; set; }  
    public required string Explanation { get; set; }
    public required decimal AmountFunds { get; set; }
}