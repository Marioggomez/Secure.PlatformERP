param(
    [Parameter(Mandatory = $true)]
    [string]$Name,
    [string]$MigrationsPath = ""
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$repoRoot = [System.IO.Path]::GetFullPath((Join-Path -Path $PSScriptRoot -ChildPath ".."))
if ([string]::IsNullOrWhiteSpace($MigrationsPath)) {
    $MigrationsPath = Join-Path -Path $repoRoot -ChildPath "database/migrations"
}

$MigrationsPath = [System.IO.Path]::GetFullPath($MigrationsPath)
if (-not (Test-Path -Path $MigrationsPath)) {
    New-Item -Path $MigrationsPath -ItemType Directory -Force | Out-Null
}

$slug = ($Name.ToLowerInvariant() -replace "[^a-z0-9]+", "_").Trim("_")
if ([string]::IsNullOrWhiteSpace($slug)) {
    throw "No se pudo generar slug de migracion. Use un nombre con letras o numeros."
}

$id = (Get-Date).ToUniversalTime().ToString("yyyyMMddHHmmss")
$baseName = "$id" + "__" + "$slug"
$upFile = Join-Path -Path $MigrationsPath -ChildPath ($baseName + ".up.sql")
$downFile = Join-Path -Path $MigrationsPath -ChildPath ($baseName + ".down.sql")

if ((Test-Path -Path $upFile) -or (Test-Path -Path $downFile)) {
    throw "Ya existe una migracion con el id $id. Intente de nuevo en un segundo."
}

$header = @"
/*
    Migracion: $baseName
    Autor: Mario Gomez
    Fecha UTC: $((Get-Date).ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss"))
*/
"@

$upTemplate = @"
$header
-- TODO: implementar cambios UP
PRINT N'Pendiente implementar migracion UP: $baseName';
"@

$downTemplate = @"
$header
-- TODO: implementar rollback DOWN
PRINT N'Pendiente implementar migracion DOWN: $baseName';
"@

Set-Content -Path $upFile -Value $upTemplate -Encoding UTF8
Set-Content -Path $downFile -Value $downTemplate -Encoding UTF8

Write-Host "Migracion creada:"
Write-Host "  UP   : $upFile"
Write-Host "  DOWN : $downFile"
