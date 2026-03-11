# Secure Platform ERP

Plataforma enterprise ERP + IAM construida con:

- .NET 8
- SQL Server
- ADO.NET puro
- Stored Procedures
- API REST versionada `/api/v1/`
- WinForms VB.NET con arquitectura preparada para DevExpress 25

## Autor

Mario Gomez

## Solucion

- `src/Secure.Platform.sln`
- `src/Secure.Api`
- `src/Secure.Application`
- `src/Secure.Domain`
- `src/Secure.Infrastructure`
- `src/Secure.Data`
- `src/Secure.Contracts`
- `src/Secure.Common`
- `src/Secure.WinForms`

## Esquema de base de datos

Lectura realizada exclusivamente con:

- `INFORMATION_SCHEMA.TABLES`
- `INFORMATION_SCHEMA.COLUMNS`
- `sys.foreign_keys`

Artefactos generados:

- `database/schema-snapshots/secure-schema.json`
- `database/schema-snapshots/schema-report.md`
- `database/stored-procedures/<schema>/usp_<tabla>_crud.sql`

## Versionado de Base de Datos

El versionado SQL esta en:

- `database/migrations`
- `tools/db-migrate.ps1` (status/up/down/rollback)
- `tools/new-db-migration.ps1` (scaffold de nuevas migraciones)

Guia:

- `database/migrations/README.md`

## Build

API y capas C#:

```powershell
dotnet build .\src\Secure.Api\Secure.Api.csproj
```

Solucion completa:

```powershell
dotnet build .\src\Secure.Platform.sln
```

Nota: el proyecto `Secure.WinForms` requiere assemblies DevExpress 25 instalados en el entorno.

## Configuracion de base de datos

- El archivo local `database.config.json` es privado y no se versiona.
- Usa `database.config.example.json` como plantilla para crear tu configuracion local.
