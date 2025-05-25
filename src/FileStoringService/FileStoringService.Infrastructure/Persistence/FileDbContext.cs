using Microsoft.EntityFrameworkCore;
using FileEntity = FileStoringService.Core.Entities.File;

namespace FileStoringService.Infrastructure.Persistence;

public class FileDbContext : DbContext
{
    public FileDbContext(DbContextOptions<FileDbContext> options) : base(options)
    {
    }

    public DbSet<FileEntity> Files { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FileEntity>(entity =>
        {
            entity.ToTable("files");
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Location)
                .HasColumnName("location")
                .IsRequired();
            entity.OwnsOne(f => f.Metadata, m =>
            {
                m.Property(p => p.FileName).HasColumnName("file_name");
                m.Property(p => p.ContentType).HasColumnName("content_type");
                m.Property(p => p.Size).HasColumnName("size");
                m.Property(p => p.Hash).HasColumnName("hash");
                m.HasIndex(p => p.Hash).IsUnique();
            });
        });
    }
}