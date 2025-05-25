using System.Net.Http.Json;
using FileAnalysisService.UseCases.Interfaces;

namespace FileAnalysisService.Infrastructure.Services;

public class WordCloudClient : IWordCloudClient
{
    private readonly HttpClient _http;
    private const string RequestUri = "/wordcloud";
    private const string ImgFormat = "png";

    public WordCloudClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<Stream> CreateWordCloudAsync(string fullText)
    {
        var payload = new { text = fullText, format = ImgFormat };
        var resp = await _http.PostAsJsonAsync(RequestUri, payload);
        resp.EnsureSuccessStatusCode();
        var doc = await resp.Content.ReadAsStreamAsync();
        return doc;
    }
}