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
        return new[] { BuildEnterpriseTag(relativePath) };
    });

    options.OrderActionsBy(apiDescription =>
    {
        var relativePath = apiDescription.RelativePath ?? string.Empty;
        var (module, schema, table, endpoint) = ParseRouteSegments(relativePath);
        return $"{module}_{schema}_{table}_{endpoint}_{apiDescription.HttpMethod}";
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

static string BuildEnterpriseTag(string relativePath)
{
    var (module, schema, table, _) = ParseRouteSegments(relativePath);
    return $"{module} | {schema}.{table}";
}

static (string Module, string Schema, string Table, string Endpoint) ParseRouteSegments(string relativePath)
{
    var pathOnly = (relativePath ?? string.Empty).Split('?', 2)[0];
    var pathSegments = pathOnly.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    if (pathSegments.Length >= 4 &&
        string.Equals(pathSegments[0], "api", StringComparison.OrdinalIgnoreCase) &&
        pathSegments[1].StartsWith("v", StringComparison.OrdinalIgnoreCase))
    {
        var schema = NormalizeSegment(pathSegments[2]);
        var table = NormalizeSegment(pathSegments[3]);
        var endpoint = pathSegments.Length >= 5
            ? NormalizeSegment(string.Join("_", pathSegments.Skip(4)))
            : "base";
        var module = BuildModuleLabel(schema);
        return (module, schema, table, endpoint);
    }

    return ("General", "general", "general", "base");
}

static string BuildModuleLabel(string schema)
{
    return schema switch
    {
        "seguridad" => "Seguridad",
        "tercero" => "Tercero",
        "organizacion" => "Organizacion",
        "catalogo" => "Catalogo",
        "plataforma" => "Plataforma",
        "cumplimiento" => "Cumplimiento",
        "observabilidad" => "Observabilidad",
        "dbo" => "Dbo",
        _ => "General"
    };
}

static string NormalizeSegment(string? segment)
{
    if (string.IsNullOrWhiteSpace(segment))
    {
        return "unknown";
    }

    var normalized = segment.Trim().ToLowerInvariant();
    return normalized.Replace("{", string.Empty).Replace("}", string.Empty);
}
