
/*
    Migracion: 20260314193000__enterprise_scope_authorization_hardening
    Objetivo:
    - Endurecer aislamiento tenant/empresa en tablas operativas auxiliares.
    - Eliminar ambiguedad de FKs simples vs compuestas.
    - Separar preferencias de UI de filtros de seguridad de datos.
    - Crear capa canonica de resolucion de alcance efectivo.
    - Preparar base para RLS (estado OFF).
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ============================================================
   1) Tenant materializado en tablas empresa-scoped sin tenant
   ============================================================ */
IF COL_LENGTH('seguridad.usuario_scope_empresa', 'id_tenant') IS NULL
    ALTER TABLE seguridad.usuario_scope_empresa ADD id_tenant BIGINT NULL;

IF COL_LENGTH('seguridad.usuario_scope_unidad', 'id_tenant') IS NULL
    ALTER TABLE seguridad.usuario_scope_unidad ADD id_tenant BIGINT NULL;

IF COL_LENGTH('seguridad.usuario_scope_unidad', 'id_empresa') IS NULL
    ALTER TABLE seguridad.usuario_scope_unidad ADD id_empresa BIGINT NULL;

IF COL_LENGTH('tercero.tercero_empresa', 'id_tenant') IS NULL
    ALTER TABLE tercero.tercero_empresa ADD id_tenant BIGINT NULL;

IF COL_LENGTH('tercero.tercero_rol', 'id_tenant') IS NULL
    ALTER TABLE tercero.tercero_rol ADD id_tenant BIGINT NULL;

IF COL_LENGTH('plataforma.configuracion_empresa', 'id_tenant') IS NULL
    ALTER TABLE plataforma.configuracion_empresa ADD id_tenant BIGINT NULL;

IF COL_LENGTH('seguridad.politica_empresa_override', 'id_tenant') IS NULL
    ALTER TABLE seguridad.politica_empresa_override ADD id_tenant BIGINT NULL;

IF COL_LENGTH('cumplimiento.accion_instancia_aprobacion', 'id_tenant') IS NULL
    ALTER TABLE cumplimiento.accion_instancia_aprobacion ADD id_tenant BIGINT NULL;

IF COL_LENGTH('cumplimiento.excepcion_sod', 'id_tenant') IS NULL
    ALTER TABLE cumplimiento.excepcion_sod ADD id_tenant BIGINT NULL;

IF COL_LENGTH('organizacion.grupo_empresarial_empresa', 'id_tenant') IS NULL
    ALTER TABLE organizacion.grupo_empresarial_empresa ADD id_tenant BIGINT NULL;

IF COL_LENGTH('logistica.recargo_regla', 'id_tenant') IS NULL
    ALTER TABLE logistica.recargo_regla ADD id_tenant BIGINT NULL;
GO

/* Backfill tenant/empresa desde organizacion.empresa / unidad_organizativa */
UPDATE usemp
SET usemp.id_tenant = e.id_tenant
FROM seguridad.usuario_scope_empresa usemp
INNER JOIN organizacion.empresa e ON e.id_empresa = usemp.id_empresa
WHERE usemp.id_tenant IS NULL;

UPDATE usu
SET usu.id_empresa = uo.id_empresa,
    usu.id_tenant = uo.id_tenant
FROM seguridad.usuario_scope_unidad usu
INNER JOIN organizacion.unidad_organizativa uo
    ON uo.id_unidad_organizativa = usu.id_unidad_organizativa
WHERE usu.id_empresa IS NULL OR usu.id_tenant IS NULL;

UPDATE te
SET te.id_tenant = e.id_tenant
FROM tercero.tercero_empresa te
INNER JOIN organizacion.empresa e ON e.id_empresa = te.id_empresa
WHERE te.id_tenant IS NULL;

UPDATE tr
SET tr.id_tenant = e.id_tenant
FROM tercero.tercero_rol tr
INNER JOIN organizacion.empresa e ON e.id_empresa = tr.id_empresa
WHERE tr.id_empresa IS NOT NULL
  AND tr.id_tenant IS NULL;

UPDATE ce
SET ce.id_tenant = e.id_tenant
FROM plataforma.configuracion_empresa ce
INNER JOIN organizacion.empresa e ON e.id_empresa = ce.id_empresa
WHERE ce.id_tenant IS NULL;

UPDATE peo
SET peo.id_tenant = e.id_tenant
FROM seguridad.politica_empresa_override peo
INNER JOIN organizacion.empresa e ON e.id_empresa = peo.id_empresa
WHERE peo.id_tenant IS NULL;

UPDATE aia
SET aia.id_tenant = e.id_tenant
FROM cumplimiento.accion_instancia_aprobacion aia
INNER JOIN organizacion.empresa e ON e.id_empresa = aia.id_empresa
WHERE aia.id_tenant IS NULL;

UPDATE esod
SET esod.id_tenant = e.id_tenant
FROM cumplimiento.excepcion_sod esod
INNER JOIN organizacion.empresa e ON e.id_empresa = esod.id_empresa
WHERE esod.id_tenant IS NULL;

