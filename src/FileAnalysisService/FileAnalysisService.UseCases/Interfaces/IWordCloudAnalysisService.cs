using FileAnalysisService.UseCases.DTOs;

namespace FileAnalysisService.UseCases.Interfaces;

public interface IWordCloudAnalysisService
{
    Task<ImgResultDto> GenerateImgByFileIdAsync(long fileId, CancellationToken cancellationToken = default);
}