param(
    [ValidateSet("status", "up", "down")]
    [string]$Action = "status",
    [string]$Target = "",
    [int]$Step = 1,
    [string]$ConfigPath = "",
    [string]$MigrationsPath = "",
    [switch]$BackupBeforeChange,
    [string]$BackupFile = ""
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

function Resolve-NormalPath {
    param([string]$PathValue)

    if ([string]::IsNullOrWhiteSpace($PathValue)) {
        return $null
    }

    return [System.IO.Path]::GetFullPath($PathValue)
}

function Get-DbConfig {
    param([string]$PathValue)

    if (-not (Test-Path -Path $PathValue)) {
        throw "No existe el archivo de configuracion: $PathValue"
    }

    $json = Get-Content -Path $PathValue -Raw | ConvertFrom-Json
    if ($null -eq $json -or [string]::IsNullOrWhiteSpace($json.connectionString)) {
        throw "El archivo $PathValue no contiene connectionString."
    }

    $builder = New-Object System.Data.SqlClient.SqlConnectionStringBuilder($json.connectionString)

    $config = [ordered]@{
        Provider = if ($null -ne $json.provider) { [string]$json.provider } else { "sqlserver" }
        ConnectionString = [string]$json.connectionString
        Server = [string]$builder.DataSource
        Database = [string]$builder.InitialCatalog
        UserId = [string]$builder.UserID
        Password = [string]$builder.Password
        IntegratedSecurity = [bool]$builder.IntegratedSecurity
        TrustServerCertificate = [bool]$builder.TrustServerCertificate
    }

    if ([string]::IsNullOrWhiteSpace($config.Server) -or [string]::IsNullOrWhiteSpace($config.Database)) {
        throw "ConnectionString invalida: falta servidor o base de datos."
    }

    return [PSCustomObject]$config
}

function New-SqlConnection {
    param([string]$ConnectionString)

    $connection = New-Object System.Data.SqlClient.SqlConnection($ConnectionString)
    $connection.Open()
    return $connection
}

function Ensure-MigrationHistoryTable {
    param([System.Data.SqlClient.SqlConnection]$Connection)

    $sql = @"
IF SCHEMA_ID(N'plataforma') IS NULL
BEGIN
    EXEC(N'CREATE SCHEMA plataforma AUTHORIZATION dbo;');
END;

IF OBJECT_ID(N'plataforma.db_migration_history', N'U') IS NULL
BEGIN
    CREATE TABLE plataforma.db_migration_history
    (
        migration_id NVARCHAR(128) NOT NULL PRIMARY KEY,
        script_name NVARCHAR(260) NOT NULL,
        checksum_sha256 CHAR(64) NOT NULL,
        applied_utc DATETIME2(0) NOT NULL CONSTRAINT DF_db_migration_history_applied_utc DEFAULT SYSUTCDATETIME(),
        applied_by NVARCHAR(128) NOT NULL
    );
END;
"@

    $command = $Connection.CreateCommand()
    $command.CommandText = $sql
    [void]$command.ExecuteNonQuery()
}

function Get-AppliedMigrationIds {
    param([System.Data.SqlClient.SqlConnection]$Connection)

    $result = New-Object System.Collections.Generic.List[string]
    $command = $Connection.CreateCommand()
    $command.CommandText = "SELECT migration_id FROM plataforma.db_migration_history ORDER BY migration_id;"
    $reader = $command.ExecuteReader()
    try {
        while ($reader.Read()) {
            $result.Add($reader.GetString(0))
        }
    }
    finally {
        $reader.Close()
    }

    return $result
}

function Add-AppliedMigration {
    param(
        [System.Data.SqlClient.SqlConnection]$Connection,
        [string]$MigrationId,
        [string]$ScriptName,
        [string]$Checksum,
        [string]$AppliedBy
    )

    $command = $Connection.CreateCommand()
    $command.CommandText = @"
INSERT INTO plataforma.db_migration_history
(
    migration_id,
    script_name,
    checksum_sha256,
    applied_by
)
VALUES
(
    @migration_id,
    @script_name,
    @checksum_sha256,
    @applied_by
);
"@

    [void]$command.Parameters.Add("@migration_id", [System.Data.SqlDbType]::NVarChar, 128)
    [void]$command.Parameters.Add("@script_name", [System.Data.SqlDbType]::NVarChar, 260)
    [void]$command.Parameters.Add("@checksum_sha256", [System.Data.SqlDbType]::Char, 64)
    [void]$command.Parameters.Add("@applied_by", [System.Data.SqlDbType]::NVarChar, 128)

    $command.Parameters["@migration_id"].Value = $MigrationId
    $command.Parameters["@script_name"].Value = $ScriptName
    $command.Parameters["@checksum_sha256"].Value = $Checksum
    $command.Parameters["@applied_by"].Value = $AppliedBy

    [void]$command.ExecuteNonQuery()
}

function Remove-AppliedMigration {
    param(
        [System.Data.SqlClient.SqlConnection]$Connection,
        [string]$MigrationId
    )

    $command = $Connection.CreateCommand()
    $command.CommandText = "DELETE FROM plataforma.db_migration_history WHERE migration_id = @migration_id;"
    [void]$command.Parameters.Add("@migration_id", [System.Data.SqlDbType]::NVarChar, 128)
    $command.Parameters["@migration_id"].Value = $MigrationId
    [void]$command.ExecuteNonQuery()
}

function Get-Migrations {
    param([string]$PathValue)

    if (-not (Test-Path -Path $PathValue)) {
        throw "No existe la carpeta de migraciones: $PathValue"
    }

    $files = Get-ChildItem -Path $PathValue -File -Filter "*.up.sql" | Sort-Object Name
    $seenIds = New-Object System.Collections.Generic.HashSet[string]
    $items = New-Object System.Collections.Generic.List[psobject]

    foreach ($file in $files) {
        if ($file.Name -notmatch "^(?<id>[0-9A-Za-z_-]+)__(?<name>.+)\.up\.sql$") {
            throw "Nombre invalido de migracion: $($file.Name). Formato: <id>__<descripcion>.up.sql"
        }

        $id = $Matches["id"]
        $name = $Matches["name"]
        if (-not $seenIds.Add($id)) {
            throw "ID de migracion duplicado: $id"
        }

        $baseName = $file.Name -replace "\.up\.sql$", ""
        $downFile = Join-Path -Path $file.DirectoryName -ChildPath ($baseName + ".down.sql")
        if (-not (Test-Path -Path $downFile)) {
            throw "No existe el script DOWN de la migracion $($file.Name): $downFile"
        }

        $hash = (Get-FileHash -Path $file.FullName -Algorithm SHA256).Hash.ToLowerInvariant()

        $items.Add([PSCustomObject]@{
            Id = $id
            Name = $name
            UpFileName = $file.Name
            UpPath = $file.FullName
            DownPath = $downFile
            Checksum = $hash
        })
    }

    return $items
}

function Invoke-SqlcmdScript {
    param(
        [psobject]$Config,
        [string]$ScriptPath
    )

    $args = New-Object System.Collections.Generic.List[string]
    $args.Add("-S")
    $args.Add($Config.Server)
    $args.Add("-d")
    $args.Add($Config.Database)
    $args.Add("-b")
    $args.Add("-i")
    $args.Add($ScriptPath)

    if ($Config.IntegratedSecurity) {
        $args.Add("-E")
    }
    else {
        $args.Add("-U")
        $args.Add($Config.UserId)
        $args.Add("-P")
        $args.Add($Config.Password)
    }

    if ($Config.TrustServerCertificate) {
        $args.Add("-C")
    }

    & sqlcmd @args
    if ($LASTEXITCODE -ne 0) {
        throw "Fallo la ejecucion sqlcmd para el script: $ScriptPath"
    }
}

function Invoke-MigrationScriptTransactional {
    param(
        [psobject]$Config,
        [string]$InnerScriptPath
    )

    if (-not (Test-Path -Path $InnerScriptPath)) {
        throw "No existe script de migracion: $InnerScriptPath"
    }

    $safePath = $InnerScriptPath.Replace("""", """""")
    $tempFile = [System.IO.Path]::GetTempFileName() + ".sql"

    $wrapper = @"
SET NOCOUNT ON;
SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;
    :r "$safePath"
    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
    DECLARE @ErrorState INT = ERROR_STATE();
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH;
"@

    try {
        Set-Content -Path $tempFile -Value $wrapper -Encoding UTF8
        Invoke-SqlcmdScript -Config $Config -ScriptPath $tempFile
    }
    finally {
        if (Test-Path -Path $tempFile) {
            Remove-Item -Path $tempFile -Force
        }
    }
}

function Backup-Database {
    param(
        [System.Data.SqlClient.SqlConnection]$Connection,
        [string]$BackupFilePath
    )

    $sql = @"
BACKUP DATABASE [$($Connection.Database)]
TO DISK = N'$($BackupFilePath.Replace("'", "''"))'
WITH INIT, COPY_ONLY, COMPRESSION, CHECKSUM, STATS = 10;
"@

    $command = $Connection.CreateCommand()
    $command.CommandText = $sql
    $command.CommandTimeout = 0
    [void]$command.ExecuteNonQuery()
}

function Print-Status {
    param(
        [object[]]$Migrations,
        [object[]]$AppliedIds
    )

    $appliedSet = New-Object System.Collections.Generic.HashSet[string]
    foreach ($id in $AppliedIds) {
        [void]$appliedSet.Add([string]$id)
    }
    Write-Host ""
    Write-Host "Estado de migraciones:"
    Write-Host "----------------------"

    foreach ($migration in $Migrations) {
        $state = if ($appliedSet.Contains($migration.Id)) { "APLICADA" } else { "PENDIENTE" }
        Write-Host ("{0,-10} {1,-22} {2}" -f $state, $migration.Id, $migration.UpFileName)
    }

    Write-Host ""
    $total = @($Migrations).Count
    $appliedCount = @($AppliedIds).Count
    Write-Host ("Total: {0} | Aplicadas: {1} | Pendientes: {2}" -f $total, $appliedCount, ($total - $appliedCount))
}

$repoRoot = Resolve-NormalPath (Join-Path -Path $PSScriptRoot -ChildPath "..")
if ([string]::IsNullOrWhiteSpace($ConfigPath)) {
    $ConfigPath = Join-Path -Path $repoRoot -ChildPath "database.config.json"
}
if ([string]::IsNullOrWhiteSpace($MigrationsPath)) {
    $MigrationsPath = Join-Path -Path $repoRoot -ChildPath "database/migrations"
}

$ConfigPath = Resolve-NormalPath $ConfigPath
$MigrationsPath = Resolve-NormalPath $MigrationsPath

if ($Step -lt 1) {
    throw "El parametro -Step debe ser mayor o igual a 1."
}

$dbConfig = Get-DbConfig -PathValue $ConfigPath
$migrations = @(Get-Migrations -PathValue $MigrationsPath)

if (-not (Get-Command sqlcmd -ErrorAction SilentlyContinue)) {
    throw "No se encontro sqlcmd en el PATH."
}

$connection = New-SqlConnection -ConnectionString $dbConfig.ConnectionString
try {
    Ensure-MigrationHistoryTable -Connection $connection
    $appliedIds = @(Get-AppliedMigrationIds -Connection $connection)

    if ($Action -eq "status") {
        Print-Status -Migrations $migrations -AppliedIds $appliedIds
        return
    }

    $changesRequireBackup = $BackupBeforeChange -and ($Action -in @("up", "down"))
    if ($changesRequireBackup) {
        if ([string]::IsNullOrWhiteSpace($BackupFile)) {
            throw "Si usa -BackupBeforeChange debe indicar -BackupFile con una ruta accesible por SQL Server."
        }

        Write-Host "Generando backup previo: $BackupFile"
        Backup-Database -Connection $connection -BackupFilePath $BackupFile
        Write-Host "Backup completado."
    }

    $appliedSet = New-Object System.Collections.Generic.HashSet[string]
    foreach ($id in $appliedIds) {
        [void]$appliedSet.Add([string]$id)
    }
    $appliedBy = if ([string]::IsNullOrWhiteSpace($env:USERNAME)) { "unknown" } else { $env:USERNAME }

    if ($Action -eq "up") {
        $pending = $migrations | Where-Object { -not $appliedSet.Contains($_.Id) }

        if (-not [string]::IsNullOrWhiteSpace($Target)) {
            $exists = $migrations | Where-Object { $_.Id -eq $Target } | Select-Object -First 1
            if ($null -eq $exists) {
                throw "No existe la migracion objetivo: $Target"
            }
            $pending = $pending | Where-Object { $_.Id -le $Target }
        }
        else {
            $pending = $pending | Select-Object -First $Step
        }

        if (@($pending).Count -eq 0) {
            Write-Host "No hay migraciones pendientes para aplicar."
            return
        }

        foreach ($migration in $pending) {
            Write-Host "Aplicando migracion $($migration.Id) - $($migration.UpFileName)"
            Invoke-MigrationScriptTransactional -Config $dbConfig -InnerScriptPath $migration.UpPath
            Add-AppliedMigration -Connection $connection -MigrationId $migration.Id -ScriptName $migration.UpFileName -Checksum $migration.Checksum -AppliedBy $appliedBy
            Write-Host "OK: $($migration.Id)"
        }

        return
    }

    if ($Action -eq "down") {
        $appliedInRepoOrder = $migrations | Where-Object { $appliedSet.Contains($_.Id) } | Sort-Object Id -Descending

        $toRollback = @()
        if (-not [string]::IsNullOrWhiteSpace($Target)) {
            if ($Target -eq "0") {
                $toRollback = $appliedInRepoOrder
            }
            else {
                $exists = $migrations | Where-Object { $_.Id -eq $Target } | Select-Object -First 1
                if ($null -eq $exists) {
                    throw "No existe la migracion objetivo: $Target"
                }
                $toRollback = $appliedInRepoOrder | Where-Object { $_.Id -gt $Target }
            }
        }
        else {
            $toRollback = $appliedInRepoOrder | Select-Object -First $Step
        }

        $rollbackList = @($toRollback)
        if ($rollbackList.Count -eq 0) {
            Write-Host "No hay migraciones para revertir."
            return
        }

        foreach ($migration in $rollbackList) {
            Write-Host "Revirtiendo migracion $($migration.Id) - $([System.IO.Path]::GetFileName($migration.DownPath))"
            Invoke-MigrationScriptTransactional -Config $dbConfig -InnerScriptPath $migration.DownPath
            Remove-AppliedMigration -Connection $connection -MigrationId $migration.Id
            Write-Host "OK rollback: $($migration.Id)"
        }

        return
    }
}
finally {
    if ($null -ne $connection) {
        $connection.Dispose()
    }
}
