namespace FileStoringService.UseCases.Interfaces;

public interface IFileStorageService
{
    Task<long> StoreFileAsync(string fileName, string contentType, Stream content, CancellationToken cancellationToken = default);
    Task<(string FileName, string ContentType, Stream Content)> GetFileByIdAsync(long id, CancellationToken cancellationToken = default);
}