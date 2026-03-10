using EInvoice.Business.DTOs.CommonDTO;
namespace EInvoice.Business.DTOs.GoodDTO;

public class GoodAdminResponseDto : AuditableEntityDto, ISoftDeletableDto
{
    public string GoodCode { get; set; } = string.Empty; 
    public decimal Price { get; set; }
    public required int Quantity { get; set; }
    public Guid InvoiceId { get; set; }
    public decimal TotalAmount { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeleteTime { get; set; }
}