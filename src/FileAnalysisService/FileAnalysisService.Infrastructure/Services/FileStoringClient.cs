using FileAnalysisService.UseCases.Interfaces;

namespace FileAnalysisService.Infrastructure.Services;

public class FileStoringClient : IFileStoringClient
{
    private readonly HttpClient _httpClient;

    public FileStoringClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(Stream Content, string ContentType, string FileName)> GetFileAsync(long id)
    {
        var response = await _httpClient.GetAsync($"api/file/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new FileNotFoundException($"File with id {id} not found");
        }

        response.EnsureSuccessStatusCode();

        var contentStream = await response.Content.ReadAsStreamAsync();
        var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";

        var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"')
                       ?? $"file_{id}";

        return (contentStream, contentType, fileName);
    }
}