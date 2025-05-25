using System.Text;
using FileAnalysisService.Core.ValueObjects;

namespace FileAnalysisService.Infrastructure.Services;

public class FileTextAnalyzer
{
    public static TextStatistics Analyze(Stream content)
    {
        using var reader = new StreamReader(content, Encoding.UTF8, leaveOpen: true);
        var text = reader.ReadToEnd();

        var paragraphs = text.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries).Length;

        var words = text.Split((char[])null!, StringSplitOptions.RemoveEmptyEntries).Length;

        var characters = text.Count(c => c != '\r' && c != '\n');
        return new TextStatistics(paragraphs, words, characters);
    }
}