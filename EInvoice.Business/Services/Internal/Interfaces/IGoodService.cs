using EInvoice.Business.DTOs.CommonDTO;
using EInvoice.Business.DTOs.GoodDTO;

namespace EInvoice.Business.Services.Internal.Interfaces;

public interface IGoodService
{
    // Public Operations
    Task<IEnumerable<GoodResponseDto>> GetAllPublicAsync(CancellationToken cancellationToken);

    // Admin Operations
    Task<GoodAdminResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken); 
    Task<PagedResult<GoodAdminResponseDto>> GetAllAsync(CancellationToken cancellationToken);                                    
    Task CreateAsync (GoodCreateRequestDTO goodDto, CancellationToken cancellationToken);
    Task UpdateAsync(Guid id, GoodUpdateRequestDTO goodDto, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken);
    Task RecoverAsync(Guid id, CancellationToken cancellationToken);
    Task ActiceAsync(Guid id, CancellationToken cancellationToken);
    Task DeActiveAsync(Guid id, CancellationToken cancellationToken);

    // Export
    // Import
}