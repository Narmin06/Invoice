using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Business.DTOs.GoodDTO;

public class GoodCreateRequestDTO
{
    public string GoodCode { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public required int Quantity { get; set; }
    public Guid InvoiceId { get; set; }
}