UPDATE gee
SET gee.id_tenant = e.id_tenant
FROM organizacion.grupo_empresarial_empresa gee
INNER JOIN organizacion.empresa e ON e.id_empresa = gee.id_empresa
WHERE gee.id_tenant IS NULL;

UPDATE rr
SET rr.id_tenant = e.id_tenant
FROM logistica.recargo_regla rr
INNER JOIN organizacion.empresa e ON e.id_empresa = rr.id_empresa
WHERE rr.id_tenant IS NULL;
GO
/* Validacion de consistencia antes de NOT NULL */
IF EXISTS (SELECT 1 FROM seguridad.usuario_scope_empresa WHERE id_tenant IS NULL)
    THROW 51100, N'No fue posible materializar id_tenant en seguridad.usuario_scope_empresa.', 1;
IF EXISTS (SELECT 1 FROM seguridad.usuario_scope_unidad WHERE id_tenant IS NULL OR id_empresa IS NULL)
    THROW 51101, N'No fue posible materializar id_tenant/id_empresa en seguridad.usuario_scope_unidad.', 1;
IF EXISTS (SELECT 1 FROM tercero.tercero_empresa WHERE id_tenant IS NULL)
    THROW 51102, N'No fue posible materializar id_tenant en tercero.tercero_empresa.', 1;
IF EXISTS (SELECT 1 FROM plataforma.configuracion_empresa WHERE id_tenant IS NULL)
    THROW 51103, N'No fue posible materializar id_tenant en plataforma.configuracion_empresa.', 1;
IF EXISTS (SELECT 1 FROM seguridad.politica_empresa_override WHERE id_tenant IS NULL)
    THROW 51104, N'No fue posible materializar id_tenant en seguridad.politica_empresa_override.', 1;
IF EXISTS (SELECT 1 FROM cumplimiento.accion_instancia_aprobacion WHERE id_tenant IS NULL)
    THROW 51105, N'No fue posible materializar id_tenant en cumplimiento.accion_instancia_aprobacion.', 1;
IF EXISTS (SELECT 1 FROM cumplimiento.excepcion_sod WHERE id_tenant IS NULL)
    THROW 51106, N'No fue posible materializar id_tenant en cumplimiento.excepcion_sod.', 1;
IF EXISTS (SELECT 1 FROM organizacion.grupo_empresarial_empresa WHERE id_tenant IS NULL)
    THROW 51107, N'No fue posible materializar id_tenant en organizacion.grupo_empresarial_empresa.', 1;
IF EXISTS (SELECT 1 FROM logistica.recargo_regla WHERE id_tenant IS NULL)
    THROW 51108, N'No fue posible materializar id_tenant en logistica.recargo_regla.', 1;
GO

/* NOT NULL donde aplica */
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'seguridad.usuario_scope_empresa') AND name = N'id_tenant' AND is_nullable = 1)
    ALTER TABLE seguridad.usuario_scope_empresa ALTER COLUMN id_tenant BIGINT NOT NULL;

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'seguridad.usuario_scope_unidad') AND name = N'id_tenant' AND is_nullable = 1)
    ALTER TABLE seguridad.usuario_scope_unidad ALTER COLUMN id_tenant BIGINT NOT NULL;

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'seguridad.usuario_scope_unidad') AND name = N'id_empresa' AND is_nullable = 1)
    ALTER TABLE seguridad.usuario_scope_unidad ALTER COLUMN id_empresa BIGINT NOT NULL;

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'tercero.tercero_empresa') AND name = N'id_tenant' AND is_nullable = 1)
    ALTER TABLE tercero.tercero_empresa ALTER COLUMN id_tenant BIGINT NOT NULL;

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'plataforma.configuracion_empresa') AND name = N'id_tenant' AND is_nullable = 1)
    ALTER TABLE plataforma.configuracion_empresa ALTER COLUMN id_tenant BIGINT NOT NULL;

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'seguridad.politica_empresa_override') AND name = N'id_tenant' AND is_nullable = 1)
    ALTER TABLE seguridad.politica_empresa_override ALTER COLUMN id_tenant BIGINT NOT NULL;

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'cumplimiento.accion_instancia_aprobacion') AND name = N'id_tenant' AND is_nullable = 1)
    ALTER TABLE cumplimiento.accion_instancia_aprobacion ALTER COLUMN id_tenant BIGINT NOT NULL;

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'cumplimiento.excepcion_sod') AND name = N'id_tenant' AND is_nullable = 1)
    ALTER TABLE cumplimiento.excepcion_sod ALTER COLUMN id_tenant BIGINT NOT NULL;

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'organizacion.grupo_empresarial_empresa') AND name = N'id_tenant' AND is_nullable = 1)
    ALTER TABLE organizacion.grupo_empresarial_empresa ALTER COLUMN id_tenant BIGINT NOT NULL;

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'logistica.recargo_regla') AND name = N'id_tenant' AND is_nullable = 1)
    ALTER TABLE logistica.recargo_regla ALTER COLUMN id_tenant BIGINT NOT NULL;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.check_constraints
    WHERE name = N'CK_tercero_tercero_rol_tenant_empresa_par'
      AND parent_object_id = OBJECT_ID(N'tercero.tercero_rol')
)
BEGIN
    ALTER TABLE tercero.tercero_rol
    ADD CONSTRAINT CK_tercero_tercero_rol_tenant_empresa_par
    CHECK (
        (id_empresa IS NULL AND id_tenant IS NULL)
        OR
        (id_empresa IS NOT NULL AND id_tenant IS NOT NULL)
    );
