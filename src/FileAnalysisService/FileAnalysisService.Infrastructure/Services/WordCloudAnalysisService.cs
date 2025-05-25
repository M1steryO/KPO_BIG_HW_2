using System.Text;
using FileAnalysisService.Core.Repositories;
using FileAnalysisService.UseCases.DTOs;
using FileAnalysisService.UseCases.Interfaces;
using FileAnalysisService.Infrastructure.Persistence;
using Microsoft.Extensions.Options;

namespace FileAnalysisService.Infrastructure.Services;

public class WordCloudAnalysisService : IWordCloudAnalysisService
{
    private readonly IFileStoringClient _fs;
    private readonly IWordCloudClient _wc;
    private readonly IAnalysisResultRepository _repo;
    private readonly string _basePath;

    private const string BaseImgContentType = "image/png";


    public WordCloudAnalysisService(
        IFileStoringClient fs,
        IWordCloudClient wc, IAnalysisResultRepository repo, IOptions<StorageOptions> options)
    {
        _fs = fs;
        _wc = wc;
        _repo = repo;
        _basePath = options.Value.BasePath;
    }

    public async Task<ImgResultDto> GenerateImgByFileIdAsync(long fileId, CancellationToken cancellationToken = default)
    {
        var existing = await _repo.GetByFileIdAsync(fileId, cancellationToken);
        ImgResultDto dto;
        if (existing?.ImgLocation != null)
        {
            var data = File.OpenRead(Path.Combine(_basePath, existing.ImgLocation));
            dto = new ImgResultDto
            {
                Content = data,
                FileName = existing.ImgLocation,
                ContentType = BaseImgContentType
            };
            return dto;
        }

        var (content, _, fileName) = await _fs.GetFileAsync(fileId);
        using var reader = new StreamReader(content, Encoding.UTF8, leaveOpen: true);
        var text = reader.ReadToEnd();

        var imgFileName = fileName.Split(".")[0] + ".png";
        var imgContent = await _wc.CreateWordCloudAsync(text);

        Directory.CreateDirectory(_basePath);
        var location = Path.Combine(_basePath, imgFileName);
        await using var fs = File.Create(location);
        await content.CopyToAsync(fs, cancellationToken);

        await _repo.SaveLocationByFileIdAsync(fileId, imgFileName, cancellationToken);

        dto = new ImgResultDto
        {
            Content = imgContent,
            FileName = imgFileName,
            ContentType = BaseImgContentType
        };
        return dto;
    }
}