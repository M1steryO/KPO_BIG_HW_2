using FileStoringService.Core.Repositories;
using FileStoringService.Infrastructure.Persistence;
using FileStoringService.Infrastructure.Repositories;
using FileStoringService.Infrastructure.Services;
using FileStoringService.UseCases.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<StorageOptions>(options =>
{
    builder.Configuration.GetSection("Storage").Bind(options);
    var contentRoot = builder.Environment.ContentRootPath;
    options.BasePath = Path.Combine(contentRoot, options.BasePath);
});

builder.Services.AddDbContext<FileDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetSection("Storage").Get<StorageOptions>().ConnectionString));

builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IFileStorageService, FileStorageProvider>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FileDbContext>();
    try
    {
        db.Database.EnsureCreated();
    }
    catch (System.Net.Sockets.SocketException)
    {
        Console.WriteLine("DB is not initialized!");
        return;
    }
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FileStoringService FileStoringService.Web V1");
    c.RoutePrefix = string.Empty;
});
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();