END;
GO
/* ============================================================
   2) FKs compuestas y limpieza de FKs ambiguas (id_empresa sola)
   ============================================================ */
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_usuario_scope_empresa_empresa_scope' AND parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_empresa'))
BEGIN
    ALTER TABLE seguridad.usuario_scope_empresa WITH CHECK
    ADD CONSTRAINT FK_seguridad_usuario_scope_empresa_empresa_scope
    FOREIGN KEY (id_tenant, id_empresa)
    REFERENCES organizacion.empresa (id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_usuario_scope_unidad_empresa_scope' AND parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_unidad'))
BEGIN
    ALTER TABLE seguridad.usuario_scope_unidad WITH CHECK
    ADD CONSTRAINT FK_seguridad_usuario_scope_unidad_empresa_scope
    FOREIGN KEY (id_tenant, id_empresa)
    REFERENCES organizacion.empresa (id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_usuario_scope_unidad_unidad_scope' AND parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_unidad'))
BEGIN
    ALTER TABLE seguridad.usuario_scope_unidad WITH CHECK
    ADD CONSTRAINT FK_seguridad_usuario_scope_unidad_unidad_scope
    FOREIGN KEY (id_empresa, id_unidad_organizativa)
    REFERENCES organizacion.unidad_organizativa (id_empresa, id_unidad_organizativa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_tercero_empresa_empresa_scope' AND parent_object_id = OBJECT_ID(N'tercero.tercero_empresa'))
BEGIN
    ALTER TABLE tercero.tercero_empresa WITH CHECK
    ADD CONSTRAINT FK_tercero_tercero_empresa_empresa_scope
    FOREIGN KEY (id_tenant, id_empresa)
    REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_tercero_rol_empresa_scope' AND parent_object_id = OBJECT_ID(N'tercero.tercero_rol'))
BEGIN
    ALTER TABLE tercero.tercero_rol WITH CHECK
    ADD CONSTRAINT FK_tercero_tercero_rol_empresa_scope
    FOREIGN KEY (id_tenant, id_empresa)
    REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_plataforma_configuracion_empresa_scope' AND parent_object_id = OBJECT_ID(N'plataforma.configuracion_empresa'))
BEGIN
    ALTER TABLE plataforma.configuracion_empresa WITH CHECK
    ADD CONSTRAINT FK_plataforma_configuracion_empresa_scope
    FOREIGN KEY (id_tenant, id_empresa)
    REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_politica_empresa_override_scope' AND parent_object_id = OBJECT_ID(N'seguridad.politica_empresa_override'))
BEGIN
    ALTER TABLE seguridad.politica_empresa_override WITH CHECK
    ADD CONSTRAINT FK_seguridad_politica_empresa_override_scope
    FOREIGN KEY (id_tenant, id_empresa)
    REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_cumplimiento_accion_instancia_aprobacion_scope' AND parent_object_id = OBJECT_ID(N'cumplimiento.accion_instancia_aprobacion'))
BEGIN
    ALTER TABLE cumplimiento.accion_instancia_aprobacion WITH CHECK
    ADD CONSTRAINT FK_cumplimiento_accion_instancia_aprobacion_scope
    FOREIGN KEY (id_tenant, id_empresa)
    REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_cumplimiento_excepcion_sod_scope' AND parent_object_id = OBJECT_ID(N'cumplimiento.excepcion_sod'))
BEGIN
    ALTER TABLE cumplimiento.excepcion_sod WITH CHECK
    ADD CONSTRAINT FK_cumplimiento_excepcion_sod_scope
    FOREIGN KEY (id_tenant, id_empresa)
    REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_organizacion_grupo_empresarial_empresa_scope' AND parent_object_id = OBJECT_ID(N'organizacion.grupo_empresarial_empresa'))
BEGIN
    ALTER TABLE organizacion.grupo_empresarial_empresa WITH CHECK
    ADD CONSTRAINT FK_organizacion_grupo_empresarial_empresa_scope
    FOREIGN KEY (id_tenant, id_empresa)
    REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_logistica_recargo_regla_scope' AND parent_object_id = OBJECT_ID(N'logistica.recargo_regla'))
BEGIN
    ALTER TABLE logistica.recargo_regla WITH CHECK
    ADD CONSTRAINT FK_logistica_recargo_regla_scope
    FOREIGN KEY (id_tenant, id_empresa)
    REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;
GO

DECLARE @fkToDrop TABLE (fk_name SYSNAME, parent_name SYSNAME, composite_fk SYSNAME);
INSERT INTO @fkToDrop (fk_name, parent_name, composite_fk)
VALUES
(N'FK_seguridad_asignacion_rol_usuario_organizacion_empresa_id_empresa', N'seguridad.asignacion_rol_usuario', N'FK_seguridad_asignacion_rol_usuario_empresa_scope'),
(N'FK_seguridad_excepcion_permiso_usuario_organizacion_empresa_id_empresa', N'seguridad.excepcion_permiso_usuario', N'FK_seguridad_excepcion_permiso_usuario_empresa_scope'),
(N'FK_seguridad_configuracion_canal_notificacion_organizacion_empresa_id_empresa', N'seguridad.configuracion_canal_notificacion', N'FK_seguridad_configuracion_canal_notificacion_empresa_scope'),
(N'FK_seguridad_contador_rate_limit_organizacion_empresa_id_empresa', N'seguridad.contador_rate_limit', N'FK_seguridad_contador_rate_limit_empresa_scope'),
(N'FK_seguridad_desafio_mfa_organizacion_empresa_id_empresa', N'seguridad.desafio_mfa', N'FK_seguridad_desafio_mfa_empresa_scope'),
(N'FK_seguridad_filtro_dato_usuario_organizacion_empresa_id_empresa', N'seguridad.filtro_dato_usuario', N'FK_seguridad_filtro_dato_usuario_empresa_scope'),
(N'FK_seguridad_politica_ip_organizacion_empresa_id_empresa', N'seguridad.politica_ip', N'FK_seguridad_politica_ip_empresa_scope'),
(N'FK_observabilidad_auditoria_autorizacion_organizacion_empresa_id_empresa', N'observabilidad.auditoria_autorizacion', N'FK_observabilidad_auditoria_autorizacion_empresa_scope'),
(N'FK_observabilidad_auditoria_evento_seguridad_organizacion_empresa_id_empresa', N'observabilidad.auditoria_evento_seguridad', N'FK_observabilidad_auditoria_evento_seguridad_empresa_scope'),
(N'FK_observabilidad_auditoria_reinicio_mesa_ayuda_organizacion_empresa_id_empresa', N'observabilidad.auditoria_reinicio_mesa_ayuda', N'FK_observabilidad_auditoria_reinicio_mesa_ayuda_empresa_scope'),
(N'FK_observabilidad_error_aplicacion_organizacion_empresa_id_empresa', N'observabilidad.error_aplicacion', N'FK_observabilidad_error_aplicacion_empresa_scope'),
(N'FK_cumplimiento_perfil_aprobacion_organizacion_empresa_id_empresa', N'cumplimiento.perfil_aprobacion', N'FK_cumplimiento_perfil_aprobacion_empresa_scope'),
(N'FK_cumplimiento_regla_sod_organizacion_empresa_id_empresa', N'cumplimiento.regla_sod', N'FK_cumplimiento_regla_sod_empresa_scope'),
(N'FK_logistica_regla_control_empresa', N'logistica.regla_control_carga', N'FK_logistica_regla_control_carga_empresa_scope');

DECLARE @fk SYSNAME, @parent SYSNAME, @composite SYSNAME, @sql NVARCHAR(MAX);
DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
SELECT fk_name, parent_name, composite_fk
FROM @fkToDrop;

OPEN cur;
FETCH NEXT FROM cur INTO @fk, @parent, @composite;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = @fk AND parent_object_id = OBJECT_ID(@parent))
       AND EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = @composite AND parent_object_id = OBJECT_ID(@parent))
    BEGIN
        SET @sql = N'ALTER TABLE ' + @parent + N' DROP CONSTRAINT ' + QUOTENAME(@fk) + N';';
        EXEC sys.sp_executesql @sql;
    END;

    FETCH NEXT FROM cur INTO @fk, @parent, @composite;
