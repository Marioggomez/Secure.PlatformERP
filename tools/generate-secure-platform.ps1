param(
    [string]$RepoRoot = "C:\Users\Administrator\OneDrive\Documents\Secure Plataform ERP"
)

$ErrorActionPreference = "Stop"

function Ensure-Directory {
    param([string]$Path)
    if (-not (Test-Path $Path)) {
        New-Item -Path $Path -ItemType Directory -Force | Out-Null
    }
}

function Write-FileUtf8 {
    param(
        [string]$Path,
        [string]$Content
    )

    Ensure-Directory -Path (Split-Path -Path $Path -Parent)
    Set-Content -Path $Path -Value $Content -Encoding UTF8
}

function To-PascalCase {
    param([string]$Value)
    if ([string]::IsNullOrWhiteSpace($Value)) { return $Value }

    $parts = $Value -split "[^a-zA-Z0-9]+"
    $tokens = foreach ($part in $parts) {
        if ([string]::IsNullOrWhiteSpace($part)) { continue }
        if ($part.Length -eq 1) {
            $part.ToUpperInvariant()
        }
        else {
            $part.Substring(0, 1).ToUpperInvariant() + $part.Substring(1)
        }
    }

    return ($tokens -join "")
}

function To-CamelCase {
    param([string]$Value)
    $pascal = To-PascalCase -Value $Value
    if ([string]::IsNullOrWhiteSpace($pascal)) { return $pascal }
    if ($pascal.Length -eq 1) { return $pascal.ToLowerInvariant() }
    return $pascal.Substring(0, 1).ToLowerInvariant() + $pascal.Substring(1)
}

function Get-CSharpType {
    param(
        [string]$SqlType,
        [string]$IsNullable
    )

    $baseType = switch ($SqlType.ToLowerInvariant()) {
        "bigint" { "long" }
        "int" { "int" }
        "smallint" { "short" }
        "tinyint" { "byte" }
        "bit" { "bool" }
        "uniqueidentifier" { "Guid" }
        "date" { "DateTime" }
        "datetime" { "DateTime" }
        "datetime2" { "DateTime" }
        "smalldatetime" { "DateTime" }
        "time" { "TimeSpan" }
        "decimal" { "decimal" }
        "numeric" { "decimal" }
        "money" { "decimal" }
        "smallmoney" { "decimal" }
        "float" { "double" }
        "real" { "float" }
        "char" { "string" }
        "nchar" { "string" }
        "varchar" { "string" }
        "nvarchar" { "string" }
        "text" { "string" }
        "ntext" { "string" }
        "xml" { "string" }
        "binary" { "byte[]" }
        "varbinary" { "byte[]" }
        "image" { "byte[]" }
        "timestamp" { "byte[]" }
        "rowversion" { "byte[]" }
        "sql_variant" { "object" }
        default { "string" }
    }

    $isValueType = $baseType -in @("short", "int", "long", "byte", "bool", "Guid", "DateTime", "TimeSpan", "decimal", "double", "float")
    $isNullable = $IsNullable -eq "YES"

    if ($isNullable) {
        if ($isValueType -or $baseType -in @("string", "byte[]", "object")) {
            return ($baseType + "?")
        }
    }

    return [string]$baseType
}

function Get-NonNullableType {
    param([string]$TypeName)
    return $TypeName.TrimEnd("?")
}

function Get-SqlDbType {
    param([string]$SqlType)
    switch ($SqlType.ToLowerInvariant()) {
        "bigint" { "BigInt" }
        "int" { "Int" }
        "smallint" { "SmallInt" }
        "tinyint" { "TinyInt" }
        "bit" { "Bit" }
        "uniqueidentifier" { "UniqueIdentifier" }
        "date" { "Date" }
        "datetime" { "DateTime" }
        "datetime2" { "DateTime2" }
        "smalldatetime" { "SmallDateTime" }
        "time" { "Time" }
        "decimal" { "Decimal" }
        "numeric" { "Decimal" }
        "money" { "Money" }
        "smallmoney" { "SmallMoney" }
        "float" { "Float" }
        "real" { "Real" }
        "char" { "Char" }
        "nchar" { "NChar" }
        "varchar" { "VarChar" }
        "nvarchar" { "NVarChar" }
        "text" { "Text" }
        "ntext" { "NText" }
        "xml" { "Xml" }
        "binary" { "Binary" }
        "varbinary" { "VarBinary" }
        "image" { "Image" }
        "timestamp" { "Timestamp" }
        "rowversion" { "Timestamp" }
        "sql_variant" { "Variant" }
        default { "NVarChar" }
    }
}

