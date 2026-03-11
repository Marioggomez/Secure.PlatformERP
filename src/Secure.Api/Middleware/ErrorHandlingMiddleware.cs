using Secure.Platform.Infrastructure.Observability;

namespace Secure.Platform.Api.Middleware;

/// <summary>
/// Middleware global para manejo de errores tecnicos.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    /// <summary>
    /// Crea el middleware de manejo de errores.
    /// </summary>
    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Maneja excepciones no controladas y registra el error tecnico.
    /// </summary>
    public async Task InvokeAsync(HttpContext context, IApiObservabilityService observabilityService)
    {
        try
        {
            await _next(context).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error tecnico no controlado");

            var correlationId = context.Items.TryGetValue("X-Correlation-Id", out var item) && item is Guid guid
                ? guid
                : (Guid?)null;

            await observabilityService.LogErrorAsync(
                correlationId,
                context.User?.Identity?.Name,
                context.Request.Path.Value,
                exception.Message,
                exception.StackTrace,
                null,
                context.RequestAborted).ConfigureAwait(false);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                mensaje = "Error tecnico en API",
                correlationId
            }, context.RequestAborted).ConfigureAwait(false);
        }
    }
}
