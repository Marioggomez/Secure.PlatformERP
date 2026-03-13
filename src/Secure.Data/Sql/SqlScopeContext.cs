using System.Threading;

namespace Secure.Platform.Data.Sql;

/// <summary>
/// Scope de seguridad actual propagado por request para SQL SESSION_CONTEXT.
/// Autor: Mario Gomez.
/// </summary>
public static class SqlScopeContext
{
    private static readonly AsyncLocal<SqlScopeSnapshot?> CurrentScope = new();

    public static SqlScopeSnapshot? Current => CurrentScope.Value;

    public static void Set(long idTenant, long idEmpresa, long idUsuario)
    {
        CurrentScope.Value = new SqlScopeSnapshot(idTenant, idEmpresa, idUsuario);
    }

    public static void Clear()
    {
        CurrentScope.Value = null;
    }
}

/// <summary>
/// Snapshot inmutable del scope de sesion.
/// </summary>
public sealed record SqlScopeSnapshot(long IdTenant, long IdEmpresa, long IdUsuario);
