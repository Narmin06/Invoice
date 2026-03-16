using EInvoice.Business.DTOs.CommonDTO;
using EInvoice.Business.DTOs.InvoiceFieldDefinitionDTO;
namespace EInvoice.Business.Services.Internal.Interfaces;

public interface IInvoiceFieldDefinitionService
{
    // Public Operations
    Task<PagedResult<InvoiceFieldDefinitionResponseDto>> GetAllPublicAsync(BaseQueryDto dto, CancellationToken cancellationToken = default!);

    // Admin Operations
    Task CreateAsync(InvoiceFieldDefinitionCreateRequestDto documentDto, CancellationToken cancellationToken = default!);
    Task UpdateAsync(Guid id, InvoiceFieldDefinitionUpdateRequestDto documentDto, CancellationToken cancellationToken = default!);
    Task<PagedResult<InvoiceFieldDefinitionAdminResponseDto>> GetAllAsync(BaseQueryDto dto, CancellationToken cancellationToken = default!);
    Task<InvoiceFieldDefinitionResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default!);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default!);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default!);
    Task RecoverAsync(Guid id, CancellationToken cancellationToken = default!);
}