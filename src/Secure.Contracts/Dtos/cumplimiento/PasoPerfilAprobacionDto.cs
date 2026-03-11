namespace Secure.Platform.Contracts.Dtos.Cumplimiento;

/// <summary>
/// DTO de la tabla cumplimiento.paso_perfil_aprobacion.
/// Autor: Mario Gomez.
/// </summary>
public sealed class PasoPerfilAprobacionDto
{
    /// <summary>
    /// Columna id_paso_perfil_aprobacion.
    /// </summary>
    public long? IdPasoPerfilAprobacion { get; set; }
    /// <summary>
    /// Columna id_perfil_aprobacion.
    /// </summary>
    public long? IdPerfilAprobacion { get; set; }
    /// <summary>
    /// Columna nivel_orden.
    /// </summary>
    public byte? NivelOrden { get; set; }
    /// <summary>
    /// Columna id_rol_requerido.
    /// </summary>
    public long? IdRolRequerido { get; set; }
    /// <summary>
    /// Columna id_privilegio_requerido.
    /// </summary>
    public long? IdPrivilegioRequerido { get; set; }
    /// <summary>
    /// Columna id_alcance_asignacion_requerido.
    /// </summary>
    public short? IdAlcanceAsignacionRequerido { get; set; }
    /// <summary>
    /// Columna aprobadores_minimos.
    /// </summary>
    public byte? AprobadoresMinimos { get; set; }
    /// <summary>
    /// Columna todos_deben_aprobar.
    /// </summary>
    public bool? TodosDebenAprobar { get; set; }
    /// <summary>
    /// Columna monto_minimo.
    /// </summary>
    public decimal? MontoMinimo { get; set; }
    /// <summary>
    /// Columna monto_maximo.
    /// </summary>
    public decimal? MontoMaximo { get; set; }
    /// <summary>
    /// Columna activo.
    /// </summary>
    public bool? Activo { get; set; }
}
