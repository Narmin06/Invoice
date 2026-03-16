using EInvoice.Business.DTOs.CommonDTO;
using EInvoice.Business.DTOs.GoodDTO;
namespace EInvoice.Business.Services.Internal.Interfaces;

public interface IGoodService
{
    // Public Operations
    Task<IEnumerable<GoodResponseDto>> GetAllPublicAsync(CancellationToken cancellationToken = default);

    // Admin Operations
    Task CreateAsync (GoodCreateRequestDTO goodDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(Guid id, GoodUpdateRequestDTO goodDto, CancellationToken cancellationToken = default);
    Task<PagedResult<GoodAdminResponseDto>> GetAllAsync(CancellationToken cancellationToken = default);                                    
    Task<GoodAdminResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default); 
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task RecoverAsync(Guid id, CancellationToken cancellationToken = default);
    Task ActiceAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeActiveAsync(Guid id, CancellationToken cancellationToken = default);
}