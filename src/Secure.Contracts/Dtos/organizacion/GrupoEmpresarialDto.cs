namespace Secure.Platform.Contracts.Dtos.Organizacion;

/// <summary>
/// DTO de la tabla organizacion.grupo_empresarial.
/// Autor: Mario Gomez.
/// </summary>
public sealed class GrupoEmpresarialDto
{
    /// <summary>
    /// Columna id_grupo_empresarial.
    /// </summary>
    public long? IdGrupoEmpresarial { get; set; }
    /// <summary>
    /// Columna id_tenant.
    /// </summary>
    public long? IdTenant { get; set; }
    /// <summary>
    /// Columna codigo.
    /// </summary>
    public string? Codigo { get; set; }
    /// <summary>
    /// Columna nombre.
    /// </summary>
    public string? Nombre { get; set; }
    /// <summary>
    /// Columna descripcion.
    /// </summary>
    public string? Descripcion { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
    /// <summary>
    /// Columna creado_utc.
    /// </summary>
    public DateTime? CreadoUtc { get; set; }
    /// <summary>
    /// Columna actualizado_utc.
    /// </summary>
    public DateTime? ActualizadoUtc { get; set; }
    /// <summary>
    /// Columna version_fila.
    /// </summary>
    public byte[]? VersionFila { get; set; }
}
