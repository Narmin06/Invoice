using EInvoice.Business.DTOs.CircumstancesAffectingInvoiceDTO;
using EInvoice.Business.DTOs.ExporterDTO;
using EInvoice.Business.DTOs.InvoiceFieldValueDTO;
using EInvoice.Business.DTOs.InvoiceRequisites;
using Microsoft.AspNetCore.Http;

namespace EInvoice.Business.DTOs.InvoiceDTO;

public class InvoiceUpdateRequestDTO
{
    public ImporterDto? Importer { get; set; }
    public ExporterDto? Exporter { get; set; }
    public InvoiceRequisitesDto? InvoiceRequisites { get; set; }
    public CircumstancesAffectingInvoiceDto? CircumstancesAffectingInvoice { get; set; }

    public required IFormFile File { get; set; }

    public string? FieldValueJson { get; set; }
    public IEnumerable<InvoiceFieldValueCreateRequestDto>? FieldValues { get; set; }
}