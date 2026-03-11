namespace Secure.Platform.Api.Middleware;

/// <summary>
/// Middleware para correlacion de requests.
/// Autor: Mario Gomez.
/// </summary>
public sealed class CorrelationIdMiddleware
{
    private const string HeaderName = "X-Correlation-Id";
    private readonly RequestDelegate _next;

    /// <summary>
    /// Crea una instancia del middleware.
    /// </summary>
    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Ejecuta la logica de correlacion para el request actual.
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers.TryGetValue(HeaderName, out var incoming)
            && Guid.TryParse(incoming.FirstOrDefault(), out var parsed)
            ? parsed
            : Guid.NewGuid();

        context.Items[HeaderName] = correlationId;
        context.Response.Headers[HeaderName] = correlationId.ToString();

        await _next(context).ConfigureAwait(false);
    }
}