function Get-SqlTypeDeclaration {
    param($Column)

    $type = $Column.DATA_TYPE.ToLowerInvariant()

    if ($type -in @("varchar", "nvarchar", "char", "nchar", "binary", "varbinary")) {
        if ($null -eq $Column.CHARACTER_MAXIMUM_LENGTH) { return $type }
        if ([int]$Column.CHARACTER_MAXIMUM_LENGTH -eq -1) { return "$type(max)" }
        return "$type($($Column.CHARACTER_MAXIMUM_LENGTH))"
    }

    if ($type -in @("decimal", "numeric")) {
        $precision = if ($null -eq $Column.NUMERIC_PRECISION) { 18 } else { [int]$Column.NUMERIC_PRECISION }
        $scale = if ($null -eq $Column.NUMERIC_SCALE) { 2 } else { [int]$Column.NUMERIC_SCALE }
        return "$type($precision,$scale)"
    }

    return $type
}

function Get-ReaderExpression {
    param($Column)

    $columnName = $Column.COLUMN_NAME
    $ordinal = "reader.GetOrdinal(`"$columnName`")"
    $sqlType = $Column.DATA_TYPE.ToLowerInvariant()

    $readExpr = switch ($sqlType) {
        "bigint" { "reader.GetInt64($ordinal)" }
        "int" { "reader.GetInt32($ordinal)" }
        "smallint" { "reader.GetInt16($ordinal)" }
        "tinyint" { "reader.GetByte($ordinal)" }
        "bit" { "reader.GetBoolean($ordinal)" }
        "uniqueidentifier" { "reader.GetGuid($ordinal)" }
        "date" { "reader.GetDateTime($ordinal)" }
        "datetime" { "reader.GetDateTime($ordinal)" }
        "datetime2" { "reader.GetDateTime($ordinal)" }
        "smalldatetime" { "reader.GetDateTime($ordinal)" }
        "time" { "reader.GetTimeSpan($ordinal)" }
        "decimal" { "reader.GetDecimal($ordinal)" }
        "numeric" { "reader.GetDecimal($ordinal)" }
        "money" { "reader.GetDecimal($ordinal)" }
        "smallmoney" { "reader.GetDecimal($ordinal)" }
        "float" { "reader.GetDouble($ordinal)" }
        "real" { "reader.GetFloat($ordinal)" }
        "binary" { "(byte[])reader.GetValue($ordinal)" }
        "varbinary" { "(byte[])reader.GetValue($ordinal)" }
        "image" { "(byte[])reader.GetValue($ordinal)" }
        "timestamp" { "(byte[])reader.GetValue($ordinal)" }
        "rowversion" { "(byte[])reader.GetValue($ordinal)" }
        "sql_variant" { "reader.GetValue($ordinal)" }
        default { "reader.GetString($ordinal)" }
    }

    if ($Column.IS_NULLABLE -eq "YES") {
        return "reader.IsDBNull($ordinal) ? null : $readExpr"
    }

    if ($sqlType -in @("varchar", "nvarchar", "char", "nchar", "text", "ntext", "xml")) {
        return "reader.IsDBNull($ordinal) ? string.Empty : $readExpr"
    }

    if ($sqlType -in @("binary", "varbinary", "image", "timestamp", "rowversion")) {
        return "reader.IsDBNull($ordinal) ? Array.Empty<byte>() : $readExpr"
    }

    return $readExpr
}

function Is-IdentityLikePrimaryKey {
    param($PrimaryColumn)
    $type = $PrimaryColumn.DATA_TYPE.ToLowerInvariant()
    return $PrimaryColumn.COLUMN_NAME.ToLowerInvariant().StartsWith("id_") -and ($type -in @("bigint", "int", "smallint", "tinyint", "decimal", "numeric"))
}

function Read-Query {
    param(
        [System.Data.SqlClient.SqlConnection]$Connection,
        [string]$Sql
    )

    $command = $Connection.CreateCommand()
    $command.CommandText = $Sql
    $adapter = New-Object System.Data.SqlClient.SqlDataAdapter($command)
    $table = New-Object System.Data.DataTable
    [void]$adapter.Fill($table)

    $result = @()
    foreach ($row in $table.Rows) {
        $item = [ordered]@{}
        foreach ($column in $table.Columns) {
            $value = $row[$column.ColumnName]
            $item[$column.ColumnName] = if ($value -is [System.DBNull]) { $null } else { $value }
        }
        $result += [PSCustomObject]$item
    }

    return $result
}

$srcRoot = Join-Path $RepoRoot "src"
$databaseConfigPath = Join-Path $RepoRoot "database.config.json"

$databaseConfig = Get-Content -Path $databaseConfigPath -Raw | ConvertFrom-Json
$connection = New-Object System.Data.SqlClient.SqlConnection($databaseConfig.connectionString)
$connection.Open()

$tables = Read-Query -Connection $connection -Sql @"
SELECT TABLE_SCHEMA, TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_SCHEMA, TABLE_NAME;
"@

$columns = Read-Query -Connection $connection -Sql @"
SELECT
    TABLE_SCHEMA,
    TABLE_NAME,
    COLUMN_NAME,
    ORDINAL_POSITION,
    DATA_TYPE,
    IS_NULLABLE,
    CHARACTER_MAXIMUM_LENGTH,
    NUMERIC_PRECISION,
    NUMERIC_SCALE
FROM INFORMATION_SCHEMA.COLUMNS
ORDER BY TABLE_SCHEMA, TABLE_NAME, ORDINAL_POSITION;
"@

$foreignKeys = Read-Query -Connection $connection -Sql @"
SELECT
    fk.name AS FK_NAME,
    s1.name AS PARENT_SCHEMA,
    t1.name AS PARENT_TABLE,
    c1.name AS PARENT_COLUMN,
    s2.name AS REFERENCED_SCHEMA,
    t2.name AS REFERENCED_TABLE,
    c2.name AS REFERENCED_COLUMN
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
INNER JOIN sys.tables t1 ON fkc.parent_object_id = t1.object_id
INNER JOIN sys.schemas s1 ON t1.schema_id = s1.schema_id
INNER JOIN sys.columns c1 ON c1.object_id = t1.object_id AND c1.column_id = fkc.parent_column_id
INNER JOIN sys.tables t2 ON fkc.referenced_object_id = t2.object_id
INNER JOIN sys.schemas s2 ON t2.schema_id = s2.schema_id
INNER JOIN sys.columns c2 ON c2.object_id = t2.object_id AND c2.column_id = fkc.referenced_column_id
ORDER BY s1.name, t1.name, fk.name, fkc.constraint_column_id;
"@

$connection.Close()

Ensure-Directory -Path (Join-Path $RepoRoot "database\schema-snapshots")
@{
    generated_utc = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
    source = "INFORMATION_SCHEMA.TABLES | INFORMATION_SCHEMA.COLUMNS | sys.foreign_keys"
    tableCount = $tables.Count
    columnCount = $columns.Count
    foreignKeyCount = $foreignKeys.Count
    tables = $tables
    foreignKeys = $foreignKeys
} | ConvertTo-Json -Depth 8 | Set-Content -Path (Join-Path $RepoRoot "database\schema-snapshots\secure-schema.json") -Encoding UTF8

$columnsByTable = @{}
foreach ($column in $columns) {
    $key = "$($column.TABLE_SCHEMA).$($column.TABLE_NAME)"
    if (-not $columnsByTable.ContainsKey($key)) {
        $columnsByTable[$key] = New-Object System.Collections.ArrayList
    }
    [void]$columnsByTable[$key].Add($column)
}

foreach ($table in $tables) {
    $schema = $table.TABLE_SCHEMA
    $tableName = $table.TABLE_NAME
    $entityName = To-PascalCase -Value $tableName
    $schemaName = To-PascalCase -Value $schema
    $key = "$schema.$tableName"
    if (-not $columnsByTable.ContainsKey($key)) { continue }

    $tableColumns = @($columnsByTable[$key] | Sort-Object { [int]$_.ORDINAL_POSITION })
    $primaryColumn = $tableColumns[0]
    $primaryProperty = To-PascalCase -Value $primaryColumn.COLUMN_NAME
    $primaryArg = To-CamelCase -Value $primaryColumn.COLUMN_NAME
    $primaryType = Get-CSharpType -SqlType $primaryColumn.DATA_TYPE -IsNullable $primaryColumn.IS_NULLABLE
    $primaryTypeNonNullable = if ($primaryType.EndsWith("?")) { $primaryType.Substring(0, $primaryType.Length - 1) } else { $primaryType }
    $primarySqlType = Get-SqlTypeDeclaration -Column $primaryColumn
    $primaryDbType = Get-SqlDbType -SqlType $primaryColumn.DATA_TYPE
    $primarySizeExpr = if ($primaryColumn.DATA_TYPE.ToLowerInvariant() -in @("varchar", "nvarchar", "char", "nchar", "binary", "varbinary") -and $null -ne $primaryColumn.CHARACTER_MAXIMUM_LENGTH) { ", $($primaryColumn.CHARACTER_MAXIMUM_LENGTH)" } else { "" }

    $dtoProperties = ($tableColumns | ForEach-Object {
        $propertyType = Get-CSharpType -SqlType $_.DATA_TYPE -IsNullable $_.IS_NULLABLE
        $propertyName = To-PascalCase -Value $_.COLUMN_NAME
@"
    /// <summary>
    /// Columna $($_.COLUMN_NAME).
    /// </summary>
    public $propertyType $propertyName { get; set; }
"@
    }) -join "`r`n"

    $dtoContent = @"
namespace Secure.Platform.Contracts.Dtos.$schemaName;

/// <summary>
/// DTO de la tabla $schema.$tableName.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ${entityName}Dto
{
$dtoProperties
}
"@

    Write-FileUtf8 -Path (Join-Path $srcRoot "Secure.Contracts\Dtos\$schema\${entityName}Dto.cs") -Content $dtoContent.Trim()

    $interfaceContent = @"
using Secure.Platform.Contracts.Dtos.$schemaName;

namespace Secure.Platform.Data.Repositories.Interfaces.$schemaName;

/// <summary>
/// Contrato del repositorio para $schema.$tableName.
/// Autor: Mario Gomez.
/// </summary>
public interface I${entityName}Repository
{
    Task<IReadOnlyList<${entityName}Dto>> ListarAsync(CancellationToken cancellationToken);
    Task<${entityName}Dto?> ObtenerAsync($primaryTypeNonNullable $primaryArg, CancellationToken cancellationToken);
    Task<$primaryTypeNonNullable> CrearAsync(${entityName}Dto dto, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(${entityName}Dto dto, CancellationToken cancellationToken);
    Task<bool> DesactivarAsync($primaryTypeNonNullable $primaryArg, string? usuario, CancellationToken cancellationToken);
}
"@

    Write-FileUtf8 -Path (Join-Path $srcRoot "Secure.Data\Repositories\Interfaces\$schema\I${entityName}Repository.cs") -Content $interfaceContent.Trim()

    $mapProperties = ($tableColumns | ForEach-Object {
        "            $(To-PascalCase -Value $_.COLUMN_NAME) = $(Get-ReaderExpression -Column $_)"
    }) -join ",`r`n"

    $createColumns = @()
    foreach ($column in $tableColumns) {
        $type = $column.DATA_TYPE.ToLowerInvariant()
        if ($type -in @("timestamp", "rowversion")) { continue }
        if ($column.COLUMN_NAME -eq $primaryColumn.COLUMN_NAME -and (Is-IdentityLikePrimaryKey -PrimaryColumn $primaryColumn)) { continue }
        $createColumns += $column
    }

    $updateColumns = @()
    foreach ($column in $tableColumns) {
        $type = $column.DATA_TYPE.ToLowerInvariant()
        if ($type -in @("timestamp", "rowversion")) { continue }
        if ($column.COLUMN_NAME -eq $primaryColumn.COLUMN_NAME) { continue }
        $updateColumns += $column
    }

    $createParams = ($createColumns | ForEach-Object {
        $size = if ($_.DATA_TYPE.ToLowerInvariant() -in @("varchar", "nvarchar", "char", "nchar", "binary", "varbinary") -and $null -ne $_.CHARACTER_MAXIMUM_LENGTH) { ", $($_.CHARACTER_MAXIMUM_LENGTH)" } else { "" }
        "        command.Parameters.Add(CreateParameter(`"@$($_.COLUMN_NAME)`", SqlDbType.$(Get-SqlDbType -SqlType $_.DATA_TYPE), dto.$(To-PascalCase -Value $_.COLUMN_NAME)$size));"
    }) -join "`r`n"

    $updateParams = ($tableColumns | ForEach-Object {
        if ($_.DATA_TYPE.ToLowerInvariant() -in @("timestamp", "rowversion")) { return }
        $size = if ($_.DATA_TYPE.ToLowerInvariant() -in @("varchar", "nvarchar", "char", "nchar", "binary", "varbinary") -and $null -ne $_.CHARACTER_MAXIMUM_LENGTH) { ", $($_.CHARACTER_MAXIMUM_LENGTH)" } else { "" }
        "        command.Parameters.Add(CreateParameter(`"@$($_.COLUMN_NAME)`", SqlDbType.$(Get-SqlDbType -SqlType $_.DATA_TYPE), dto.$(To-PascalCase -Value $_.COLUMN_NAME)$size));"
    } | Where-Object { $_ }) -join "`r`n"

    $returnConversion = switch ($primaryTypeNonNullable) {
        "long" { "Convert.ToInt64(result)" }
        "int" { "Convert.ToInt32(result)" }
        "short" { "Convert.ToInt16(result)" }
        "byte" { "Convert.ToByte(result)" }
        "Guid" { "(result is Guid value ? value : Guid.Parse(result?.ToString() ?? Guid.Empty.ToString()))" }
        "string" { "result?.ToString() ?? string.Empty" }
        default { "($primaryTypeNonNullable)result!" }
    }

    $repositoryContent = @"
using System.Data;
using System.Data.SqlClient;
using Secure.Platform.Contracts.Dtos.$schemaName;
using Secure.Platform.Data.Repositories.Interfaces.$schemaName;
using Secure.Platform.Data.Sql;

namespace Secure.Platform.Data.Repositories.$schemaName;

/// <summary>
/// Repositorio ADO.NET para $schema.$tableName con stored procedures.
/// Autor: Mario Gomez.
/// </summary>
public sealed class ${entityName}Repository : I${entityName}Repository
{
    private const string SpListar = "$schema.usp_${tableName}_listar";
    private const string SpObtener = "$schema.usp_${tableName}_obtener";
    private const string SpCrear = "$schema.usp_${tableName}_crear";
    private const string SpActualizar = "$schema.usp_${tableName}_actualizar";
    private const string SpDesactivar = "$schema.usp_${tableName}_desactivar";

    private readonly IDbConnectionFactory _connectionFactory;

    public ${entityName}Repository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IReadOnlyList<${entityName}Dto>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = new List<${entityName}Dto>();
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpListar;
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            result.Add(new ${entityName}Dto
            {
$mapProperties
            });
        }
        return result;
    }

    public async Task<${entityName}Dto?> ObtenerAsync($primaryTypeNonNullable $primaryArg, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpObtener;
        command.Parameters.Add(CreateParameter("@$($primaryColumn.COLUMN_NAME)", SqlDbType.$primaryDbType, $primaryArg$primarySizeExpr));
        using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            return new ${entityName}Dto
            {
$mapProperties
            };
        }
        return null;
    }

    public async Task<$primaryTypeNonNullable> CrearAsync(${entityName}Dto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpCrear;
$createParams
        var result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return $returnConversion;
    }

    public async Task<bool> ActualizarAsync(${entityName}Dto dto, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpActualizar;
$updateParams
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    public async Task<bool> DesactivarAsync($primaryTypeNonNullable $primaryArg, string? usuario, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.StoredProcedure;
        command.CommandText = SpDesactivar;
        command.Parameters.Add(CreateParameter("@$($primaryColumn.COLUMN_NAME)", SqlDbType.$primaryDbType, $primaryArg$primarySizeExpr));
        command.Parameters.Add(CreateParameter("@usuario", SqlDbType.VarChar, usuario, 180));
        var affected = await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
        return affected > 0;
    }

    private static SqlParameter CreateParameter(string name, SqlDbType type, object? value, int? size = null)
    {
        var parameter = size.HasValue ? new SqlParameter(name, type, size.Value) : new SqlParameter(name, type);
        parameter.Value = value ?? DBNull.Value;
        return parameter;
    }
}
"@

    Write-FileUtf8 -Path (Join-Path $srcRoot "Secure.Data\Repositories\$schema\${entityName}Repository.cs") -Content $repositoryContent.Trim()

    $controllerContent = @"
using Microsoft.AspNetCore.Mvc;
using Secure.Platform.Contracts.Dtos.$schemaName;
using Secure.Platform.Data.Repositories.Interfaces.$schemaName;

namespace Secure.Platform.Api.Controllers.V1.$schemaName;

/// <summary>
/// Controller API v1 para la tabla $schema.$tableName.
/// Autor: Mario Gomez.
/// </summary>
[ApiController]
[Route("api/v1/$schema/$tableName")]
public sealed class ${entityName}Controller : ControllerBase
{
    private readonly I${entityName}Repository _repository;

    public ${entityName}Controller(I${entityName}Repository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<${entityName}Dto>>> ListarAsync(CancellationToken cancellationToken)
    {
        var result = await _repository.ListarAsync(cancellationToken).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("{$primaryArg}")]
    public async Task<ActionResult<${entityName}Dto>> ObtenerAsync([FromRoute] $primaryTypeNonNullable $primaryArg, CancellationToken cancellationToken)
    {
        var dto = await _repository.ObtenerAsync($primaryArg, cancellationToken).ConfigureAwait(false);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<object>> CrearAsync([FromBody] ${entityName}Dto dto, CancellationToken cancellationToken)
    {
        var id = await _repository.CrearAsync(dto, cancellationToken).ConfigureAwait(false);
        return CreatedAtAction(nameof(ObtenerAsync), new { $primaryArg = id }, new { id });
    }

    [HttpPut("{$primaryArg}")]
    public async Task<ActionResult> ActualizarAsync([FromRoute] $primaryTypeNonNullable $primaryArg, [FromBody] ${entityName}Dto dto, CancellationToken cancellationToken)
    {
        dto.$primaryProperty = $primaryArg;
        var ok = await _repository.ActualizarAsync(dto, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }

    [HttpDelete("{$primaryArg}")]
    public async Task<ActionResult> DesactivarAsync([FromRoute] $primaryTypeNonNullable $primaryArg, CancellationToken cancellationToken)
    {
        var ok = await _repository.DesactivarAsync($primaryArg, User?.Identity?.Name, cancellationToken).ConfigureAwait(false);
        return ok ? Ok() : NotFound();
    }
}
"@

    Write-FileUtf8 -Path (Join-Path $srcRoot "Secure.Api\Controllers\V1\$schema\${entityName}Controller.cs") -Content $controllerContent.Trim()

    $listColumnsSql = ($tableColumns | ForEach-Object { "[$($_.COLUMN_NAME)]" }) -join ", "
    $createDeclaration = ($createColumns | ForEach-Object { "@$($_.COLUMN_NAME) $(Get-SqlTypeDeclaration -Column $_)" }) -join ",`r`n    "
    $createColumnList = ($createColumns | ForEach-Object { "[$($_.COLUMN_NAME)]" }) -join ", "
    $createValueList = ($createColumns | ForEach-Object { "@$($_.COLUMN_NAME)" }) -join ", "
    $updateDeclaration = ($tableColumns | Where-Object { $_.DATA_TYPE.ToLowerInvariant() -notin @("timestamp", "rowversion") } | ForEach-Object { "@$($_.COLUMN_NAME) $(Get-SqlTypeDeclaration -Column $_)" }) -join ",`r`n    "
    $updateSetClause = ($updateColumns | ForEach-Object { "[$($_.COLUMN_NAME)] = @$($_.COLUMN_NAME)" }) -join ",`r`n        "
    if ([string]::IsNullOrWhiteSpace($updateSetClause)) {
        $updateSetClause = "[$($primaryColumn.COLUMN_NAME)] = @$($primaryColumn.COLUMN_NAME)"
    }

    $createReturn = if (Is-IdentityLikePrimaryKey -PrimaryColumn $primaryColumn) {
        "SELECT CAST(SCOPE_IDENTITY() AS $primarySqlType) AS id;"
    }
    else {
        "SELECT @$($primaryColumn.COLUMN_NAME) AS id;"
    }

    $hasActivo = $tableColumns.COLUMN_NAME -contains "activo"
    $hasActualizadoUtc = $tableColumns.COLUMN_NAME -contains "actualizado_utc"
    if ($hasActivo) {
        $setClause = "[activo] = 0"
        if ($hasActualizadoUtc) {
            $setClause += ",`r`n        [actualizado_utc] = SYSUTCDATETIME()"
        }
        $deactivateStatement = @"
    UPDATE $schema.$tableName
    SET $setClause
    WHERE [$($primaryColumn.COLUMN_NAME)] = @$($primaryColumn.COLUMN_NAME);
"@
    }
    else {
        $deactivateStatement = @"
    DELETE FROM $schema.$tableName
    WHERE [$($primaryColumn.COLUMN_NAME)] = @$($primaryColumn.COLUMN_NAME);
"@
    }

    $storedProceduresContent = @"
CREATE OR ALTER PROCEDURE $schema.usp_${tableName}_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT $listColumnsSql
    FROM $schema.$tableName;
END
GO

CREATE OR ALTER PROCEDURE $schema.usp_${tableName}_obtener
    @$($primaryColumn.COLUMN_NAME) $primarySqlType
AS
BEGIN
    SET NOCOUNT ON;
    SELECT $listColumnsSql
    FROM $schema.$tableName
    WHERE [$($primaryColumn.COLUMN_NAME)] = @$($primaryColumn.COLUMN_NAME);
END
GO

CREATE OR ALTER PROCEDURE $schema.usp_${tableName}_crear
    $createDeclaration
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO $schema.$tableName ($createColumnList)
    VALUES ($createValueList);
    $createReturn
END
GO

CREATE OR ALTER PROCEDURE $schema.usp_${tableName}_actualizar
    $updateDeclaration
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE $schema.$tableName
    SET $updateSetClause
    WHERE [$($primaryColumn.COLUMN_NAME)] = @$($primaryColumn.COLUMN_NAME);
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE $schema.usp_${tableName}_desactivar
    @$($primaryColumn.COLUMN_NAME) $primarySqlType,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
$deactivateStatement
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
"@

    Write-FileUtf8 -Path (Join-Path $RepoRoot "database\stored-procedures\$schema\usp_${tableName}_crud.sql") -Content $storedProceduresContent.Trim()
}

Write-Host "Generacion finalizada: tablas=$($tables.Count), columnas=$($columns.Count), fk=$($foreignKeys.Count)"
