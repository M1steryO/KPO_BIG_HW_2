using FileStoringService.Core.Repositories;
using FileStoringService.Infrastructure.Persistence;
using Microsoft.Extensions.Options;
using FileEntity = FileStoringService.Core.Entities.File;
using Microsoft.EntityFrameworkCore;

namespace FileStoringService.Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly FileDbContext _db;
        private readonly string _basePath;

        public FileRepository(FileDbContext db, IOptions<StorageOptions> options)
        {
            _db = db;
            _basePath = options.Value.BasePath;
        }

        public async Task<FileEntity?> GetByHashAsync(string hash, CancellationToken cancellationToken = default)
        {
            return await _db.Files
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.Metadata.Hash == hash, cancellationToken);
        }

        public async Task<long> SaveAsync(FileEntity file, Stream content,
            CancellationToken cancellationToken = default)
        {
            var location = Path.Combine(_basePath, file.Metadata.Hash);

            file = new FileEntity(file.Metadata, location);

            await _db.Files.AddAsync(file, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);


            var id = file.Id;

            Directory.CreateDirectory(_basePath);
            await using var fs = File.Create(location);
            await content.CopyToAsync(fs, cancellationToken);
            return id;
        }

        public async Task<(FileEntity file, Stream content)> GetByIdAsync(long id,
            CancellationToken cancellationToken = default)
        {
            var fileEntity = await _db.Files.FindAsync(new object[] { id }, cancellationToken)
                             ?? throw new FileNotFoundException($"File with id {id} not found");

            var contentPath = Path.Combine(_basePath, fileEntity.Metadata.Hash);
            if (!File.Exists(contentPath))
                throw new FileNotFoundException($"File content not found for id {id}");

            var stream = File.OpenRead(contentPath);
            return (fileEntity, stream);
        }
    }
}