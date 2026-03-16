using EInvoice.Business.Services.External.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace EInvoice.Business.Services.External.Implements;

public class FileService(IWebHostEnvironment environment) : IFileService
{
    private readonly string _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "documents");

    private const long MaxFileSize = 5 * 1024 * 1024;         // 5Mb saxlayir
    private static readonly string[] AllowedExtensions = [".pdf"];

    private const string PublicBaseUrl = "https://localhost:5001/uploads/documents";

    public async Task<string> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        await ValidateFileAsync(file, cancellationToken);

        if (!Directory.Exists(_uploadFolder))
            Directory.CreateDirectory(_uploadFolder);
         
        var uniqueFileName = $"{Guid.NewGuid()}.pdf";         // PDF saxlayırıq
        var filePath = Path.Combine(_uploadFolder, uniqueFileName);

        await using var stream = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        await file.CopyToAsync(stream, cancellationToken);    // Fayl diskə yazılır

        return $"{PublicBaseUrl}/{uniqueFileName}";
    }

    public async Task<IEnumerable<string>> UploadFilesAsync(IEnumerable<IFormFile> files, CancellationToken cancellationToken = default)
    {
        var uploadedPaths = new List<string>();

        foreach (var file in files)
        {
            var path = await UploadFileAsync(file, cancellationToken);
            uploadedPaths.Add(path);
        }

        return uploadedPaths;
    }

    public Task<bool> DeleteFileAsync(string fileUrlOrPath)
    {
        try
        {
            var fileName = Path.GetFileName(fileUrlOrPath);
            var physicalPath = Path.Combine(_uploadFolder, fileName);

            if (!File.Exists(physicalPath))
                return Task.FromResult(false);

            File.Delete(physicalPath);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }

    public Task<bool> FileExistsAsync(string fileUrlOrPath)
    {
        var fileName = Path.GetFileName(fileUrlOrPath);
        var physicalPath = Path.Combine(_uploadFolder, fileName);    // Faylın tam yolunu birləşdirir : C:\Uploads\documents\file.pdf
        return Task.FromResult(File.Exists(physicalPath));           //faylın sistemdə mövcud olmasını yoxlayır
    }




    // Private validation
    private async Task ValidateFileAsync(IFormFile file, CancellationToken ct)
    {
        if (file is null)
            throw new ArgumentNullException(nameof(file));

        if (file.Length <= 0)
            throw new InvalidOperationException("The file is empty.");

        if (file.Length > MaxFileSize)
            throw new InvalidOperationException($"File size limit exceeded. Max: {MaxFileSize / (1024 * 1024)}MB");

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();      //faylın uzantısını alır
        if (!AllowedExtensions.Contains(ext))
            throw new InvalidOperationException("Only PDF files are accepted.");

        // Content-Type check (client bunu dəyişə bilər, amma yenə də faydalıdır)
        var contentType = (file.ContentType ?? "").ToLowerInvariant();
        if (contentType is not "application/pdf")
            throw new InvalidOperationException("Content-Type is not PDF.");

        await using var s = file.OpenReadStream();
        var header = new byte[5];
        var read = await s.ReadAsync(header.AsMemory(0, 5), ct);

        // %PDF- signature yoxlanışı
        if (read < 5 || header[0] != (byte)'%' || header[1] != (byte)'P' || header[2] != (byte)'D' || header[3] != (byte)'F' || header[4] != (byte)'-')
            throw new InvalidOperationException("File does not appear as a PDF.");
    }
}