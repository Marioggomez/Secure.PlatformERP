using System.Diagnostics;
using Secure.Platform.Infrastructure.Observability;

namespace Secure.Platform.Api.Middleware;

/// <summary>
/// Middleware de auditoria de requests API.
/// Autor: Mario Gomez.
/// </summary>
public sealed class AuditMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Crea el middleware de auditoria.
    /// </summary>
    public AuditMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Registra usuario, endpoint, resultado, ip y duracion.
    /// </summary>
    public async Task InvokeAsync(HttpContext context, IApiObservabilityService observabilityService)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context).ConfigureAwait(false);
        }
        finally
        {
            stopwatch.Stop();

            var correlationId = context.Items.TryGetValue("X-Correlation-Id", out var item) && item is Guid guid
                ? guid
                : (Guid?)null;

            await observabilityService.LogOperationAsync(
                correlationId,
                context.Request.Path.Value,
                context.Request.Method,
                context.User?.Identity?.Name,
                context.Response.StatusCode,
                (int)stopwatch.ElapsedMilliseconds,
                context.Connection.RemoteIpAddress?.ToString(),
                null,
                context.RequestAborted).ConfigureAwait(false);
        }
    }
}
