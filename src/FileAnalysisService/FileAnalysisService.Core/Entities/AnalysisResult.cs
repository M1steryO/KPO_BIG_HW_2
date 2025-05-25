using FileAnalysisService.Core.ValueObjects;

namespace FileAnalysisService.Core.Entities;

public class AnalysisResult
{
    public long Id { get; private set; }
    public TextStatistics? Statistics { get; set; }

    public long FileId { get; set; }

    public string? ImgLocation { get; set; }

    public AnalysisResult()
    {
    }

    public AnalysisResult(TextStatistics stats, long fileId)
    {
        Statistics = stats;
        FileId = fileId;
    }
}