END;
CLOSE cur;
DEALLOCATE cur;
GO
/* ============================================================
   3) Indices/constraints para performance + integridad de scope
   ============================================================ */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_seguridad_usuario_scope_empresa_usuario_tenant_empresa' AND object_id = OBJECT_ID(N'seguridad.usuario_scope_empresa'))
    CREATE UNIQUE NONCLUSTERED INDEX UX_seguridad_usuario_scope_empresa_usuario_tenant_empresa
        ON seguridad.usuario_scope_empresa (id_usuario, id_tenant, id_empresa);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_seguridad_usuario_scope_unidad_usuario_tenant_empresa_unidad' AND object_id = OBJECT_ID(N'seguridad.usuario_scope_unidad'))
    CREATE UNIQUE NONCLUSTERED INDEX UX_seguridad_usuario_scope_unidad_usuario_tenant_empresa_unidad
        ON seguridad.usuario_scope_unidad (id_usuario, id_tenant, id_empresa, id_unidad_organizativa);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_sesion_usuario_tenant_empresa_usuario_activo' AND object_id = OBJECT_ID(N'seguridad.sesion_usuario'))
    CREATE NONCLUSTERED INDEX IX_seguridad_sesion_usuario_tenant_empresa_usuario_activo
        ON seguridad.sesion_usuario (id_tenant, id_empresa, id_usuario, activo)
        INCLUDE (ultima_actividad_utc, expira_absoluta_utc, mfa_validado);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_usuario_empresa_tenant_empresa_usuario_operacion' AND object_id = OBJECT_ID(N'seguridad.usuario_empresa'))
    CREATE NONCLUSTERED INDEX IX_seguridad_usuario_empresa_tenant_empresa_usuario_operacion
        ON seguridad.usuario_empresa (id_tenant, id_empresa, id_usuario)
        INCLUDE (activo, puede_operar, es_empresa_predeterminada, fecha_inicio_utc, fecha_fin_utc);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_organizacion_unidad_organizativa_tenant_empresa_activo' AND object_id = OBJECT_ID(N'organizacion.unidad_organizativa'))
    CREATE NONCLUSTERED INDEX IX_organizacion_unidad_organizativa_tenant_empresa_activo
        ON organizacion.unidad_organizativa (id_tenant, id_empresa, activo)
        INCLUDE (id_unidad_organizativa, id_unidad_padre, codigo, nombre);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tercero_tercero_empresa_tenant_empresa_tercero' AND object_id = OBJECT_ID(N'tercero.tercero_empresa'))
    CREATE NONCLUSTERED INDEX IX_tercero_tercero_empresa_tenant_empresa_tercero
        ON tercero.tercero_empresa (id_tenant, id_empresa, id_tercero)
        INCLUDE (activo);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tercero_tercero_rol_tenant_empresa_tercero' AND object_id = OBJECT_ID(N'tercero.tercero_rol'))
    CREATE NONCLUSTERED INDEX IX_tercero_tercero_rol_tenant_empresa_tercero
        ON tercero.tercero_rol (id_tenant, id_empresa, id_tercero)
        INCLUDE (id_rol_tercero, activo);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_filtro_dato_usuario_tenant_empresa_usuario_entidad' AND object_id = OBJECT_ID(N'seguridad.filtro_dato_usuario'))
    CREATE NONCLUSTERED INDEX IX_seguridad_filtro_dato_usuario_tenant_empresa_usuario_entidad
        ON seguridad.filtro_dato_usuario (id_tenant, id_empresa, id_usuario, codigo_entidad, activo)
        INCLUDE (id_modo_filtro_dato, id_unidad_organizativa, fecha_inicio_utc, fecha_fin_utc);
