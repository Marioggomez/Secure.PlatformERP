namespace Secure.Platform.Contracts.Dtos.Organizacion;

/// <summary>
/// DTO de la tabla organizacion.grupo_empresarial_empresa.
/// Autor: Mario Gomez.
/// </summary>
public sealed class GrupoEmpresarialEmpresaDto
{
    /// <summary>
    /// Columna id_grupo_empresarial.
    /// </summary>
    public long? IdGrupoEmpresarial { get; set; }
    /// <summary>
    /// Columna id_empresa.
    /// </summary>
    public long? IdEmpresa { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
    /// <summary>
    /// Columna creado_utc.
    /// </summary>
    public DateTime? CreadoUtc { get; set; }
}
