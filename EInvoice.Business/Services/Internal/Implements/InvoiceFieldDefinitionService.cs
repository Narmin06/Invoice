using EInvoice.Business.DTOs.CommonDTO;
using EInvoice.Business.DTOs.InvoiceFieldDefinitionDTO;
using EInvoice.Business.Extensions;
using EInvoice.Business.Services.Internal.Interfaces;
using EInvoice.DAL.Data;
using EInvoice.Domain.Models;
namespace EInvoice.Business.Services.Internal.Implements;

public class InvoiceFieldDefinitionService : IInvoiceFieldDefinitionService
{
    private readonly IUnitOfWork _unitOfWork;
    public InvoiceFieldDefinitionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    // Public Operations
    public async Task<PagedResult<InvoiceFieldDefinitionResponseDto>> GetAllPublicAsync(BaseQueryDto dto, CancellationToken cancellationToken = default)
    {
        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        IQueryable<InvoiceFieldDefinition> query = _unitOfWork.Repository<InvoiceFieldDefinition>().GetAll().Where(x => x.IsDeleted == false);


        //if (!string.IsNullOrEmpty(dto.Search))
        //    query = query.Where(x => x.Label.Contains(dto.Search));

        int pageNumber = dto.PageNumber <= 0 ? 1 : dto.PageNumber;
        int pageSize = dto.PageSize <= 0 ? 10 : dto.PageSize;
        var pagedResult = await query.ToPagedResultAsync(pageNumber, pageSize, cancellationToken);

        var dtoItems = pagedResult.Items.Select(x => new InvoiceFieldDefinitionResponseDto
        {
            Label = x.Label,
            IsRequired = x.IsRequired,
        }).ToList();

        return new PagedResult<InvoiceFieldDefinitionResponseDto>
        {
            Items = dtoItems,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalCount = pagedResult.TotalCount
        };
    }


    // Admin Operations
    public async Task CreateAsync(InvoiceFieldDefinitionCreateRequestDto dto, CancellationToken cancellationToken = default)
    {
        if(dto is null)
            throw new ArgumentNullException(nameof(dto));

        var invoiceDefinition = new InvoiceFieldDefinition
        { 
            Label = dto.Label,
            IsRequired = dto.IsRequired,
            FieldType = dto.FieldType
        };

        _unitOfWork.Repository<InvoiceFieldDefinition>().Create(invoiceDefinition);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task UpdateAsync(Guid id, InvoiceFieldDefinitionUpdateRequestDto dto, CancellationToken cancellationToken = default)
    {
        var invoiceDefinition = await _unitOfWork.Repository<InvoiceFieldDefinition>().GetByIdAsync(id);

        if(invoiceDefinition is null)
            throw new KeyNotFoundException($"InvoiceFieldDefinition with id {id} not found.");

        invoiceDefinition.Label = dto.Label;
        invoiceDefinition.IsRequired = dto.IsRequired;
        invoiceDefinition.FieldType = dto.FieldType;

        _unitOfWork.Repository<InvoiceFieldDefinition>().Update(invoiceDefinition);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task<PagedResult<InvoiceFieldDefinitionAdminResponseDto>> GetAllAsync(BaseQueryDto dto, CancellationToken cancellationToken = default)
    {
        IQueryable<InvoiceFieldDefinition> query = _unitOfWork.Repository<InvoiceFieldDefinition>().GetAll();

        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        //if (!string.IsNullOrEmpty(dto.Search))
        //    query = query.Where(x => x.Label.Contains(dto.Search));

        int pageNumber = dto.PageNumber <= 0 ? 1 : dto.PageNumber;
        int pageSize = dto.PageSize <= 0 ? 10 : dto.PageSize;
        var pageResult = await query.ToPagedResultAsync(pageNumber, pageSize, cancellationToken);

        var dtoItems = pageResult.Items.Select(x => new InvoiceFieldDefinitionAdminResponseDto
        {
            Id = x.Id,
            Label = x.Label,
            IsRequired = x.IsRequired,
            IsDeleted = x.IsDeleted,
            DeleteTime = x.IsDeleted ? x.DeletedTime ?? DateTime.UtcNow : null,  
            UpdateTime = x.UpdateTime
        }).ToList();

        return new PagedResult<InvoiceFieldDefinitionAdminResponseDto>() 
        { 
            Items = dtoItems,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = dtoItems.Count,
        };
    }


    public async Task<InvoiceFieldDefinitionResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var invoiceFieldDefinition = await _unitOfWork.Repository<InvoiceFieldDefinition>().GetByIdAsync(id);

        if(invoiceFieldDefinition == null)
            throw new KeyNotFoundException ($"InvoiceFieldDefinition with id {id} not found.");

        return new InvoiceFieldDefinitionResponseDto
        {
            Label = invoiceFieldDefinition.Label,
            IsRequired = invoiceFieldDefinition.IsRequired,
        };
    }


    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var invoiceFieldDefinition = await _unitOfWork.Repository<InvoiceFieldDefinition>().GetByIdAsync(id);

        if(invoiceFieldDefinition == null)
            throw new KeyNotFoundException($"InvoiceFieldDefinition with id {id} not found.");

        _unitOfWork.Repository<InvoiceFieldDefinition>().Delete(invoiceFieldDefinition);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var invoiceFieldDefinition = await _unitOfWork.Repository<InvoiceFieldDefinition>().GetByIdAsync(id);

        if (invoiceFieldDefinition is null)
            throw new KeyNotFoundException($"InvoiceFieldDefinition with id {id} not found.");

        if (invoiceFieldDefinition.IsDeleted)
        {
            throw new Exception($"InvoiceFieldDefinition with id {id} is already soft deleted.");
        }
        else
        {
            invoiceFieldDefinition.IsDeleted = true;
            invoiceFieldDefinition.DeletedTime = DateTime.UtcNow;
            invoiceFieldDefinition.UpdateTime = DateTime.UtcNow;
        }
        _unitOfWork.Repository<InvoiceFieldDefinition>().Update(invoiceFieldDefinition);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task RecoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var invoiceFieldDefinition = await _unitOfWork.Repository<InvoiceFieldDefinition>().GetByIdAsync(id);

        if (invoiceFieldDefinition is null)
            throw new KeyNotFoundException($"InvoiceFieldDefinition with id {id} not found.");

        if (invoiceFieldDefinition.IsDeleted)
        {
            invoiceFieldDefinition.IsDeleted = false;
            invoiceFieldDefinition.DeletedTime = null;
            invoiceFieldDefinition.UpdateTime = DateTime.UtcNow;
        }
        else
        {
            throw new Exception($"InvoiceFieldDefinition with id {id} is not soft deleted, so it cannot be recovered.");
        }
        _unitOfWork.Repository<InvoiceFieldDefinition>().Update(invoiceFieldDefinition);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}