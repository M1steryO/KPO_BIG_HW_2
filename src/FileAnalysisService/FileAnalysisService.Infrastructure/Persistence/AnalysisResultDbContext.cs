using FileAnalysisService.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysisService.Infrastructure.Persistence;

public class AnalysisResultDbContext : DbContext
{
    public AnalysisResultDbContext(DbContextOptions<AnalysisResultDbContext> options) : base(options)
    {
    }

    public DbSet<AnalysisResult> AnalysisResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnalysisResult>(entity =>
        {
            entity.ToTable("analysis_results");
            entity.HasKey(f => f.Id);
            entity.Property(f => f.FileId)
                .HasColumnName("file_id")
                .IsRequired();
            entity.Property(f => f.ImgLocation)
                .HasColumnName("img_location");
            entity.OwnsOne(f => f.Statistics, m =>
            {
                m.Property(p => p.CharacterCount).HasColumnName("character_count");
                m.Property(p => p.ParagraphCount).HasColumnName("paragraph_count");
                m.Property(p => p.WordCount).HasColumnName("word_count");
            });
        });
    }
}