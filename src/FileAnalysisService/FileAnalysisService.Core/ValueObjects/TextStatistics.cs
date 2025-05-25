namespace FileAnalysisService.Core.ValueObjects;

public class TextStatistics
{
    public int? ParagraphCount { get; set; }
    public int? WordCount { get; set; }
    public int? CharacterCount { get; set; }

    public TextStatistics()
    {
    }

    public TextStatistics(int paragraphs, int words, int characters)
    {
        ParagraphCount = paragraphs;
        WordCount = words;
        CharacterCount = characters;
    }
}