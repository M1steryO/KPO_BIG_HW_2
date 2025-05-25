using FileAnalysisService.UseCases.Interfaces;
using FileAnalysisService.Core.Repositories;
using FileAnalysisService.Infrastructure.Persistence;
using FileAnalysisService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using FileAnalysisServiceObject = FileAnalysisService.Infrastructure.Services.FileAnalysisService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IFileStoringClient, FileStoringClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["FileStoringService:BaseUrl"]);
});
var wcOptions = builder.Configuration.GetSection("WordCloudApi").Get<WordCloudApiOptions>();

builder.Services.AddHttpClient<IWordCloudClient, WordCloudClient>(c => { c.BaseAddress = new Uri(wcOptions.BaseUrl); });


builder.Services.Configure<StorageOptions>(options =>
{
    builder.Configuration.GetSection("Storage").Bind(options);
    var contentRoot = builder.Environment.ContentRootPath;
    options.BasePath = Path.Combine(contentRoot, options.BasePath);
});

builder.Services.AddDbContext<AnalysisResultDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetSection("Storage").Get<StorageOptions>().ConnectionString);
});

builder.Services.AddScoped<FileTextAnalyzer>();
builder.Services.AddScoped<IAnalysisResultRepository, AnalysisResultRepository>();

builder.Services
    .AddScoped<IFileAnalysisService, FileAnalysisServiceObject>();
builder.Services
    .AddScoped<IWordCloudAnalysisService, WordCloudAnalysisService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AnalysisResultDbContext>();
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
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FileAnalysisService FileAnalysisService.Web V1");
    c.RoutePrefix = string.Empty;
});

app.MapControllers();
app.Run();