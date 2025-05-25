using FileAnalysisService.Core.Entities;
using FileAnalysisService.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysisService.Infrastructure.Persistence;

public class AnalysisResultRepository : IAnalysisResultRepository
{
    private readonly AnalysisResultDbContext _db;


    public AnalysisResultRepository(AnalysisResultDbContext db)
    {
        _db = db;
    }

    public async Task<long> SaveAsync(AnalysisResult result, CancellationToken cancellationToken = default)
    {
        var existing = await _db.AnalysisResults
            .FirstOrDefaultAsync(x => x.FileId == result.FileId, cancellationToken);

        if (existing is null)
        {
            await _db.AnalysisResults.AddAsync(result, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return result.Id;
        }

        existing.Statistics = result.Statistics;

        _db.AnalysisResults.Update(existing);
        await _db.SaveChangesAsync(cancellationToken);

        return existing.Id;
    }

    public async Task<AnalysisResult?> GetByFileIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _db.AnalysisResults
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.FileId == id, cancellationToken);
    }

    public async Task<AnalysisResult?> SaveLocationByFileIdAsync(long id, string location,
        CancellationToken cancellationToken = default)
    {
        var entity = await _db.AnalysisResults
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.FileId == id, cancellationToken);

        if (entity == null)
        {
            entity = new AnalysisResult
            {
                FileId = id,
                ImgLocation = location,
            };
            await _db.AnalysisResults.AddAsync(entity, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return entity;
        }

        entity.ImgLocation = location;
        _db.AnalysisResults.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);

        return entity;
    }
}