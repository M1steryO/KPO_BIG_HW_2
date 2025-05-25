using FileAnalysisService.UseCases.DTOs;

namespace FileAnalysisService.UseCases.Interfaces;

public interface IFileAnalysisService
{
    Task<AnalysisResultDto> AnalyzeAsync(long fileId, CancellationToken cancellationToken = default);
}