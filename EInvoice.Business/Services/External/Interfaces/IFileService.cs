using Microsoft.AspNetCore.Http;
namespace EInvoice.Business.Services.External.Interfaces;

public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file, CancellationToken ct = default);
    Task<IEnumerable<string>> UploadFilesAsync(IEnumerable<IFormFile> files, CancellationToken ct = default);
    Task<bool> DeleteFileAsync(string fileUrlOrPath);
    Task<bool> FileExistsAsync(string fileUrlOrPath);               //faylın mövcud olub-olmaması ilə bağlı yoxlama
}