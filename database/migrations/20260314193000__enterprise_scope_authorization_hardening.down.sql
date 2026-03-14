/*
    Down: 20260314193000__enterprise_scope_authorization_hardening
    Nota de rollback:
    - Se revierten objetos logicos y constraints agregados en esta migracion.
    - No se eliminan columnas materializadas id_tenant/id_empresa para evitar perdida de datos.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF EXISTS (SELECT 1 FROM sys.security_policies WHERE name = N'RLS_scope_tenant_empresa')
    DROP SECURITY POLICY seguridad.RLS_scope_tenant_empresa;
GO

IF OBJECT_ID(N'seguridad.SP_rls_scope_prepared', N'P') IS NOT NULL
    DROP PROCEDURE seguridad.SP_rls_scope_prepared;
GO

IF OBJECT_ID(N'seguridad.fn_rls_tenant_empresa', N'IF') IS NOT NULL
    DROP FUNCTION seguridad.fn_rls_tenant_empresa;
GO

IF OBJECT_ID(N'seguridad.vw_usuario_scope_efectivo', N'V') IS NOT NULL
    DROP VIEW seguridad.vw_usuario_scope_efectivo;
GO

IF OBJECT_ID(N'seguridad.fn_usuario_unidades_efectivas', N'IF') IS NOT NULL
    DROP FUNCTION seguridad.fn_usuario_unidades_efectivas;
GO

IF OBJECT_ID(N'seguridad.fn_usuario_empresas_efectivas', N'IF') IS NOT NULL
    DROP FUNCTION seguridad.fn_usuario_empresas_efectivas;
GO

IF OBJECT_ID(N'seguridad.usp_scope_validar_sesion', N'P') IS NOT NULL
    DROP PROCEDURE seguridad.usp_scope_validar_sesion;
GO

IF OBJECT_ID(N'seguridad.usp_preferencia_ui_usuario_guardar', N'P') IS NOT NULL
    DROP PROCEDURE seguridad.usp_preferencia_ui_usuario_guardar;
GO

IF OBJECT_ID(N'seguridad.usp_preferencia_ui_usuario_obtener', N'P') IS NOT NULL
    DROP PROCEDURE seguridad.usp_preferencia_ui_usuario_obtener;
GO

/* Vuelve a fuente anterior de layout UI (filtro_dato_usuario) */
IF OBJECT_ID(N'seguridad.usp_filtro_dato_usuario_guardar_layout_ui', N'P') IS NOT NULL
BEGIN
    EXEC(N'
    CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_guardar_layout_ui
        @id_usuario bigint,
        @id_tenant bigint,
        @id_empresa bigint = NULL,
        @codigo_entidad nvarchar(256),
        @layout_payload nvarchar(300)
    AS
    BEGIN
        SET NOCOUNT ON;
        THROW 51190, ''Reaplicar procedimiento legacy de layout UI desde backup previo.'', 1;
    END
    ');
END;
GO

IF OBJECT_ID(N'seguridad.usp_filtro_dato_usuario_obtener_layout_ui', N'P') IS NOT NULL
BEGIN
    EXEC(N'
    CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_obtener_layout_ui
        @id_usuario bigint,
        @id_tenant bigint,
        @id_empresa bigint = NULL,
        @codigo_entidad nvarchar(256)
    AS
    BEGIN
        SET NOCOUNT ON;
        THROW 51191, ''Reaplicar procedimiento legacy de layout UI desde backup previo.'', 1;
    END
    ');
END;
GO

IF OBJECT_ID(N'seguridad.preferencia_ui_usuario', N'U') IS NOT NULL
    DROP TABLE seguridad.preferencia_ui_usuario;
GO

/* Remueve nuevos indices */
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_filtro_dato_usuario_tenant_empresa_usuario_entidad' AND object_id = OBJECT_ID(N'seguridad.filtro_dato_usuario'))
    DROP INDEX IX_seguridad_filtro_dato_usuario_tenant_empresa_usuario_entidad ON seguridad.filtro_dato_usuario;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tercero_tercero_rol_tenant_empresa_tercero' AND object_id = OBJECT_ID(N'tercero.tercero_rol'))
    DROP INDEX IX_tercero_tercero_rol_tenant_empresa_tercero ON tercero.tercero_rol;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tercero_tercero_empresa_tenant_empresa_tercero' AND object_id = OBJECT_ID(N'tercero.tercero_empresa'))
    DROP INDEX IX_tercero_tercero_empresa_tenant_empresa_tercero ON tercero.tercero_empresa;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_organizacion_unidad_organizativa_tenant_empresa_activo' AND object_id = OBJECT_ID(N'organizacion.unidad_organizativa'))
    DROP INDEX IX_organizacion_unidad_organizativa_tenant_empresa_activo ON organizacion.unidad_organizativa;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_usuario_empresa_tenant_empresa_usuario_operacion' AND object_id = OBJECT_ID(N'seguridad.usuario_empresa'))
    DROP INDEX IX_seguridad_usuario_empresa_tenant_empresa_usuario_operacion ON seguridad.usuario_empresa;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_sesion_usuario_tenant_empresa_usuario_activo' AND object_id = OBJECT_ID(N'seguridad.sesion_usuario'))
    DROP INDEX IX_seguridad_sesion_usuario_tenant_empresa_usuario_activo ON seguridad.sesion_usuario;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_seguridad_usuario_scope_unidad_usuario_tenant_empresa_unidad' AND object_id = OBJECT_ID(N'seguridad.usuario_scope_unidad'))
    DROP INDEX UX_seguridad_usuario_scope_unidad_usuario_tenant_empresa_unidad ON seguridad.usuario_scope_unidad;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_seguridad_usuario_scope_empresa_usuario_tenant_empresa' AND object_id = OBJECT_ID(N'seguridad.usuario_scope_empresa'))
    DROP INDEX UX_seguridad_usuario_scope_empresa_usuario_tenant_empresa ON seguridad.usuario_scope_empresa;
GO

/* Remueve nuevos FKs compuestos */
DECLARE @dropFk TABLE (parent_table SYSNAME, fk_name SYSNAME);
INSERT INTO @dropFk (parent_table, fk_name)
VALUES
(N'seguridad.usuario_scope_empresa', N'FK_seguridad_usuario_scope_empresa_empresa_scope'),
(N'seguridad.usuario_scope_unidad', N'FK_seguridad_usuario_scope_unidad_empresa_scope'),
(N'seguridad.usuario_scope_unidad', N'FK_seguridad_usuario_scope_unidad_unidad_scope'),
(N'tercero.tercero_empresa', N'FK_tercero_tercero_empresa_empresa_scope'),
(N'tercero.tercero_rol', N'FK_tercero_tercero_rol_empresa_scope'),
(N'plataforma.configuracion_empresa', N'FK_plataforma_configuracion_empresa_scope'),
(N'seguridad.politica_empresa_override', N'FK_seguridad_politica_empresa_override_scope'),
(N'cumplimiento.accion_instancia_aprobacion', N'FK_cumplimiento_accion_instancia_aprobacion_scope'),
(N'cumplimiento.excepcion_sod', N'FK_cumplimiento_excepcion_sod_scope'),
(N'organizacion.grupo_empresarial_empresa', N'FK_organizacion_grupo_empresarial_empresa_scope'),
(N'logistica.recargo_regla', N'FK_logistica_recargo_regla_scope');

DECLARE @pt SYSNAME, @fk SYSNAME, @q NVARCHAR(MAX);
DECLARE c CURSOR LOCAL FAST_FORWARD FOR SELECT parent_table, fk_name FROM @dropFk;
OPEN c;
FETCH NEXT FROM c INTO @pt, @fk;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = @fk AND parent_object_id = OBJECT_ID(@pt))
    BEGIN
        SET @q = N'ALTER TABLE ' + @pt + N' DROP CONSTRAINT ' + QUOTENAME(@fk) + N';';
        EXEC sys.sp_executesql @q;
    END;
    FETCH NEXT FROM c INTO @pt, @fk;
END;
CLOSE c;
DEALLOCATE c;
GO

IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_tercero_tercero_rol_tenant_empresa_par' AND parent_object_id = OBJECT_ID(N'tercero.tercero_rol'))
    ALTER TABLE tercero.tercero_rol DROP CONSTRAINT CK_tercero_tercero_rol_tenant_empresa_par;
GO
