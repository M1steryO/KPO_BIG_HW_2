using FileEntity = FileStoringService.Core.Entities.File;

namespace FileStoringService.Core.Repositories
{
    public interface IFileRepository
    {
        Task<FileEntity?> GetByHashAsync(string hash,
            CancellationToken cancellationToken = default);

        Task<long> SaveAsync(FileEntity file, Stream content, CancellationToken cancellationToken = default);

        Task<(FileEntity file, Stream content)> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    }
}