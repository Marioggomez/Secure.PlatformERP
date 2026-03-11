using Secure.Platform.Api.Middleware;
using Secure.Platform.Common.Configuration;
using Secure.Platform.Data.DependencyInjection;
using Secure.Platform.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var repositoryRoot = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", ".."));
var databaseConfigPath = Path.Combine(repositoryRoot, "database.config.json");
var databaseConfig = DatabaseConfigLoader.Load(databaseConfigPath);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSecureData(databaseConfig.ConnectionString);
builder.Services.AddSecureInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<AuditMiddleware>();

app.MapControllers();

app.Run();
