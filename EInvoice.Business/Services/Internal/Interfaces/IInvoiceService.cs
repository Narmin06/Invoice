using EInvoice.Business.DTOs.CommonDTO;
using EInvoice.Business.DTOs.InvoiceDTO;
using EInvoice.Business.DTOs.InvoiceDTOl;

namespace EInvoice.Business.Services.Internal.Interfaces;

public interface IInvoiceService
{
    // Public Operations
    Task<PagedResult<InvoiceResponseDTO>> GetAllPublicAsync(InvoiceQueryDTO dto, CancellationToken cancellationToken = default!);

    // Admin Operations
    Task CreateAsync(InvoiceCreateRequestDTO dto, CancellationToken cancellationToken = default!);
    Task UpdateAsync(Guid id, InvoiceUpdateRequestDTO dto, CancellationToken cancellationToken = default!);
    Task<InvoiceUpdateResponseDTO> UpdateResponseAsync(Guid id, CancellationToken cancellationToken = default!);
    Task<PagedResult<InvoiceAdminResponseDTO>> GetAllAsync(InvoiceQueryDTO dto, CancellationToken cancellationToken = default!);
    Task<InvoiceAdminResponseDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken = default!);
    Task ChangeStatusAsync(Guid id, CancellationToken cancellationToken = default!);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default!);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default!);
    Task RecoverAsync(Guid id, CancellationToken cancellationToken = default!);
    Task ActivateAsync(Guid id, CancellationToken cancellationToken = default!);
    Task DeactivateAsync(Guid id, CancellationToken cancellationToken = default!);
}