namespace EInvoice.Domain.Models;

public class Address
{
    public string? StreetAndNumber { get; set; }
    public string? PostalCode { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
}
