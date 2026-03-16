namespace EInvoice.Business.DTOs.CommonDTO;

public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }                                           // Umumi data sayi
    public int PageNumber { get; set; }                                           // Cari (oldugumuz) sehifenin nomresi
    public int PageSize { get; set; }                                             // 1 sehifedeki data sayi
                                                                                  
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);    // Umumi sehife sayi
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}