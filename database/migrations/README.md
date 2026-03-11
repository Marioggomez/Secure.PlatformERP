# Versionado de Base de Datos (SQL Server)

Este proyecto versiona la base de datos con migraciones SQL `UP/DOWN` almacenadas en repositorio.

## Estructura

- `database/migrations/<id>__<descripcion>.up.sql`
- `database/migrations/<id>__<descripcion>.down.sql`

Ejemplo:

- `20260311193000__baseline_repositorio.up.sql`
- `20260311193000__baseline_repositorio.down.sql`

## Tabla de historial

El script de migraciones crea/usa la tabla:

- `plataforma.db_migration_history`

Columnas:

- `migration_id`
- `script_name`
- `checksum_sha256`
- `applied_utc`
- `applied_by`

## Comandos

Desde la raiz del repositorio:

```powershell
# Ver estado (aplicadas vs pendientes)
powershell -ExecutionPolicy Bypass -File .\tools\db-migrate.ps1 -Action status

# Aplicar una migracion pendiente
powershell -ExecutionPolicy Bypass -File .\tools\db-migrate.ps1 -Action up -Step 1

# Aplicar todas las pendientes (o hasta target)
powershell -ExecutionPolicy Bypass -File .\tools\db-migrate.ps1 -Action up
powershell -ExecutionPolicy Bypass -File .\tools\db-migrate.ps1 -Action up -Target 20260311193000

# Rollback de la ultima migracion aplicada
powershell -ExecutionPolicy Bypass -File .\tools\db-migrate.ps1 -Action down -Step 1

# Rollback hasta una version objetivo (deja esa version aplicada)
powershell -ExecutionPolicy Bypass -File .\tools\db-migrate.ps1 -Action down -Target 20260311193000

# Rollback total
powershell -ExecutionPolicy Bypass -File .\tools\db-migrate.ps1 -Action down -Target 0
```

## Generar nueva migracion

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\new-db-migration.ps1 -Name "agregar_indice_usuario_login"
```

Esto crea archivos `up/down` con el mismo `id`.

## Backup previo (opcional)

Puede forzar backup antes de aplicar/revertir:

```powershell
powershell -ExecutionPolicy Bypass -File .\tools\db-migrate.ps1 `
  -Action up `
  -BackupBeforeChange `
  -BackupFile "\\servidor\sqlbackups\Secure_before_up_20260311.bak"
```

`-BackupFile` debe ser una ruta accesible por el servicio SQL Server.
