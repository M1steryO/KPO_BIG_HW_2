using FileAnalysisService.Core.Entities;
using FileAnalysisService.Core.Repositories;
using FileAnalysisService.Core.ValueObjects;
using FileAnalysisService.UseCases.Interfaces;
using FileAnalysisService.UseCases.DTOs;

namespace FileAnalysisService.Infrastructure.Services;

public class FileAnalysisService : IFileAnalysisService
{
    private readonly IFileStoringClient _fileClient;
    private readonly IAnalysisResultRepository _repo;

    public FileAnalysisService(IFileStoringClient fileClient, IAnalysisResultRepository repo)
    {
        _fileClient = fileClient;
        _repo = repo;
    }

    public async Task<AnalysisResultDto> AnalyzeAsync(long fileId, CancellationToken cancellationToken = default)
    {
        var existing = await _repo.GetByFileIdAsync(fileId, cancellationToken);

        var stats = existing?.Statistics
                    ?? await CalculateAndSaveStatisticsAsync(fileId, cancellationToken);

        return new AnalysisResultDto
        {
            Paragraphs = stats.ParagraphCount,
            Words = stats.WordCount,
            Characters = stats.CharacterCount
        };
    }

    private async Task<TextStatistics> CalculateAndSaveStatisticsAsync(
        long fileId,
        CancellationToken cancellationToken)
    {
        var (content, _, _) = await _fileClient.GetFileAsync(fileId);

        var stats = FileTextAnalyzer.Analyze(content);

        var analysisResult = new AnalysisResult(stats, fileId);
        await _repo.SaveAsync(analysisResult, cancellationToken);

        return stats;
    }
}