GO

/* ============================================================
   4) Separacion: seguridad de datos vs preferencias UI
   ============================================================ */
IF OBJECT_ID(N'seguridad.preferencia_ui_usuario', N'U') IS NULL
BEGIN
    CREATE TABLE seguridad.preferencia_ui_usuario
    (
        id_preferencia_ui_usuario BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_seguridad_preferencia_ui_usuario PRIMARY KEY,
        id_usuario BIGINT NOT NULL,
        id_tenant BIGINT NOT NULL,
        id_empresa BIGINT NULL,
        codigo_formulario NVARCHAR(256) NOT NULL,
        layout_payload NVARCHAR(MAX) NOT NULL,
        fecha_inicio_utc DATETIME2 NOT NULL CONSTRAINT DF_seguridad_preferencia_ui_usuario_fecha_inicio DEFAULT SYSUTCDATETIME(),
        fecha_fin_utc DATETIME2 NULL,
        activo BIT NOT NULL CONSTRAINT DF_seguridad_preferencia_ui_usuario_activo DEFAULT (1),
        creado_utc DATETIME2 NOT NULL CONSTRAINT DF_seguridad_preferencia_ui_usuario_creado DEFAULT SYSUTCDATETIME(),
        actualizado_utc DATETIME2 NULL
    );
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_preferencia_ui_usuario_usuario' AND parent_object_id = OBJECT_ID(N'seguridad.preferencia_ui_usuario'))
BEGIN
    ALTER TABLE seguridad.preferencia_ui_usuario WITH CHECK
    ADD CONSTRAINT FK_seguridad_preferencia_ui_usuario_usuario
    FOREIGN KEY (id_usuario) REFERENCES seguridad.usuario(id_usuario);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_seguridad_preferencia_ui_usuario_scope' AND object_id = OBJECT_ID(N'seguridad.preferencia_ui_usuario'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_seguridad_preferencia_ui_usuario_scope
        ON seguridad.preferencia_ui_usuario (id_usuario, id_tenant, id_empresa, codigo_formulario)
        WHERE activo = 1;
END;

INSERT INTO seguridad.preferencia_ui_usuario
(
    id_usuario,
    id_tenant,
    id_empresa,
    codigo_formulario,
    layout_payload,
    fecha_inicio_utc,
    fecha_fin_utc,
    activo,
    creado_utc,
    actualizado_utc
)
SELECT
    f.id_usuario,
    f.id_tenant,
    f.id_empresa,
    f.codigo_entidad,
    f.valor_filtro,
    f.fecha_inicio_utc,
    f.fecha_fin_utc,
    f.activo,
    f.creado_utc,
    f.actualizado_utc
FROM seguridad.filtro_dato_usuario f
INNER JOIN catalogo.modo_filtro_dato m
    ON m.id_modo_filtro_dato = f.id_modo_filtro_dato
   AND UPPER(m.codigo) = 'UI_LAYOUT'
