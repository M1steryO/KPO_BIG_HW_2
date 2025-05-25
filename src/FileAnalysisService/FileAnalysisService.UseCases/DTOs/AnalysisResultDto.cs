namespace FileAnalysisService.UseCases.DTOs;

public class AnalysisResultDto
{
    public int? Paragraphs { get; set; }
    public int? Words { get; set; }
    public int? Characters { get; set; }
}