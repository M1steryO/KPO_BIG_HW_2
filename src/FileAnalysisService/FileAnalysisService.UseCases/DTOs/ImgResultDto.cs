namespace FileAnalysisService.UseCases.DTOs;

public class ImgResultDto
{
    public string? FileName { get; set; }
    public Stream? Content { get; set; }
    
    public string? ContentType { get; set; }
}