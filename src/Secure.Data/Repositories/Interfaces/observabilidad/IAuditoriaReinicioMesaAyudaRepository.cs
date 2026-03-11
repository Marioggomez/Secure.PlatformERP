using Secure.Platform.Contracts.Dtos.Observabilidad;

namespace Secure.Platform.Data.Repositories.Interfaces.Observabilidad;

/// <summary>
/// Contrato del repositorio para observabilidad.auditoria_reinicio_mesa_ayuda.
/// Autor: Mario Gomez.
/// </summary>
public interface IAuditoriaReinicioMesaAyudaRepository
{
    Task<IReadOnlyList<AuditoriaReinicioMesaAyudaDto>> ListarAsync(CancellationToken cancellationToken);
    Task<AuditoriaReinicioMesaAyudaDto?> ObtenerAsync(long idAuditoriaReinicioMesaAyuda, CancellationToken cancellationToken);
    Task<long> CrearAsync(AuditoriaReinicioMesaAyudaDto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(AuditoriaReinicioMesaAyudaDto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync(long idAuditoriaReinicioMesaAyuda, string? usuario, CancellationToken cancellationToken);
}
