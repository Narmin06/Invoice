using EInvoice.Domain.Models.Common;
namespace EInvoice.Domain.Models;

public class Good : AuditableEntity, ISoftDeletable
{
    public string GoodCode { get; set; } = string.Empty;
    public decimal Price { get; set; }   
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }

    public Guid InvoiceId { get; set; }
    public Invoice Invoice { get; set; } = null!;

    public DateTime? DeletedTime { get; set; }
    public bool IsDeleted { get; set; }
}
// Demeli mallari import ve export ede bilirik. Yeni Export etdikde excel fayli kimi komputere yuklenir. Bu excel faylinin icinde 10 reqemden ibaret GoodsCode olur. Import etdikde ise bu excel fayli komputerdən goturulur ve goods listine elave olunur. Elave olunmamisdan evvel  "Kohne goods silinsin?" kimi sual mesaji gelir(Bunu front edir, bizlik deyil). Yeni Import etdikde ya kohne goods silinir, daha sonra elave olunur. Ya da kohne good silinmeden Godds listine elave olunur.