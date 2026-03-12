namespace Secure.Platform.Contracts.Dtos.Tercero;

/// <summary>
/// DTO de la tabla tercero.cuenta_bancaria_tercero.
/// Autor: Mario Gomez.
/// </summary>
public sealed class CuentaBancariaTerceroDto
{
    /// <summary>
    /// Columna id_cuenta_bancaria_tercero.
    /// </summary>
    public long? IdCuentaBancariaTercero { get; set; }
    /// <summary>
    /// Columna id_tercero.
    /// </summary>
    public long? IdTercero { get; set; }
    /// <summary>
    /// Columna id_banco.
    /// </summary>
    public int? IdBanco { get; set; }
    /// <summary>
    /// Columna numero_cuenta.
    /// </summary>
    public string? NumeroCuenta { get; set; }
    /// <summary>
    /// Columna id_moneda.
    /// </summary>
    public int? IdMoneda { get; set; }
    /// <summary>
    /// Columna principal.
    /// </summary>
    public bool? Principal { get; set; }
}