WHERE NOT EXISTS
(
    SELECT 1
    FROM seguridad.preferencia_ui_usuario p
    WHERE p.id_usuario = f.id_usuario
      AND p.id_tenant = f.id_tenant
      AND ((p.id_empresa IS NULL AND f.id_empresa IS NULL) OR p.id_empresa = f.id_empresa)
      AND p.codigo_formulario = f.codigo_entidad
);
GO
CREATE OR ALTER PROCEDURE seguridad.usp_preferencia_ui_usuario_guardar
    @id_usuario BIGINT,
    @id_tenant BIGINT,
    @id_empresa BIGINT = NULL,
    @codigo_formulario NVARCHAR(256),
    @layout_payload NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    IF @id_usuario IS NULL OR @id_tenant IS NULL OR @codigo_formulario IS NULL OR LTRIM(RTRIM(@codigo_formulario)) = N''
        THROW 51120, N'Parametros invalidos para guardar preferencia UI.', 1;

    IF @layout_payload IS NULL OR LTRIM(RTRIM(@layout_payload)) = N''
        THROW 51121, N'layout_payload es obligatorio.', 1;

    MERGE seguridad.preferencia_ui_usuario AS target
    USING (SELECT @id_usuario AS id_usuario, @id_tenant AS id_tenant, @id_empresa AS id_empresa, @codigo_formulario AS codigo_formulario) AS source
        ON target.id_usuario = source.id_usuario
       AND target.id_tenant = source.id_tenant
       AND ((target.id_empresa IS NULL AND source.id_empresa IS NULL) OR target.id_empresa = source.id_empresa)
       AND target.codigo_formulario = source.codigo_formulario
       AND target.activo = 1
    WHEN MATCHED THEN
        UPDATE SET
            layout_payload = @layout_payload,
            fecha_fin_utc = NULL,
            actualizado_utc = SYSUTCDATETIME()
    WHEN NOT MATCHED THEN
        INSERT (id_usuario, id_tenant, id_empresa, codigo_formulario, layout_payload, fecha_inicio_utc, fecha_fin_utc, activo, creado_utc, actualizado_utc)
        VALUES (@id_usuario, @id_tenant, @id_empresa, @codigo_formulario, @layout_payload, SYSUTCDATETIME(), NULL, 1, SYSUTCDATETIME(), SYSUTCDATETIME());

    SELECT 1 AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_preferencia_ui_usuario_obtener
    @id_usuario BIGINT,
    @id_tenant BIGINT,
    @id_empresa BIGINT = NULL,
    @codigo_formulario NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        codigo_formulario,
        layout_payload,
        actualizado_utc
    FROM seguridad.preferencia_ui_usuario
    WHERE id_usuario = @id_usuario
      AND id_tenant = @id_tenant
      AND codigo_formulario = @codigo_formulario
      AND activo = 1
      AND ((@id_empresa IS NULL AND id_empresa IS NULL) OR id_empresa = @id_empresa OR (@id_empresa IS NOT NULL AND id_empresa IS NULL))
    ORDER BY CASE WHEN id_empresa = @id_empresa THEN 0 ELSE 1 END, id_preferencia_ui_usuario DESC;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_guardar_layout_ui
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint = NULL,
    @codigo_entidad nvarchar(256),
    @layout_payload nvarchar(300)
AS
BEGIN
    SET NOCOUNT ON;
    EXEC seguridad.usp_preferencia_ui_usuario_guardar
        @id_usuario = @id_usuario,
        @id_tenant = @id_tenant,
        @id_empresa = @id_empresa,
        @codigo_formulario = @codigo_entidad,
        @layout_payload = @layout_payload;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_obtener_layout_ui
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint = NULL,
    @codigo_entidad nvarchar(256)
AS
BEGIN
    SET NOCOUNT ON;
    EXEC seguridad.usp_preferencia_ui_usuario_obtener
        @id_usuario = @id_usuario,
        @id_tenant = @id_tenant,
        @id_empresa = @id_empresa,
        @codigo_formulario = @codigo_entidad;
END
GO

