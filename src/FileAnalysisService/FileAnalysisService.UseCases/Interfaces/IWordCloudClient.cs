namespace FileAnalysisService.UseCases.Interfaces;

public interface IWordCloudClient
{
    Task<Stream> CreateWordCloudAsync(string fullText);
}