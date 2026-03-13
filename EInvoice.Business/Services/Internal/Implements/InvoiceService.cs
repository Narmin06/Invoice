using EInvoice.Business.DTOs.AddressDTO;
using EInvoice.Business.DTOs.CommonDTO;
using EInvoice.Business.DTOs.CommonDTO.EnumDTO;
using EInvoice.Business.DTOs.GoodDTO;
using EInvoice.Business.DTOs.InvoiceDTO;
using EInvoice.Business.DTOs.InvoiceDTOl;
using EInvoice.Business.DTOs.InvoiceFieldValueDTO;
using EInvoice.Business.Extensions;
using EInvoice.Business.Services.External.Interfaces;
using EInvoice.Business.Services.Internal.Interfaces;
using EInvoice.DAL.Data;
using EInvoice.Domain.Enum;
using EInvoice.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EInvoice.Business.Services.Internal.Implements;

public class InvoiceService : IInvoiceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;

    public InvoiceService(IUnitOfWork unitOfWork, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }


    // Public Operations
    public async Task<PagedResult<InvoiceResponseDTO>> GetAllPublicAsync(InvoiceQueryDTO dto, CancellationToken cancellationToken = default)
    {
        IQueryable<Invoice> query = _unitOfWork.Repository<Invoice>().GetAll(includes: x => x.Include(invoice => invoice.Importer)
                                                                                             .Include(invoice => invoice.Exporter)
                                                                                             .Include(invoice => invoice.InvoiceRequisites)
                                                                                             .Include(invoice => invoice.CircumstancesAffectingInvoice)
                                                                                             .Include(invoice => invoice.Goods)
                                                                                             .Include(invoice => invoice.InvoiceFieldValues));
        query = query.Where(x => !x.IsDeleted && x.IsActive);

        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        if (dto.CreatedFrom.HasValue)
            query = query.Where(x => x.CreateTime >= dto.CreatedFrom.Value);

        if (dto.CreatedTo.HasValue)
            query = query.Where(x => x.CreateTime <= dto.CreatedTo.Value);

        if (!string.IsNullOrEmpty(dto.InvoiceNo))
            query = query.Where(x => x.InvoiceRequisites != null && 
                                     x.InvoiceRequisites.InvoiceNumber.Contains(dto.InvoiceNo));
        
        if (!string.IsNullOrEmpty(dto.PinCode))
            query = query.Where(x => x.PinCode.Contains(dto.PinCode));

        if (!string.IsNullOrEmpty(dto.NumberOfShortDeclaration))
            query = query.Where(x => x.InvoiceRequisites != null &&
                                     x.InvoiceRequisites.ShortDeclarationNumber.Contains(dto.NumberOfShortDeclaration));

        if (dto.InvoiceDate != default)
            query = query.Where(x => x.InvoiceRequisites != null &&
                                     x.InvoiceRequisites.InvoiceDate == dto.InvoiceDate.Date);

        if (dto.SubjectType != ESubjectTypeDto.None) 
        {
            switch (dto.SubjectType)
            {
                case ESubjectTypeDto.Exporter:
                    query = query.Where(x => x.Exporter != null && x.Exporter.Status == EExporterStatus.Exporter); 
                    break;

                case ESubjectTypeDto.Importer:
                    query = query.Where(x => x.Importer != null && x.Importer.Status == EImporterStatus.Importer); 
                    break;

                case ESubjectTypeDto.Sender:
                    var senderInvoiceIds = _unitOfWork.Repository<Exporter>().GetAll(x => x.Status == EExporterStatus.Sender)
                                                                             .Select(x => x.InvoiceId);
                    query = query.Where(x => x.Exporter != null && x.Exporter.Status == EExporterStatus.Sender);   
                    break;

                case ESubjectTypeDto.Recipient:
                    var recipientInvoiceIds = _unitOfWork.Repository<Importer>().GetAll(x => x.Status == EImporterStatus.Recipient)
                                                                                .Select(x => x.InvoiceId);
                    query = query.Where(x => x.Importer != null && x.Importer.Status == EImporterStatus.Recipient);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (!string.IsNullOrWhiteSpace(dto.Voen) && dto.SubjectType != ESubjectTypeDto.None)
        {
            switch (dto.SubjectType)
            {
                case ESubjectTypeDto.Exporter:
                    query = query.Where(x => x.Exporter.Voen.Contains(dto.Voen));
                    break;

                case ESubjectTypeDto.Importer:
                    query = query.Where(x => x.Importer.Voen.Contains(dto.Voen));
                    break;

                case ESubjectTypeDto.Sender:
                    var senderIds = _unitOfWork.Repository<Exporter>().GetAll(x => x.Status == EExporterStatus.Sender && x.Voen.Contains(dto.Voen))
                                                                             .Select(x => x.InvoiceId);
                    query = query.Where(x => senderIds.Contains(x.Id));
                    break;

                case ESubjectTypeDto.Recipient:
                    var recipientIds = _unitOfWork.Repository<Importer>().GetAll(x => x.Status == EImporterStatus.Recipient && x.Voen.Contains(dto.Voen))
                                                                         .Select(x => x.InvoiceId);
                    query = query.Where(x => recipientIds.Contains(x.Id));
                    break;  
            }
        }

        if (!string.IsNullOrWhiteSpace(dto.Name) && dto.SubjectType != ESubjectTypeDto.None)
        {
            switch (dto.SubjectType)
            {
                case ESubjectTypeDto.Exporter:
                    query = query.Where(x => x.Exporter.Name.Contains(dto.Name));
                    break;

                case ESubjectTypeDto.Importer:
                    query = query.Where(x => x.Importer.Name.Contains(dto.Name));
                    break;

                case ESubjectTypeDto.Sender:
                    var senderIds = _unitOfWork.Repository<Exporter>().GetAll(x => x.Status == EExporterStatus.Sender && x.Name.Contains(dto.Name))
                                                                      .Select(x => x.InvoiceId);
                    query = query.Where(x => senderIds.Contains(x.Id));
                    break;
                    

                case ESubjectTypeDto.Recipient:
                    var recipientIds = _unitOfWork.Repository<Importer>().GetAll(x => x.Status == EImporterStatus.Recipient && x.Name.Contains(dto.Name))
                                                                         .Select(x => x.InvoiceId);
                    query = query.Where(x => recipientIds.Contains(x.Id));
                    break;  
            }
        }

        int pageNumber = dto.PageNumber <= 0 ? 1 : dto.PageNumber;
        int pageSize = dto.PageSize <= 0 ? 10 : dto.PageSize;
        var pagedResult = await query.ToPagedResultAsync(pageNumber, pageSize, cancellationToken);

        var dtoItems = pagedResult.Items.Select(invoice => new InvoiceResponseDTO
        {
            PinCode = invoice.PinCode,
            CreateTime = invoice.CreateTime,
            InvoiceNumber = invoice.InvoiceRequisites.InvoiceNumber,                                 
            InvoiceDate = invoice.InvoiceRequisites.InvoiceDate,                                     
            ExporterName = invoice.Exporter.Name,                                                    
            ImporterName = invoice.Importer.Name,                                                    
            GoodsCount = invoice.Goods.Count().ToString(),                                          
            TotalAmount = invoice.Goods.Sum(g => g.TotalAmount).ToString(),                          
            Status = invoice.Status,                                                                
            FieldValues = invoice.InvoiceFieldValues?.Select(fv => new InvoiceFieldValueResponseDTO
            {
                InvoiceFieldDefinitionId = fv.InvoiceFieldDefinitionId,
                FieldDefinitionName = fv.InvoiceFieldDefinition?.Label ?? string.Empty,         
                Value = fv.Value ?? string.Empty                                                
            }).ToList() ?? new List<InvoiceFieldValueResponseDTO>()
        }).ToList();

        return new PagedResult<InvoiceResponseDTO>
        {
            Items = dtoItems,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalCount = pagedResult.TotalCount
        };
    }


    // Admin Operations
    public async Task CreateAsync(InvoiceCreateRequestDTO dto, CancellationToken cancellationToken = default)
    {
        if (dto == null) 
            throw new ArgumentNullException(nameof(dto));

        var filePath = await _fileService.UploadFileAsync(dto.File, cancellationToken);


        // InvoiceRequisites yaradilmasi
        var invoiceRequisites = new InvoiceRequisites
        {
            InvoiceNumber = dto.InvoiceRequisites.InvoiceNumber,
            InvoiceDate = dto.InvoiceRequisites.InvoiceDate,
            CurrencyCode = dto.InvoiceRequisites.CurrencyCode,
            ShortDeclarationNumber = dto.InvoiceRequisites.ShortDeclarationNumber ?? string.Empty,
            Note = dto.InvoiceRequisites.Note ?? string.Empty,
            ContractNumberAndDate = dto.InvoiceRequisites.ContractNumberAndDate ?? string.Empty,
            TransportConditions = dto.InvoiceRequisites.TransportConditions,
            InvoicePurpose = dto.InvoiceRequisites.InvoicePurpose,
            PaymentConditions = dto.InvoiceRequisites.PaymentConditions
        };


        // Exporter ve Sender yaradilmasi
        var exporter = new Exporter
        {
            Voen = dto.Exporter.Voen ?? string.Empty,
            Name = dto.Exporter.Name,
            Address = new Address
            {
                StreetAndNumber = dto.Exporter.Address.StreetAndNumber,
                PostalCode = dto.Exporter.Address.PostalCode,
                City = dto.Exporter.Address.City,
                Country = dto.Exporter.Address.Country
            },
             Status = EExporterStatus.Exporter
        };

        if (dto.Exporter.IsExporterDifferentFromSender)
        {
            // Eğer `IsExporterDifferentFromSender` true olarsa, yeni bir Sender yaradılır
            var sender = new Exporter
            {
                Voen = dto.Exporter.Voen ?? string.Empty,
                Name = dto.Exporter.Name,
                Address = new Address
                {
                    StreetAndNumber = dto.Exporter.Address.StreetAndNumber,
                    PostalCode = dto.Exporter.Address.PostalCode,
                    City = dto.Exporter.Address.City,
                    Country = dto.Exporter.Address.Country
                },
                Status = EExporterStatus.Sender  
            };
            _unitOfWork.Repository<Exporter>().Create(sender);
        }


        // Importer ve Recipient yaradilmasi
        var importer = new Importer
        {
            Voen = dto.Importer.Voen,
            Name = dto.Importer.Name,
            Address = new Address
            {
                StreetAndNumber = dto.Importer.Address.StreetAndNumber,
                PostalCode = dto.Importer.Address.PostalCode,
                City = dto.Importer.Address.City,
                Country = dto.Importer.Address.Country
            },
            Status = EImporterStatus.Importer
        };
     
        if (dto.Importer.IsImporterDifferentFromRecipient)
        {
            var recipient = new Importer
            {
                Voen = dto.Importer.Voen,
                Name = dto.Importer.Name,
                Address = new Address
                {
                    StreetAndNumber = dto.Importer.Address.StreetAndNumber,
                    PostalCode = dto.Importer.Address.PostalCode,
                    City = dto.Importer.Address.City,
                    Country = dto.Importer.Address.Country
                },
                Status = EImporterStatus.Recipient
            };
            _unitOfWork.Repository<Importer>().Create(recipient);
        }


        // CircumstancesAffectingInvoice yaradilmasi
        CircumstancesAffectingInvoice? circumstancesAffectingInvoice = null;
        if (dto.CircumstancesAffectingInvoice.IsCircumstancesAffectingInvoice)
        {
            circumstancesAffectingInvoice = new CircumstancesAffectingInvoice
            {
                DegreeInfluenceInvoice = dto.CircumstancesAffectingInvoice.DegreeInfluenceInvoice,
                TypeFunds = dto.CircumstancesAffectingInvoice.TypeFunds,
                Explanation = dto.CircumstancesAffectingInvoice.Explanation,
                AmountFunds = dto.CircumstancesAffectingInvoice.AmountFunds
            };
        }


        // Invoice yaradılması
        var invoice = new Invoice
        {
            Importer = importer,
            Exporter = exporter,
            InvoiceRequisites = invoiceRequisites,
            CircumstancesAffectingInvoice = circumstancesAffectingInvoice ?? null,
            Status = EInvoiceStatus.Pending,
            PinCode = dto.PinCode,
            FilePathUrl = filePath
        };

        _unitOfWork.Repository<Invoice>().Create(invoice);

        //if (dto.Goods != null && dto.Goods.Any())
        //{
        //    foreach (var goodDto in dto.Goods)
        //    {
        //        var good = new Good
        //        {
        //            InvoiceId = invoice.Id,
        //            // GoodDto-ya uyğun olan Good məlumatları
        //        };

        //        _unitOfWork.Repository<Good>().Create(good);
        //    }
        //}

        if (dto.FieldValues != null)
        {
            foreach (var fieldValue in dto.FieldValues)
            {
                var invoiceFieldValue = new InvoiceFieldValue
                {
                    Value = fieldValue.Value,
                    InvoiceId = invoice.Id,
                    InvoiceFieldDefinitionId = fieldValue.InvoiceFieldDefinitionId
                };

                _unitOfWork.Repository<InvoiceFieldValue>().Create(invoiceFieldValue);
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task UpdateAsync(Guid id, InvoiceUpdateRequestDTO dto, CancellationToken cancellationToken = default)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsync(id, cancellationToken);

        if (invoice == null)
            throw new KeyNotFoundException($"Invoice with id {id} not found.");


        // InvoiceRequisites yenilənməsi
        invoice.InvoiceRequisites.InvoiceNumber = dto.InvoiceRequisites.InvoiceNumber ?? string.Empty;
        invoice.InvoiceRequisites.InvoiceDate = dto.InvoiceRequisites.InvoiceDate;
        invoice.InvoiceRequisites.CurrencyCode = dto.InvoiceRequisites.CurrencyCode;
        invoice.InvoiceRequisites.ShortDeclarationNumber = dto.InvoiceRequisites.ShortDeclarationNumber ?? string.Empty;
        invoice.InvoiceRequisites.Note = dto.InvoiceRequisites.Note ?? string.Empty;
        invoice.InvoiceRequisites.ContractNumberAndDate = dto.InvoiceRequisites.ContractNumberAndDate ?? string.Empty;
        invoice.InvoiceRequisites.TransportConditions = dto.InvoiceRequisites.TransportConditions;
        invoice.InvoiceRequisites.InvoicePurpose = dto.InvoiceRequisites.InvoicePurpose;
        invoice.InvoiceRequisites.PaymentConditions = dto.InvoiceRequisites.PaymentConditions;


        // Exporter və Sender yenilənməsi
        invoice.Exporter.Voen = dto.Exporter.Voen ?? string.Empty;
        invoice.Exporter.Name = dto.Exporter.Name;
        invoice.Exporter.Address.StreetAndNumber = dto.Exporter.Address.StreetAndNumber;
        invoice.Exporter.Address.PostalCode = dto.Exporter.Address.PostalCode;
        invoice.Exporter.Address.City = dto.Exporter.Address.City;
        invoice.Exporter.Address.Country = dto.Exporter.Address.Country;

        var sender = await _unitOfWork.Repository<Exporter>().GetAsync(x => x.InvoiceId == invoice.Id && x.Status == EExporterStatus.Sender,
                                                                       tracking: true,
                                                                       cancellationToken: cancellationToken);

        if (sender == null)
        {
            sender = new Exporter
            {
                InvoiceId = invoice.Id,
                Voen = dto.Exporter.Voen ?? string.Empty,
                Name = dto.Exporter.Name,
                Address = new Address
                {
                    StreetAndNumber = dto.Exporter.Address.StreetAndNumber,
                    PostalCode = dto.Exporter.Address.PostalCode,
                    City = dto.Exporter.Address.City,
                    Country = dto.Exporter.Address.Country
                },
                Status = EExporterStatus.Sender
            };

            _unitOfWork.Repository<Exporter>().Create(sender);
        }
        else
        {
            sender.Voen = dto.Exporter.Voen ?? string.Empty;
            sender.Name = dto.Exporter.Name;
            sender.Address.StreetAndNumber = dto.Exporter.Address.StreetAndNumber;
            sender.Address.PostalCode = dto.Exporter.Address.PostalCode;
            sender.Address.City = dto.Exporter.Address.City;
            sender.Address.Country = dto.Exporter.Address.Country;
        }


        // Importer və Recipient yenilənməsi
        invoice.Importer.Voen = dto.Importer.Voen;
        invoice.Importer.Name = dto.Importer.Name;
        invoice.Importer.Address.StreetAndNumber = dto.Importer.Address.StreetAndNumber;
        invoice.Importer.Address.PostalCode = dto.Importer.Address.PostalCode;
        invoice.Importer.Address.City = dto.Importer.Address.City;
        invoice.Importer.Address.Country = dto.Importer.Address.Country;

        var recipient = await _unitOfWork.Repository<Importer>().GetAsync(x => x.InvoiceId == invoice.Id && x.Status == EImporterStatus.Recipient,
                                                                          tracking: true,
                                                                          cancellationToken: cancellationToken);
        if (recipient == null)
        {
            recipient = new Importer
            {
                InvoiceId = invoice.Id,
                Voen = dto.Importer.Voen,
                Name = dto.Importer.Name,
                Address = new Address
                {
                    StreetAndNumber = dto.Importer.Address.StreetAndNumber,
                    PostalCode = dto.Importer.Address.PostalCode,
                    City = dto.Importer.Address.City,
                    Country = dto.Importer.Address.Country
                },
                Status = EImporterStatus.Recipient
            };

            _unitOfWork.Repository<Importer>().Create(recipient);
        }
        else
        {
            recipient.Voen = dto.Importer.Voen;
            recipient.Name = dto.Importer.Name;
            recipient.Address.StreetAndNumber = dto.Importer.Address.StreetAndNumber;
            recipient.Address.PostalCode = dto.Importer.Address.PostalCode;
            recipient.Address.City = dto.Importer.Address.City;
            recipient.Address.Country = dto.Importer.Address.Country;
        }


        // CircumstancesAffectingInvoice yenilənməsi
        CircumstancesAffectingInvoice? circumstancesAffectingInvoice = null;
        if (dto.CircumstancesAffectingInvoice.IsCircumstancesAffectingInvoice)
        {
            circumstancesAffectingInvoice = new CircumstancesAffectingInvoice
            {
                DegreeInfluenceInvoice = dto.CircumstancesAffectingInvoice.DegreeInfluenceInvoice,
                TypeFunds = dto.CircumstancesAffectingInvoice.TypeFunds,
                Explanation = dto.CircumstancesAffectingInvoice.Explanation,
                AmountFunds = dto.CircumstancesAffectingInvoice.AmountFunds
            };
        }
        invoice.CircumstancesAffectingInvoice = circumstancesAffectingInvoice;

        // Faylın yenilənməsi 
        if (dto.File != null)
        {
            if (await _fileService.FileExistsAsync(invoice.FilePathUrl))
            {
                await _fileService.DeleteFileAsync(invoice.FilePathUrl);
            }

            var filePath = await _fileService.UploadFileAsync(dto.File, cancellationToken);
            invoice.FilePathUrl = filePath;
        }

        // Faylın yenilənməsi (əgər varsa)
        if (dto.FieldValues != null)
        {
            invoice.InvoiceFieldValues = dto.FieldValues.Select(fv => new InvoiceFieldValue
            {
                Value = fv.Value,
                InvoiceId = invoice.Id,
                InvoiceFieldDefinitionId = fv.InvoiceFieldDefinitionId
            }).ToList();
        }

        invoice.UpdateTime = DateTime.UtcNow;
        _unitOfWork.Repository<Invoice>().Update(invoice);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task<InvoiceUpdateResponseDTO> UpdateResponseAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsync(id, cancellationToken);

        if (invoice == null)
            throw new KeyNotFoundException($"Invoice with id {id} not found.");

        var historyQuery = _unitOfWork.Repository<InvoiceUpdateHistory>()
            .GetAll(x => x.InvoiceId == id);

        var lastOrder = historyQuery.Any() ? historyQuery.Max(x => x.Order) : 0;

        var newOrder = lastOrder + 1;

        invoice.UpdateTime = DateTime.UtcNow;

        var updateHistory = new InvoiceUpdateHistory
        {
            InvoiceId = invoice.Id,
            Order = newOrder,
            UpdateTime = invoice.UpdateTime,
            PinCode = invoice.PinCode,
            StatusUpdate = EUpdateStatus.HasBeenCorrected,
            Note = invoice.InvoiceRequisites?.Note ?? string.Empty
        };

        _unitOfWork.Repository<Invoice>().Update(invoice);
        _unitOfWork.Repository<InvoiceUpdateHistory>().Create(updateHistory);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new InvoiceUpdateResponseDTO
        {
            Order = newOrder,
            UpdateTime = updateHistory.UpdateTime,
            PinCode = updateHistory.PinCode,
            StatusUpdate = updateHistory.StatusUpdate,
            Note = updateHistory.Note
        };
    }


    public async Task<PagedResult<InvoiceAdminResponseDTO>> GetAllAsync(InvoiceQueryDTO dto, CancellationToken cancellationToken = default)
    {
        IQueryable<Invoice> query = _unitOfWork.Repository<Invoice>().GetAll(includes: x => x.Include(invoice => invoice.Importer)
                                                                                             .Include(invoice => invoice.Exporter)
                                                                                             .Include(invoice => invoice.InvoiceRequisites)
                                                                                             .Include(invoice => invoice.CircumstancesAffectingInvoice)
                                                                                             .Include(invoice => invoice.Goods)
                                                                                             .Include(invoice => invoice.InvoiceFieldValues)
                                                                                             .ThenInclude(fv => fv.InvoiceFieldDefinition));

        if (dto is null)
            throw new ArgumentNullException(nameof(dto));

        if (dto.CreatedFrom.HasValue) 
            query = query.Where(x => x.CreateTime >= dto.CreatedFrom.Value);

        if (dto.CreatedTo.HasValue)
            query = query.Where(x => x.CreateTime <= dto.CreatedTo.Value);

        if (!string.IsNullOrEmpty(dto.InvoiceNo))
            query = query.Where(x => x.InvoiceRequisites != null &&
                                     x.InvoiceRequisites.InvoiceNumber.Contains(dto.InvoiceNo));

        if (!string.IsNullOrEmpty(dto.PinCode))
            query = query.Where(x => x.PinCode.Contains(dto.PinCode));

        if (!string.IsNullOrEmpty(dto.NumberOfShortDeclaration))
            query = query.Where(x => x.InvoiceRequisites != null &&
                                     x.InvoiceRequisites.ShortDeclarationNumber.Contains(dto.NumberOfShortDeclaration));

        if (dto.InvoiceDate != default)
            query = query.Where(x => x.InvoiceRequisites != null &&
                                     x.InvoiceRequisites.InvoiceDate == dto.InvoiceDate.Date);

        if (dto.SubjectType != ESubjectTypeDto.None)
        {
            switch (dto.SubjectType)
            {
                case ESubjectTypeDto.Exporter:
                    query = query.Where(x => x.Exporter != null && x.Exporter.Status == EExporterStatus.Exporter);
                    break;

                case ESubjectTypeDto.Importer:
                    query = query.Where(x => x.Importer != null && x.Importer.Status == EImporterStatus.Importer);
                    break;

                case ESubjectTypeDto.Sender:
                    var senderInvoiceIds = _unitOfWork.Repository<Exporter>().GetAll(x => x.Status == EExporterStatus.Sender)
                                                                             .Select(x => x.InvoiceId);

                    query = query.Where(x => senderInvoiceIds.Contains(x.Id));
                    break;

                case ESubjectTypeDto.Recipient:
                    var recipientInvoiceIds = _unitOfWork.Repository<Importer>().GetAll(x => x.Status == EImporterStatus.Recipient)
                                                                                .Select(x => x.InvoiceId);

                    query = query.Where(x => x.Importer != null && x.Importer.Status == EImporterStatus.Recipient);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        if (!string.IsNullOrWhiteSpace(dto.Voen) && dto.SubjectType != ESubjectTypeDto.None)
        {
            switch (dto.SubjectType)
            {
                case ESubjectTypeDto.Exporter:
                    query = query.Where(x => x.Exporter.Voen.Contains(dto.Voen));
                    break;

                case ESubjectTypeDto.Importer:
                    query = query.Where(x => x.Importer.Voen.Contains(dto.Voen));
                    break;

                case ESubjectTypeDto.Sender:
                    {
                        var senderInvoiceIds = _unitOfWork.Repository<Exporter>()
                            .GetAll(x => x.Status == EExporterStatus.Sender && x.Voen.Contains(dto.Voen))
                            .Select(x => x.InvoiceId);

                        query = query.Where(x => senderInvoiceIds.Contains(x.Id));
                        break;
                    }

                case ESubjectTypeDto.Recipient:
                    {
                        var recipientInvoiceIds = _unitOfWork.Repository<Importer>()
                            .GetAll(x => x.Status == EImporterStatus.Recipient && x.Voen.Contains(dto.Voen))
                            .Select(x => x.InvoiceId);

                        query = query.Where(x => recipientInvoiceIds.Contains(x.Id));
                        break;
                    }
            }
        }

        if (!string.IsNullOrWhiteSpace(dto.Name) && dto.SubjectType != ESubjectTypeDto.None)
        {
            switch (dto.SubjectType)
            {
                case ESubjectTypeDto.Exporter:
                    query = query.Where(x => x.Exporter.Name.Contains(dto.Name));
                    break;

                case ESubjectTypeDto.Importer:
                    query = query.Where(x => x.Importer.Name.Contains(dto.Name));
                    break;

                case ESubjectTypeDto.Sender:
                    {
                        var senderInvoiceIds = _unitOfWork.Repository<Exporter>()
                            .GetAll(x => x.Status == EExporterStatus.Sender && x.Name.Contains(dto.Name))
                            .Select(x => x.InvoiceId);

                        query = query.Where(x => senderInvoiceIds.Contains(x.Id));
                        break;
                    }

                case ESubjectTypeDto.Recipient:
                    {
                        var recipientInvoiceIds = _unitOfWork.Repository<Importer>()
                            .GetAll(x => x.Status == EImporterStatus.Recipient && x.Name.Contains(dto.Name))
                            .Select(x => x.InvoiceId);

                        query = query.Where(x => recipientInvoiceIds.Contains(x.Id));
                        break;
                    }
            }
        }

        int pageNumber = dto.PageNumber <= 0 ? 1 : dto.PageNumber;
        int pageSize = dto.PageSize <= 0 ? 10 : dto.PageSize;
        var pagedResult = await query.ToPagedResultAsync(pageNumber, pageSize, cancellationToken);

        var invoiceIds = pagedResult.Items.Select(x => x.Id).ToList();

        var senders = await _unitOfWork.Repository<Exporter>().GetAll(x => invoiceIds.Contains(x.InvoiceId) && x.Status == EExporterStatus.Sender)
                                                              .ToListAsync(cancellationToken);

        var recipients = await _unitOfWork.Repository<Importer>().GetAll(x => invoiceIds.Contains(x.InvoiceId) && x.Status == EImporterStatus.Recipient)
                                                                 .ToListAsync(cancellationToken);

        var senderDict = senders.ToDictionary(x => x.InvoiceId, x => x);
        var recipientDict = recipients.ToDictionary(x => x.InvoiceId, x => x);

        var dtoItems = pagedResult.Items.Select(invoice =>
        {
            senderDict.TryGetValue(invoice.Id, out var sender);
            recipientDict.TryGetValue(invoice.Id, out var recipient);

            return new InvoiceAdminResponseDTO
            {
                PinCode = invoice.PinCode,
                Status = invoice.Status,

                ExporterVoen = invoice.Exporter.Voen,
                ExporterName = invoice.Exporter.Name,
                ExporterAddress = new AddressDto
                {
                    StreetAndNumber = invoice.Exporter.Address.StreetAndNumber,
                    PostalCode = invoice.Exporter.Address.PostalCode,
                    City = invoice.Exporter.Address.City,
                    Country = invoice.Exporter.Address.Country
                },
                ExporterStatus = invoice.Exporter.Status,

                SenderVoen = sender?.Voen,
                SenderName = sender?.Name ?? string.Empty,
                SenderAddress = sender == null ? null : new AddressDto
                {
                    StreetAndNumber = sender.Address.StreetAndNumber,
                    PostalCode = sender.Address.PostalCode,
                    City = sender.Address.City,
                    Country = sender.Address.Country
                },

                ImporterVoen = invoice.Importer.Voen,
                ImporterName = invoice.Importer.Name,
                ImporterAddress = new AddressDto
                {
                    StreetAndNumber = invoice.Importer.Address.StreetAndNumber,
                    PostalCode = invoice.Importer.Address.PostalCode,
                    City = invoice.Importer.Address.City,
                    Country = invoice.Importer.Address.Country
                },
                ImporterStatus = invoice.Importer.Status,

                RecipientVoen = recipient?.Voen,
                RecipientName = recipient?.Name ?? string.Empty,
                RecipientAddress = recipient == null ? null : new AddressDto
                {
                    StreetAndNumber = recipient.Address.StreetAndNumber,
                    PostalCode = recipient.Address.PostalCode,
                    City = recipient.Address.City,
                    Country = recipient.Address.Country
                },

                InvoiceNumber = invoice.InvoiceRequisites.InvoiceNumber,
                InvoiceDate = invoice.InvoiceRequisites.InvoiceDate,
                CurrencyCode = invoice.InvoiceRequisites.CurrencyCode,
                ShortDeclarationNumber = invoice.InvoiceRequisites.ShortDeclarationNumber,
                Note = invoice.InvoiceRequisites.Note,
                ContractNumberAndDate = invoice.InvoiceRequisites.ContractNumberAndDate,
                TransportConditions = invoice.InvoiceRequisites.TransportConditions,
                InvoicePurpose = invoice.InvoiceRequisites.InvoicePurpose,
                PaymentConditions = invoice.InvoiceRequisites.PaymentConditions,

                DegreeInfluenceInvoice = invoice.CircumstancesAffectingInvoice?.DegreeInfluenceInvoice ?? EDegreeInfluenceInvoice.None,
                TypeFunds = invoice.CircumstancesAffectingInvoice?.TypeFunds ?? ETypeFunds.None,
                Explanation = invoice.CircumstancesAffectingInvoice?.Explanation ?? string.Empty,
                AmountFunds = invoice.CircumstancesAffectingInvoice?.AmountFunds ?? 0,

                FileUrl = invoice.FilePathUrl,

                Goods = invoice.Goods.Select(g => new GoodAdminResponseDto
                {
                    GoodCode = g.GoodCode,
                    Price = g.Price,
                    Quantity = g.Quantity,
                    TotalAmount = g.TotalAmount,
                    IsDeleted = g.IsDeleted,
                    DeleteTime = g.DeletedTime
                }).ToList(),

                FieldValues = invoice.InvoiceFieldValues.Select(fv => new InvoiceFieldValueResponseDTO
                {
                    InvoiceFieldDefinitionId = fv.InvoiceFieldDefinitionId,
                    FieldDefinitionName = fv.InvoiceFieldDefinition?.Label ?? string.Empty,
                    Value = fv.Value ?? string.Empty
                }).ToList(),

                CreateTime = invoice.CreateTime,
                UpdateTime = invoice.UpdateTime,
                DeleteTime = invoice.DeletedTime,
                IsDeleted = invoice.IsDeleted,
                IsActive = invoice.IsActive
            };
        }).ToList();

        return new PagedResult<InvoiceAdminResponseDTO>
        {
            Items = dtoItems,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalCount = pagedResult.TotalCount
        };
    }


    public async Task<InvoiceAdminResponseDTO> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var invoice = await _unitOfWork.Repository<Invoice>().GetAsync(x => x.Id == id, includes: q => q.Include(invoice => invoice.Importer)
                                                                                            .Include(invoice => invoice.Exporter)
                                                                                            .Include(invoice => invoice.InvoiceRequisites)
                                                                                            .Include(invoice => invoice.CircumstancesAffectingInvoice)
                                                                                            .Include(invoice => invoice.Goods)
                                                                                            .Include(invoice => invoice.InvoiceFieldValues)
                                                                                            .ThenInclude(fv => fv.InvoiceFieldDefinition));
        if (invoice is null)
            throw new KeyNotFoundException($"Invoice with id {id} not found.");
        
        var sender = await _unitOfWork.Repository<Exporter>().GetAsync(x => x.InvoiceId == id && x.Status == EExporterStatus.Sender,
                                                                       tracking: false,
                                                                       cancellationToken: cancellationToken);

        var recipient = await _unitOfWork.Repository<Importer>().GetAsync(x => x.InvoiceId == id && x.Status == EImporterStatus.Recipient,
                                                                          tracking: false,
                                                                          cancellationToken: cancellationToken);
        return new InvoiceAdminResponseDTO
        {
            PinCode = invoice.PinCode,
            Status = invoice.Status,

            ExporterVoen = invoice.Exporter.Voen,
            ExporterName = invoice.Exporter.Name,
            ExporterAddress = new AddressDto
            {
                StreetAndNumber = invoice.Exporter.Address.StreetAndNumber,
                PostalCode = invoice.Exporter.Address.PostalCode,
                City = invoice.Exporter.Address.City,
                Country = invoice.Exporter.Address.Country
            },
            SenderVoen = sender?.Voen,
            SenderName = sender?.Name ?? string.Empty,
            SenderAddress = sender == null ? null : new AddressDto
            {
                StreetAndNumber = sender.Address.StreetAndNumber,
                PostalCode = sender.Address.PostalCode,
                City = sender.Address.City,
                Country = sender.Address.Country
            },
            ImporterVoen = invoice.Importer.Voen,
            ImporterName = invoice.Importer.Name,
            ImporterAddress = new AddressDto
            {
                StreetAndNumber = invoice.Importer.Address.StreetAndNumber,
                PostalCode = invoice.Importer.Address.PostalCode,
                City = invoice.Importer.Address.City,
                Country = invoice.Importer.Address.Country
            },
            RecipientVoen = recipient?.Voen,
            RecipientName = recipient?.Name ?? string.Empty,
            RecipientAddress = recipient == null ? null : new AddressDto
            {
                StreetAndNumber = recipient.Address.StreetAndNumber,
                PostalCode = recipient.Address.PostalCode,
                City = recipient.Address.City,
                Country = recipient.Address.Country
            },

            // InvoiceRequisites melumatlari
            InvoiceNumber = invoice.InvoiceRequisites.InvoiceNumber,
            InvoiceDate = invoice.InvoiceRequisites.InvoiceDate,
            CurrencyCode = invoice.InvoiceRequisites.CurrencyCode,
            ShortDeclarationNumber = invoice.InvoiceRequisites.ShortDeclarationNumber,
            Note = invoice.InvoiceRequisites.Note,
            ContractNumberAndDate = invoice.InvoiceRequisites.ContractNumberAndDate,
            TransportConditions = invoice.InvoiceRequisites.TransportConditions,
            InvoicePurpose = invoice.InvoiceRequisites.InvoicePurpose,
            PaymentConditions = invoice.InvoiceRequisites.PaymentConditions,
     
            // CircumstancesAffectingInvoice məlumatları
            DegreeInfluenceInvoice = invoice.CircumstancesAffectingInvoice?.DegreeInfluenceInvoice ?? EDegreeInfluenceInvoice.None,
            TypeFunds = invoice.CircumstancesAffectingInvoice?.TypeFunds ?? ETypeFunds.None,
            Explanation = invoice.CircumstancesAffectingInvoice?.Explanation ?? string.Empty,
            AmountFunds = invoice.CircumstancesAffectingInvoice?.AmountFunds ?? 0,

            // File
            FileUrl = invoice.FilePathUrl,

            // Goods məlumatları
            Goods = invoice.Goods?.Select(g => new GoodAdminResponseDto
            {
                GoodCode = g.GoodCode,
                Price = g.Price,
                Quantity = g.Quantity,
                TotalAmount = g.TotalAmount,
                IsDeleted = g.IsDeleted,
                DeleteTime = g.DeletedTime ?? default
            }).ToList() ?? new List<GoodAdminResponseDto>(),

            // InvoiceFieldValues məlumatları
            FieldValues = invoice.InvoiceFieldValues?.Select(fv => new InvoiceFieldValueResponseDTO
            {
                InvoiceFieldDefinitionId = fv.InvoiceFieldDefinitionId,
                FieldDefinitionName = fv.InvoiceFieldDefinition?.Label ?? string.Empty,
                Value = fv.Value ?? string.Empty
            }).ToList() ?? new List<InvoiceFieldValueResponseDTO>(),

            // UpdateTime, CreateTime və digər məlumatlar
            CreateTime = invoice.CreateTime,
            UpdateTime = invoice.UpdateTime,
            DeleteTime = invoice.DeletedTime,
            IsDeleted = invoice.IsDeleted,
            IsActive = invoice.IsActive
        };
    }


    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsync(id);

        if (invoice is null)
            throw new KeyNotFoundException($"Invoice with id {id} not found.");


        var goods = _unitOfWork.Repository<Good>().GetAll(x => x.InvoiceId == id, tracking: true).ToList();
        foreach (var good in goods)
            _unitOfWork.Repository<Good>().Delete(good);


        var fieldValues = _unitOfWork.Repository<InvoiceFieldValue>().GetAll(x => x.InvoiceId == id, tracking: true).ToList();
        foreach (var fieldValue in fieldValues)
            _unitOfWork.Repository<InvoiceFieldValue>().Delete(fieldValue);


        var sendersAndExporters = _unitOfWork.Repository<Exporter>().GetAll(x => x.InvoiceId == id, tracking: true).ToList();
        foreach (var exporter in sendersAndExporters)
            _unitOfWork.Repository<Exporter>().Delete(exporter);


        var recipientsAndImporters = _unitOfWork.Repository<Importer>().GetAll(x => x.InvoiceId == id, tracking: true).ToList();
        foreach (var importer in recipientsAndImporters)
            _unitOfWork.Repository<Importer>().Delete(importer);


        if (!string.IsNullOrWhiteSpace(invoice.FilePathUrl) && await _fileService.FileExistsAsync(invoice.FilePathUrl))
        {
            await _fileService.DeleteFileAsync(invoice.FilePathUrl);
        }

        _unitOfWork.Repository<Invoice>().Delete(invoice);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsync(id);

        if (invoice is null)
            throw new KeyNotFoundException($"Invoice with id {id} not found.");

        if (invoice.IsDeleted)
        {
            throw new Exception($"Invoice with id {id} is already soft deleted.");
        }
        else
        {
            invoice.IsDeleted = true;
            invoice.IsActive = false;
            invoice.DeletedTime = DateTime.UtcNow;
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task RecoverAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsync(id);

        if (invoice is null)
            throw new KeyNotFoundException($"Invoice with id {id} not found.");

        if (invoice.IsDeleted)
        {
            invoice.IsDeleted = false;
            invoice.DeletedTime = null;
        }
        else
        {
            throw new Exception($"Invoice with id {id} is not soft deleted, so it cannot be recovered.");
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task ActivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsync(id);

        if (invoice is null)
            throw new KeyNotFoundException($"Invoice with id {id} not found.");

        if (invoice.IsDeleted)
            throw new Exception($"Firstful Recover Invoice with id {id}.");

        if (invoice.IsActive)
        {
            throw new Exception($"Invoice with id {id} is already activated.");
        }
        else
        {
            invoice.IsActive = true;
            invoice.UpdateTime = DateTime.UtcNow;
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task DeactivateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsync(id);

        if (invoice is null)
            throw new KeyNotFoundException($"Invoice with id {id} not found.");

        if (!invoice.IsActive)
        {
            throw new Exception($"Invoice with id {id} is already deactivated.");
        }
        else
        {
            invoice.IsActive = false;
            invoice.UpdateTime = DateTime.UtcNow;
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }


    public async Task ChangeStatusAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var invoice = await _unitOfWork.Repository<Invoice>().GetByIdAsync(id, cancellationToken);

        if (invoice is null)
            throw new KeyNotFoundException($"Invoice with id {id} not found.");

        if (invoice.IsDeleted)
            throw new InvalidOperationException($"Invoice with id {id} is deleted.");

        if (invoice.Status == EInvoiceStatus.Agreed)
            throw new InvalidOperationException($"Invoice with id {id} is already agreed.");

        invoice.Status = EInvoiceStatus.Agreed;
        invoice.UpdateTime = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}