CREATE OR ALTER FUNCTION seguridad.fn_usuario_empresas_efectivas
(
    @id_usuario BIGINT,
    @id_tenant BIGINT
)
RETURNS TABLE
AS
RETURN
WITH admin_tenant AS
(
    SELECT 1 AS es_admin
    FROM seguridad.usuario_tenant ut
    WHERE ut.id_usuario = @id_usuario
      AND ut.id_tenant = @id_tenant
      AND ut.activo = 1
      AND ut.es_administrador_tenant = 1
),
fuentes_positivas AS
(
    SELECT ue.id_empresa
    FROM seguridad.usuario_empresa ue
    WHERE ue.id_usuario = @id_usuario
      AND ue.id_tenant = @id_tenant
      AND ue.activo = 1
      AND ue.puede_operar = 1
      AND ue.fecha_inicio_utc <= SYSUTCDATETIME()
      AND (ue.fecha_fin_utc IS NULL OR ue.fecha_fin_utc >= SYSUTCDATETIME())

    UNION

    SELECT aru.id_empresa
    FROM seguridad.asignacion_rol_usuario aru
    WHERE aru.id_usuario = @id_usuario
      AND aru.id_tenant = @id_tenant
      AND aru.activo = 1
      AND aru.id_empresa IS NOT NULL
      AND aru.fecha_inicio_utc <= SYSUTCDATETIME()
      AND (aru.fecha_fin_utc IS NULL OR aru.fecha_fin_utc >= SYSUTCDATETIME())

    UNION

    SELECT epu.id_empresa
    FROM seguridad.excepcion_permiso_usuario epu
    INNER JOIN catalogo.efecto_permiso ep ON ep.id_efecto_permiso = epu.id_efecto_permiso
    WHERE epu.id_usuario = @id_usuario
      AND epu.id_tenant = @id_tenant
      AND epu.activo = 1
      AND epu.id_empresa IS NOT NULL
      AND UPPER(ep.codigo) = 'ALLOW'
      AND epu.fecha_inicio_utc <= SYSUTCDATETIME()
      AND (epu.fecha_fin_utc IS NULL OR epu.fecha_fin_utc >= SYSUTCDATETIME())
),
fuentes_negativas AS
(
    SELECT DISTINCT epu.id_empresa
    FROM seguridad.excepcion_permiso_usuario epu
    INNER JOIN catalogo.efecto_permiso ep ON ep.id_efecto_permiso = epu.id_efecto_permiso
    WHERE epu.id_usuario = @id_usuario
      AND epu.id_tenant = @id_tenant
      AND epu.activo = 1
      AND epu.id_empresa IS NOT NULL
      AND UPPER(ep.codigo) = 'DENY'
      AND epu.fecha_inicio_utc <= SYSUTCDATETIME()
      AND (epu.fecha_fin_utc IS NULL OR epu.fecha_fin_utc >= SYSUTCDATETIME())
),
scope_explicito AS
(
    SELECT usemp.id_empresa
    FROM seguridad.usuario_scope_empresa usemp
    WHERE usemp.id_usuario = @id_usuario
      AND usemp.id_tenant = @id_tenant
),
positivas_filtradas AS
(
    SELECT DISTINCT p.id_empresa
    FROM fuentes_positivas p
    WHERE
        NOT EXISTS (SELECT 1 FROM scope_explicito)
        OR EXISTS (SELECT 1 FROM scope_explicito se WHERE se.id_empresa = p.id_empresa)
)
SELECT DISTINCT e.id_empresa
FROM organizacion.empresa e
WHERE e.id_tenant = @id_tenant
  AND e.activo = 1
  AND
  (
      EXISTS (SELECT 1 FROM admin_tenant)
      OR EXISTS (SELECT 1 FROM positivas_filtradas pf WHERE pf.id_empresa = e.id_empresa)
  )
  AND NOT EXISTS (SELECT 1 FROM fuentes_negativas n WHERE n.id_empresa = e.id_empresa);
GO
CREATE OR ALTER FUNCTION seguridad.fn_usuario_unidades_efectivas
(
    @id_usuario BIGINT,
    @id_tenant BIGINT,
    @id_empresa BIGINT
)
RETURNS TABLE
AS
RETURN
WITH empresas_permitidas AS
(
    SELECT id_empresa
    FROM seguridad.fn_usuario_empresas_efectivas(@id_usuario, @id_tenant)
),
admin_tenant AS
(
    SELECT 1 AS es_admin
    FROM seguridad.usuario_tenant ut
    WHERE ut.id_usuario = @id_usuario
      AND ut.id_tenant = @id_tenant
      AND ut.activo = 1
      AND ut.es_administrador_tenant = 1
)
SELECT DISTINCT uo.id_unidad_organizativa
FROM organizacion.unidad_organizativa uo
WHERE uo.id_tenant = @id_tenant
  AND uo.id_empresa = @id_empresa
  AND uo.activo = 1
  AND EXISTS (SELECT 1 FROM empresas_permitidas ep WHERE ep.id_empresa = @id_empresa)
  AND
  (
      EXISTS (SELECT 1 FROM admin_tenant)
      OR NOT EXISTS (
            SELECT 1
            FROM seguridad.usuario_scope_unidad usu
            WHERE usu.id_usuario = @id_usuario
              AND usu.id_tenant = @id_tenant
              AND usu.id_empresa = @id_empresa
      )
      OR EXISTS (
            SELECT 1
            FROM seguridad.usuario_scope_unidad usu
            WHERE usu.id_usuario = @id_usuario
              AND usu.id_tenant = @id_tenant
              AND usu.id_empresa = @id_empresa
              AND usu.id_unidad_organizativa = uo.id_unidad_organizativa
      )
  );
GO

CREATE OR ALTER VIEW seguridad.vw_usuario_scope_efectivo
AS
SELECT
    ut.id_usuario,
    ut.id_tenant,
    emp.id_empresa,
    CAST(NULL AS BIGINT) AS id_unidad_organizativa,
    CAST(CASE WHEN ut.es_administrador_tenant = 1 THEN 1 ELSE 0 END AS BIT) AS es_admin_tenant,
    CAST(1 AS BIT) AS permitido
FROM seguridad.usuario_tenant ut
CROSS APPLY seguridad.fn_usuario_empresas_efectivas(ut.id_usuario, ut.id_tenant) emp
WHERE ut.activo = 1
UNION ALL
SELECT
    ut.id_usuario,
    ut.id_tenant,
    emp.id_empresa,
    uni.id_unidad_organizativa,
    CAST(CASE WHEN ut.es_administrador_tenant = 1 THEN 1 ELSE 0 END AS BIT) AS es_admin_tenant,
    CAST(1 AS BIT) AS permitido
