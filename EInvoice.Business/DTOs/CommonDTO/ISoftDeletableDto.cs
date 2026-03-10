namespace EInvoice.Business.DTOs.CommonDTO;

public interface ISoftDeletableDto
{
    bool IsDeleted { get; set; }
    DateTime? DeleteTime { get; set; }
}