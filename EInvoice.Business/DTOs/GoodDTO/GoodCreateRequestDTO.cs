namespace EInvoice.Business.DTOs.GoodDTO;

public class GoodCreateRequestDTO
{
    public string GoodCode { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public required int Quantity { get; set; }
    public Guid InvoiceId { get; set; }
}