FROM seguridad.usuario_tenant ut
CROSS APPLY seguridad.fn_usuario_empresas_efectivas(ut.id_usuario, ut.id_tenant) emp
CROSS APPLY seguridad.fn_usuario_unidades_efectivas(ut.id_usuario, ut.id_tenant, emp.id_empresa) uni
WHERE ut.activo = 1;
GO

CREATE OR ALTER PROCEDURE seguridad.usp_scope_validar_sesion
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ctx_id_usuario BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_usuario') AS BIGINT);
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);

    IF @ctx_id_usuario IS NULL OR @ctx_id_tenant IS NULL OR @ctx_id_empresa IS NULL
        THROW 51130, N'SESSION_CONTEXT incompleto (id_usuario/id_tenant/id_empresa).', 1;

    IF NOT EXISTS (
        SELECT 1
        FROM seguridad.fn_usuario_empresas_efectivas(@ctx_id_usuario, @ctx_id_tenant)
        WHERE id_empresa = @ctx_id_empresa
    )
        THROW 51131, N'La empresa de sesion no esta autorizada para el usuario.', 1;

    SELECT 1 AS scope_valido;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_auth_obtener_usuario_para_autenticacion
    @tenant_codigo NVARCHAR(50),
    @identificador NVARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @identificador_normalizado NVARCHAR(250) = UPPER(LTRIM(RTRIM(@identificador)));

    SELECT TOP (1)
        u.id_usuario,
        t.id_tenant,
        ue.id_empresa AS id_empresa,
        t.codigo AS tenant_codigo,
        u.login_principal,
        u.nombre_mostrar,
        u.correo_electronico,
        u.mfa_habilitado,
        u.requiere_cambio_clave,
        u.id_estado_usuario,
        u.activo AS activo_usuario,
        c.hash_clave,
        c.salt_clave,
        c.algoritmo_clave,
        c.iteraciones_clave,
        c.activo AS activo_credencial
    FROM plataforma.tenant t
    INNER JOIN seguridad.usuario_tenant ut
        ON ut.id_tenant = t.id_tenant
       AND ut.activo = 1
    INNER JOIN seguridad.usuario u
        ON u.id_usuario = ut.id_usuario
    INNER JOIN seguridad.credencial_local_usuario c
        ON c.id_usuario = u.id_usuario
    OUTER APPLY (
        SELECT TOP (1)
            ue1.id_empresa
        FROM seguridad.usuario_empresa ue1
        WHERE ue1.id_usuario = u.id_usuario
          AND ue1.id_tenant = t.id_tenant
          AND ue1.activo = 1
          AND ue1.puede_operar = 1
        ORDER BY
            CASE WHEN ue1.es_empresa_predeterminada = 1 THEN 0 ELSE 1 END,
            ue1.id_usuario_empresa
    ) ue
    WHERE t.codigo = @tenant_codigo
      AND t.activo = 1
      AND ue.id_empresa IS NOT NULL
      AND (
            u.login_normalizado = @identificador_normalizado
         OR u.correo_normalizado = @identificador_normalizado
      );
END
GO

IF EXISTS (SELECT 1 FROM sys.security_policies WHERE name = N'RLS_scope_tenant_empresa')
    DROP SECURITY POLICY seguridad.RLS_scope_tenant_empresa;
GO

CREATE OR ALTER FUNCTION seguridad.fn_rls_tenant_empresa
(
    @id_tenant BIGINT,
    @id_empresa BIGINT
)
RETURNS TABLE
WITH SCHEMABINDING
AS
RETURN
SELECT 1 AS fn_result
WHERE
    @id_tenant = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT)
    AND
    (
        TRY_CAST(SESSION_CONTEXT(N'es_admin_tenant') AS BIT) = 1
        OR
        @id_empresa = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT)
    );
GO

IF OBJECT_ID(N'seguridad.SP_rls_scope_prepared', N'P') IS NULL
    EXEC(N'CREATE PROCEDURE seguridad.SP_rls_scope_prepared AS BEGIN SET NOCOUNT ON; SELECT 1; END;');
GO

CREATE OR ALTER PROCEDURE seguridad.SP_rls_scope_prepared
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM sys.security_policies WHERE name = N'RLS_scope_tenant_empresa')
    BEGIN
        EXEC(N'
            CREATE SECURITY POLICY seguridad.RLS_scope_tenant_empresa
            ADD FILTER PREDICATE seguridad.fn_rls_tenant_empresa(id_tenant, id_empresa)
                ON seguridad.excepcion_permiso_usuario,
            ADD FILTER PREDICATE seguridad.fn_rls_tenant_empresa(id_tenant, id_empresa)
                ON seguridad.asignacion_rol_usuario,
            ADD FILTER PREDICATE seguridad.fn_rls_tenant_empresa(id_tenant, id_empresa)
                ON cumplimiento.instancia_aprobacion
            WITH (STATE = OFF);
        ');
    END;

    SELECT name AS policy_name, is_enabled
    FROM sys.security_policies
    WHERE name = N'RLS_scope_tenant_empresa';
END
GO

EXEC seguridad.SP_rls_scope_prepared;
GO
