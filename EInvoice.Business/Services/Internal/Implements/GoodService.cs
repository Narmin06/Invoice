using EInvoice.Business.DTOs.CommonDTO;
using EInvoice.Business.DTOs.GoodDTO;
using EInvoice.Business.Extensions;
using EInvoice.Business.Services.Internal.Interfaces;
using EInvoice.DAL.Data;
using EInvoice.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace EInvoice.Business.Services.Internal.Implements;

public class GoodService : IGoodService
{
    private readonly IUnitOfWork _unitOfWork;

    public GoodService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Public Operations
    public async Task<IEnumerable<GoodResponseDto>> GetAllPublicAsync(CancellationToken cancellationToken = default)
    {
        IQueryable<Good> query = _unitOfWork.Repository<Good>().GetAll(includes: x => x.Include(good => good.Invoice))
            .Where(good => !good.IsDeleted && good.Invoice != null && !good.Invoice.IsDeleted);

        return await query.Select(good => new GoodResponseDto
        {
            GoodCode = good.GoodCode,
            Price = good.Price,
            Quantity = good.Quantity,
            InvoiceId = good.InvoiceId,
            TotalAmount = good.Price * good.Quantity
        }).ToListAsync(cancellationToken);
    }


    // Admin Operations
    public async Task CreateAsync(GoodCreateRequestDTO goodDto, CancellationToken cancellationToken = default)
    {
        if(goodDto == null)
            throw new ArgumentNullException(nameof(goodDto));

        var good = new Good
        {
            GoodCode = goodDto.GoodCode,
            Price = goodDto.Price,
            Quantity = goodDto.Quantity,
            InvoiceId = goodDto.InvoiceId,
            TotalAmount = goodDto.Price * goodDto.Quantity
        };

        var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsync(goodDto.InvoiceId, cancellationToken);
        if (invoice != null)
        {
            invoice.Goods.Add(good);  //Invoice-deki Goods siyahısına add
        }

        _unitOfWork.Repository<Good>().Create(good);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task UpdateAsync(Guid id, GoodUpdateRequestDTO goodDto, CancellationToken cancellationToken = default)
    {
        var good = await _unitOfWork.Repository<Good>().GetByIdAsync(id);

        if (good == null)
            throw new KeyNotFoundException($"Good with id {id} not found.");

        good.GoodCode = goodDto.GoodCode;
        good.Price = goodDto.Price;
        good.Quantity = goodDto.Quantity;
        good.TotalAmount = goodDto.Price * goodDto.Quantity;

        _unitOfWork.Repository<Good>().Update(good);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task<PagedResult<GoodAdminResponseDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IQueryable<Good> query = _unitOfWork.Repository<Good>().GetAll(includes: x => x.Include(good => good.Invoice));

        if (!query.Any())
            throw new InvalidOperationException("No goods found.");

        var pagedResult = await query.ToPagedResultAsync<Good>(
            pageNumber: 1,  
            pageSize: 10,   
            ct: cancellationToken
        );

        var goodItem = pagedResult.Items.Select(good => new GoodAdminResponseDto
        {
            Id = good.Id,
            GoodCode = good.GoodCode,
            Price = good.Price,
            Quantity = good.Quantity,
            InvoiceId = good.InvoiceId,
            IsDeleted = good.IsDeleted,
            TotalAmount = good.Price * good.Quantity
        }).ToList();

        return new PagedResult<GoodAdminResponseDto>
        {
            Items = goodItem,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalCount = pagedResult.TotalCount
        };
    }


    public async Task<GoodAdminResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var good = await _unitOfWork.Repository<Good>().GetByIdAsync(id, cancellationToken);

        if (good == null)
            throw new KeyNotFoundException($"Good with id {id} not found.");

        return new GoodAdminResponseDto
        {
            Id = good.Id,
            GoodCode = good.GoodCode,
            Price = good.Price,
            Quantity = good.Quantity,
            InvoiceId = good.InvoiceId,
            IsDeleted = good.IsDeleted,
            TotalAmount = good.Price * good.Quantity
        };
    }


    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var good = await _unitOfWork.Repository<Good>().GetByIdAsync(id, cancellationToken);

        if (good == null)
            throw new KeyNotFoundException($"Good with id {id} not found.");

        var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsync(good.InvoiceId, cancellationToken);

        if (invoice != null)
        {
            invoice.Goods.Remove(good);
        }

        _unitOfWork.Repository<Good>().Delete(good);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default) 
    {
        var good = await _unitOfWork.Repository<Good>().GetByIdAsync(id, cancellationToken);

        if(good is null)
            throw new KeyNotFoundException($"Good with id {id} not found.");

        if(good.IsDeleted)
        {
            throw new InvalidOperationException($"Good with id {id} is already soft-deleted.");
        }
        else
        {
            good.IsDeleted = true;
            good.DeletedTime = DateTime.UtcNow;
            _unitOfWork.Repository<Good>().Update(good);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task RecoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var good = await _unitOfWork.Repository<Good>().GetByIdAsync(id, cancellationToken);
        if(good is null)
            throw new KeyNotFoundException($"Good with id {id} not found.");

        if (good.IsDeleted)
        {
            good.IsDeleted = false;
            good.DeletedTime = null;
            _unitOfWork.Repository<Good>().Update(good);
        }
        else 
        {             
            throw new InvalidOperationException($"Good with id {id} is not soft-deleted.");
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task ActiceAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var good = await _unitOfWork.Repository<Good>().GetByIdAsync(id);

        if (good == null)
            throw new KeyNotFoundException($"Good with id {id} not found.");

        if (good.IsDeleted)
            throw new Exception($"Firstful Recover Good with id {id}.");

        if (good.IsActive)
        {
            throw new Exception($"Good with id {id} is already activated.");
        }
        else
        {
            good.IsActive = true;
            good.UpdateTime = DateTime.UtcNow;
            _unitOfWork.Repository<Good>().Update(good);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task DeActiveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var good = await _unitOfWork.Repository<Good>().GetByIdAsync(id);

        if(good == null)
            throw new KeyNotFoundException($"Good with id {id} not found.");

        if (good.IsDeleted)
            throw new Exception($"Firstful Recover Good with id {id}.");

        if (!good.IsActive)
        {
            throw new Exception($"Good with id {id} is already deactivated.");
        }
        else
        {
            good.IsActive = false;
            good.UpdateTime = DateTime.UtcNow;
            _unitOfWork.Repository<Good>().Update(good);
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}