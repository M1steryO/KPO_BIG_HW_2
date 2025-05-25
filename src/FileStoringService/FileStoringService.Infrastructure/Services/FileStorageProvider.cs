using System.Security.Cryptography;
using FileEntity = FileStoringService.Core.Entities.File;
using FileMetadata = FileStoringService.Core.ValueObjects.FileMetadata;
using FileStoringService.Core.Repositories;
using FileStoringService.UseCases.Interfaces;

namespace FileStoringService.Infrastructure.Services;

public class FileStorageProvider : IFileStorageService
{
    private readonly IFileRepository _repository;

    public FileStorageProvider(IFileRepository repository)
    {
        _repository = repository;
    }

    public async Task<long> StoreFileAsync(string fileName, string contentType, Stream content,
        CancellationToken cancellationToken = default)
    {
        await using var ms = new MemoryStream();
        await content.CopyToAsync(ms, cancellationToken);

        ms.Position = 0;
        using var sha = SHA256.Create();
        var hashBytes = sha.ComputeHash(ms);
        var hash = Convert.ToHexString(hashBytes);

        var existing = await _repository.GetByHashAsync(hash, cancellationToken);
        if (existing != null)
            return existing.Id;

        var metadata = new FileMetadata(fileName, contentType, ms.Length, hash);
        var fileEntity = new FileEntity(metadata);
        ms.Position = 0;

        var newId = await _repository.SaveAsync(fileEntity, ms, cancellationToken);
        return newId;
    }

    public async Task<(string FileName, string ContentType, Stream Content)> GetFileByIdAsync(long id,
        CancellationToken cancellationToken = default)
    {
        var (file, stream) = await _repository.GetByIdAsync(id, cancellationToken);
        return (file.Metadata.FileName, file.Metadata.ContentType, stream);
    }
}