using System.Data.SqlClient;

namespace Secure.Platform.Data.Sql;

/// <summary>
/// Fabrica de conexiones SQL para ADO.NET puro.
/// Autor: Mario Gomez.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Crea una conexion SQL sin abrir.
    /// </summary>
    SqlConnection CreateConnection();
}
