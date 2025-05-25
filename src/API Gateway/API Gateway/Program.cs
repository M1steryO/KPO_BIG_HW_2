using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("gateway", new OpenApiInfo { Title = "API Gateway", Version = "v1" });
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        var path = apiDesc.RelativePath; // e.g. "api/file/{id}"
        return path.StartsWith("api/file") || path.StartsWith("api/analyze");
    });
});

builder.Services.AddOcelot(builder.Configuration);
builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/gateway/swagger.json", "API Gateway");
    c.DefaultModelsExpandDepth(-1);
    c.RoutePrefix = string.Empty;
});

await app.UseOcelot();
app.Run();