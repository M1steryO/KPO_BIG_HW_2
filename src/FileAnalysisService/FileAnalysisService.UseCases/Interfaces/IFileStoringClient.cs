namespace FileAnalysisService.UseCases.Interfaces;

public interface IFileStoringClient
{
    Task<(Stream Content, string ContentType, string FileName)> GetFileAsync(long id);
}