using FileAnalysisService.Core.Entities;

namespace FileAnalysisService.Core.Repositories;

public interface IAnalysisResultRepository
{
    Task<long> SaveAsync(AnalysisResult result, CancellationToken cancellationToken = default);
    Task<AnalysisResult?> GetByFileIdAsync(long id, CancellationToken cancellationToken = default);

    Task<AnalysisResult?> SaveLocationByFileIdAsync(long id, string location,
        CancellationToken cancellationToken = default);
}