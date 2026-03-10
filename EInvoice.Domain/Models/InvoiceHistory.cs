namespace EInvoice.Domain.Models;

public class InvoiceHistory
{
    public int Order { get; set; }  
    public DateTime UpdateTime { get; set; }
    public string Executor { get; set; } = string.Empty;        // Tokenden goturur
    public string Status { get; set; } = "New";
    public string Note { get; set; } = string.Empty;
}
