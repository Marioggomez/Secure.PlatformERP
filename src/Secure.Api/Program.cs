using Secure.Platform.Api.Middleware;
using Secure.Platform.Common.Configuration;
using Secure.Platform.Data.DependencyInjection;
using Secure.Platform.Infrastructure.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var repositoryRoot = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", ".."));
var databaseConfigPath = Path.Combine(repositoryRoot, "database.config.json");
var databaseConfig = DatabaseConfigLoader.Load(databaseConfigPath);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Secure Platform ERP API",
        Version = "v1",
        Description = "API de Secure Platform ERP agrupada por modulo funcional."
    });

    options.TagActionsBy(apiDescription =>
    {
        var relativePath = apiDescription.RelativePath ?? string.Empty;
        var pathSegments = relativePath
            .Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (pathSegments.Length >= 3 &&
            string.Equals(pathSegments[0], "api", StringComparison.OrdinalIgnoreCase) &&
            pathSegments[1].StartsWith("v", StringComparison.OrdinalIgnoreCase))
        {
            return new[] { BuildModuleTag(pathSegments[2]) };
        }

        return new[] { "General" };
    });

    options.OrderActionsBy(apiDescription =>
    {
        var relativePath = apiDescription.RelativePath ?? string.Empty;
        var pathSegments = relativePath
            .Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var module = pathSegments.Length >= 3 ? BuildModuleTag(pathSegments[2]) : "General";
        return $"{module}_{relativePath}_{apiDescription.HttpMethod}";
    });
});

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
app.UseMiddleware<SessionContextMiddleware>();
app.UseMiddleware<ScopeEnforcementMiddleware>();
app.UseMiddleware<AuditMiddleware>();

app.MapControllers();

app.Run();

static string BuildModuleTag(string rawModule)
{
    var normalized = (rawModule ?? string.Empty).Trim().ToLowerInvariant();
    return normalized switch
    {
        "seguridad" => "Seguridad",
        "tercero" => "Tercero",
        "organizacion" => "Organizacion",
        "catalogo" => "Catalogo",
        "plataforma" => "Plataforma",
        "cumplimiento" => "Cumplimiento",
        "observabilidad" => "Observabilidad",
        "dbo" => "Dbo",
        _ when string.IsNullOrWhiteSpace(normalized) => "General",
        _ => char.ToUpperInvariant(normalized[0]) + normalized[1..]
    };
}
