
/*
    Migracion: 20260314221000__enterprise_scope_phase2_hardening
    Objetivo: cierre enterprise de scope, auth, integridad, tipos y estandares DB.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ============================================================
   auth_fix + fk_normalization
   ============================================================ */
DECLARE @fkDrop TABLE (parent_table SYSNAME, fk_name SYSNAME);
INSERT INTO @fkDrop(parent_table,fk_name)
VALUES
(N'cumplimiento.accion_instancia_aprobacion',N'FK_cumplimiento_accion_instancia_aprobacion_organizacion_empresa_id_empresa'),
(N'cumplimiento.excepcion_sod',N'FK_cumplimiento_excepcion_sod_organizacion_empresa_id_empresa'),
(N'logistica.recargo_regla',N'FK_logistica_recargo_regla_empresa'),
(N'organizacion.grupo_empresarial_empresa',N'FK_organizacion_grupo_empresarial_empresa_organizacion_empresa_id_empresa'),
(N'plataforma.configuracion_empresa',N'FK_plataforma_configuracion_empresa_empresa'),
(N'seguridad.politica_empresa_override',N'FK_seguridad_politica_empresa_override_organizacion_empresa_id_empresa'),
(N'tercero.tercero_empresa',N'FK_tercero_tercero_empresa_empresa'),
(N'tercero.tercero_rol',N'FK_tercero_tercero_rol_empresa');

DECLARE @pt SYSNAME, @fk SYSNAME, @sql NVARCHAR(MAX);
DECLARE c_fk CURSOR LOCAL FAST_FORWARD FOR SELECT parent_table,fk_name FROM @fkDrop;
OPEN c_fk;
FETCH NEXT FROM c_fk INTO @pt,@fk;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=@fk AND parent_object_id=OBJECT_ID(@pt))
    BEGIN
        SET @sql = N'ALTER TABLE ' + @pt + N' DROP CONSTRAINT ' + QUOTENAME(@fk) + N';';
        EXEC sys.sp_executesql @sql;
    END;
    FETCH NEXT FROM c_fk INTO @pt,@fk;
END
CLOSE c_fk;
DEALLOCATE c_fk;
GO

/* ============================================================
   ui_preferences_split (canon: seguridad.preferencia_usuario_ui)
   ============================================================ */
IF OBJECT_ID(N'seguridad.preferencia_usuario_ui', N'U') IS NULL
BEGIN
    CREATE TABLE seguridad.preferencia_usuario_ui
    (
        id_preferencia_usuario_ui BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_seguridad_preferencia_usuario_ui PRIMARY KEY,
        id_usuario BIGINT NOT NULL,
        id_tenant BIGINT NOT NULL,
        id_empresa BIGINT NULL,
        codigo_entidad NVARCHAR(256) NOT NULL,
        layout_payload NVARCHAR(MAX) NOT NULL,
        activo BIT NOT NULL CONSTRAINT DF_seguridad_preferencia_usuario_ui_activo DEFAULT(1),
        creado_utc DATETIME2 NOT NULL CONSTRAINT DF_seguridad_preferencia_usuario_ui_creado DEFAULT SYSUTCDATETIME(),
        actualizado_utc DATETIME2 NULL
    );
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_preferencia_usuario_ui_usuario' AND parent_object_id = OBJECT_ID(N'seguridad.preferencia_usuario_ui'))
BEGIN
    ALTER TABLE seguridad.preferencia_usuario_ui WITH CHECK
        ADD CONSTRAINT FK_seguridad_preferencia_usuario_ui_usuario
            FOREIGN KEY (id_usuario) REFERENCES seguridad.usuario(id_usuario);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'seguridad.preferencia_usuario_ui') AND name = N'UX_seguridad_preferencia_usuario_ui_usuario_tenant_entidad_null_empresa')
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_seguridad_preferencia_usuario_ui_usuario_tenant_entidad_null_empresa
        ON seguridad.preferencia_usuario_ui(id_usuario,id_tenant,codigo_entidad)
        WHERE id_empresa IS NULL;
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'seguridad.preferencia_usuario_ui') AND name = N'UX_seguridad_preferencia_usuario_ui_usuario_tenant_empresa_entidad_notnull')
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_seguridad_preferencia_usuario_ui_usuario_tenant_empresa_entidad_notnull
        ON seguridad.preferencia_usuario_ui(id_usuario,id_tenant,id_empresa,codigo_entidad)
        WHERE id_empresa IS NOT NULL;
END;

/* Merge desde tabla accidental preferencia_ui_usuario si existe */
IF OBJECT_ID(N'seguridad.preferencia_ui_usuario', N'U') IS NOT NULL
BEGIN
    MERGE seguridad.preferencia_usuario_ui AS target
    USING
    (
        SELECT id_usuario, id_tenant, id_empresa, codigo_formulario AS codigo_entidad, layout_payload,
               activo, creado_utc, actualizado_utc
        FROM seguridad.preferencia_ui_usuario
    ) AS source
    ON target.id_usuario = source.id_usuario
       AND target.id_tenant = source.id_tenant
       AND ((target.id_empresa IS NULL AND source.id_empresa IS NULL) OR target.id_empresa = source.id_empresa)
       AND target.codigo_entidad = source.codigo_entidad
    WHEN MATCHED THEN
        UPDATE SET
            target.layout_payload = source.layout_payload,
            target.activo = source.activo,
            target.actualizado_utc = COALESCE(source.actualizado_utc, SYSUTCDATETIME())
    WHEN NOT MATCHED THEN
        INSERT(id_usuario,id_tenant,id_empresa,codigo_entidad,layout_payload,activo,creado_utc,actualizado_utc)
        VALUES(source.id_usuario,source.id_tenant,source.id_empresa,source.codigo_entidad,source.layout_payload,source.activo,source.creado_utc,source.actualizado_utc);

    DROP TABLE seguridad.preferencia_ui_usuario;
END;
GO

/* Mover UI_LAYOUT de filtro_dato_usuario => preferencia_usuario_ui */
INSERT INTO seguridad.preferencia_usuario_ui
(
    id_usuario,id_tenant,id_empresa,codigo_entidad,layout_payload,activo,creado_utc,actualizado_utc
)
SELECT f.id_usuario,f.id_tenant,f.id_empresa,f.codigo_entidad,COALESCE(f.valor_filtro,N''),f.activo,f.creado_utc,f.actualizado_utc
FROM seguridad.filtro_dato_usuario f
JOIN catalogo.modo_filtro_dato m ON m.id_modo_filtro_dato=f.id_modo_filtro_dato
WHERE UPPER(m.codigo)=N'UI_LAYOUT'
  AND NOT EXISTS
  (
      SELECT 1 FROM seguridad.preferencia_usuario_ui p
      WHERE p.id_usuario=f.id_usuario
        AND p.id_tenant=f.id_tenant
        AND ((p.id_empresa IS NULL AND f.id_empresa IS NULL) OR p.id_empresa=f.id_empresa)
        AND p.codigo_entidad=f.codigo_entidad
  );

DELETE f
FROM seguridad.filtro_dato_usuario f
JOIN catalogo.modo_filtro_dato m ON m.id_modo_filtro_dato=f.id_modo_filtro_dato
WHERE UPPER(m.codigo)=N'UI_LAYOUT';
GO

IF OBJECT_ID(N'seguridad.trg_filtro_dato_usuario_block_ui_layout', N'TR') IS NULL
    EXEC(N'CREATE TRIGGER seguridad.trg_filtro_dato_usuario_block_ui_layout ON seguridad.filtro_dato_usuario AFTER INSERT,UPDATE AS BEGIN SET NOCOUNT ON; END;');
GO

ALTER TRIGGER seguridad.trg_filtro_dato_usuario_block_ui_layout
ON seguridad.filtro_dato_usuario
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM inserted i
        INNER JOIN catalogo.modo_filtro_dato m ON m.id_modo_filtro_dato=i.id_modo_filtro_dato
        WHERE UPPER(m.codigo)=N'UI_LAYOUT'
    )
    BEGIN
        THROW 51220, N'UI_LAYOUT no debe persistirse en seguridad.filtro_dato_usuario. Use seguridad.preferencia_usuario_ui.', 1;
    END
END
GO
/* Procs canon de preferencias UI sobre tabla canonica */
CREATE OR ALTER PROCEDURE seguridad.usp_preferencia_usuario_ui_guardar
    @id_usuario BIGINT,
    @id_tenant BIGINT,
    @id_empresa BIGINT = NULL,
    @codigo_entidad NVARCHAR(256),
    @layout_payload NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    SET ANSI_NULLS ON;
    SET QUOTED_IDENTIFIER ON;

    IF @id_usuario IS NULL OR @id_tenant IS NULL OR @codigo_entidad IS NULL OR LTRIM(RTRIM(@codigo_entidad)) = N''
        THROW 51221, N'Parametros invalidos para guardar preferencia UI.', 1;

    IF @layout_payload IS NULL OR LTRIM(RTRIM(@layout_payload)) = N''
        THROW 51222, N'layout_payload es obligatorio.', 1;

    MERGE seguridad.preferencia_usuario_ui AS target
    USING (SELECT @id_usuario AS id_usuario, @id_tenant AS id_tenant, @id_empresa AS id_empresa, @codigo_entidad AS codigo_entidad) AS source
        ON target.id_usuario = source.id_usuario
       AND target.id_tenant = source.id_tenant
       AND ((target.id_empresa IS NULL AND source.id_empresa IS NULL) OR target.id_empresa = source.id_empresa)
       AND target.codigo_entidad = source.codigo_entidad
    WHEN MATCHED THEN
        UPDATE SET
            layout_payload = @layout_payload,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
    WHEN NOT MATCHED THEN
        INSERT (id_usuario,id_tenant,id_empresa,codigo_entidad,layout_payload,activo,creado_utc,actualizado_utc)
        VALUES (@id_usuario,@id_tenant,@id_empresa,@codigo_entidad,@layout_payload,1,SYSUTCDATETIME(),SYSUTCDATETIME());

    SELECT 1 AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_preferencia_usuario_ui_obtener
    @id_usuario BIGINT,
    @id_tenant BIGINT,
    @id_empresa BIGINT = NULL,
    @codigo_entidad NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;
    SET ANSI_NULLS ON;
    SET QUOTED_IDENTIFIER ON;

    SELECT TOP (1)
        codigo_entidad,
        layout_payload,
        actualizado_utc
    FROM seguridad.preferencia_usuario_ui
    WHERE id_usuario = @id_usuario
      AND id_tenant = @id_tenant
      AND codigo_entidad = @codigo_entidad
      AND activo = 1
      AND ((@id_empresa IS NULL AND id_empresa IS NULL) OR id_empresa = @id_empresa OR (@id_empresa IS NOT NULL AND id_empresa IS NULL))
    ORDER BY CASE WHEN id_empresa = @id_empresa THEN 0 ELSE 1 END,
             id_preferencia_usuario_ui DESC;
END
GO

/* Wrappers legacy para compatibilidad API/repositorio */
CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_guardar_layout_ui
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint = NULL,
    @codigo_entidad nvarchar(256),
    @layout_payload nvarchar(300)
AS
BEGIN
    SET NOCOUNT ON;
    EXEC seguridad.usp_preferencia_usuario_ui_guardar
        @id_usuario=@id_usuario,
        @id_tenant=@id_tenant,
        @id_empresa=@id_empresa,
        @codigo_entidad=@codigo_entidad,
        @layout_payload=@layout_payload;
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
    EXEC seguridad.usp_preferencia_usuario_ui_obtener
        @id_usuario=@id_usuario,
        @id_tenant=@id_tenant,
        @id_empresa=@id_empresa,
        @codigo_entidad=@codigo_entidad;
END
GO

/* ============================================================
   scope_hardening (semantica activa+vigencia)
   ============================================================ */
CREATE OR ALTER FUNCTION seguridad.fn_usuario_empresas_efectivas
(
    @id_usuario BIGINT,
    @id_tenant BIGINT
)
RETURNS TABLE
AS
RETURN
WITH utc_now AS
(
    SELECT SYSUTCDATETIME() AS now_utc
),
admin_tenant AS
(
    SELECT 1 AS es_admin
    FROM seguridad.usuario_tenant ut
    WHERE ut.id_usuario = @id_usuario
      AND ut.id_tenant = @id_tenant
      AND ut.activo = 1
),
fuentes_positivas AS
(
    SELECT ue.id_empresa
    FROM seguridad.usuario_empresa ue
    CROSS JOIN utc_now n
    WHERE ue.id_usuario = @id_usuario
      AND ue.id_tenant = @id_tenant
      AND ue.activo = 1
      AND ue.puede_operar = 1
      AND ue.fecha_inicio_utc <= n.now_utc
      AND (ue.fecha_fin_utc IS NULL OR ue.fecha_fin_utc >= n.now_utc)

    UNION

    SELECT aru.id_empresa
    FROM seguridad.asignacion_rol_usuario aru
    CROSS JOIN utc_now n
    WHERE aru.id_usuario = @id_usuario
      AND aru.id_tenant = @id_tenant
      AND aru.activo = 1
      AND aru.id_empresa IS NOT NULL
      AND aru.fecha_inicio_utc <= n.now_utc
      AND (aru.fecha_fin_utc IS NULL OR aru.fecha_fin_utc >= n.now_utc)

    UNION

    SELECT epu.id_empresa
    FROM seguridad.excepcion_permiso_usuario epu
    INNER JOIN catalogo.efecto_permiso ep ON ep.id_efecto_permiso = epu.id_efecto_permiso
    CROSS JOIN utc_now n
    WHERE epu.id_usuario = @id_usuario
      AND epu.id_tenant = @id_tenant
      AND epu.activo = 1
      AND epu.id_empresa IS NOT NULL
      AND UPPER(ep.codigo) = 'ALLOW'
      AND epu.fecha_inicio_utc <= n.now_utc
      AND (epu.fecha_fin_utc IS NULL OR epu.fecha_fin_utc >= n.now_utc)
),
fuentes_negativas AS
(
    SELECT DISTINCT epu.id_empresa
    FROM seguridad.excepcion_permiso_usuario epu
    INNER JOIN catalogo.efecto_permiso ep ON ep.id_efecto_permiso = epu.id_efecto_permiso
    CROSS JOIN utc_now n
    WHERE epu.id_usuario = @id_usuario
      AND epu.id_tenant = @id_tenant
      AND epu.activo = 1
      AND epu.id_empresa IS NOT NULL
      AND UPPER(ep.codigo) = 'DENY'
      AND epu.fecha_inicio_utc <= n.now_utc
      AND (epu.fecha_fin_utc IS NULL OR epu.fecha_fin_utc >= n.now_utc)
),
scope_explicito AS
(
    SELECT usemp.id_empresa
    FROM seguridad.usuario_scope_empresa usemp
    CROSS JOIN utc_now n
    WHERE usemp.id_usuario = @id_usuario
      AND usemp.id_tenant = @id_tenant
      AND usemp.activo = 1
      AND usemp.fecha_inicio_utc <= n.now_utc
      AND (usemp.fecha_fin_utc IS NULL OR usemp.fecha_fin_utc >= n.now_utc)
),
positivas_filtradas AS
(
    SELECT DISTINCT p.id_empresa
    FROM fuentes_positivas p
    WHERE NOT EXISTS (SELECT 1 FROM scope_explicito)
       OR EXISTS (SELECT 1 FROM scope_explicito se WHERE se.id_empresa = p.id_empresa)
)
SELECT DISTINCT e.id_empresa
FROM organizacion.empresa e
WHERE e.id_tenant = @id_tenant
  AND e.activo = 1
  AND (EXISTS (SELECT 1 FROM admin_tenant) OR EXISTS (SELECT 1 FROM positivas_filtradas pf WHERE pf.id_empresa = e.id_empresa))
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
WITH utc_now AS
(
    SELECT SYSUTCDATETIME() AS now_utc
),
empresas_permitidas AS
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
),
scope_unidad_activo AS
(
    SELECT usu.id_unidad_organizativa
    FROM seguridad.usuario_scope_unidad usu
    CROSS JOIN utc_now n
    WHERE usu.id_usuario = @id_usuario
      AND usu.id_tenant = @id_tenant
      AND usu.id_empresa = @id_empresa
      AND usu.activo = 1
      AND usu.fecha_inicio_utc <= n.now_utc
      AND (usu.fecha_fin_utc IS NULL OR usu.fecha_fin_utc >= n.now_utc)
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
      OR NOT EXISTS (SELECT 1 FROM scope_unidad_activo)
      OR EXISTS (SELECT 1 FROM scope_unidad_activo su WHERE su.id_unidad_organizativa = uo.id_unidad_organizativa)
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

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_empresa_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;

    DECLARE @now DATETIME2 = SYSUTCDATETIME();
    SELECT id_usuario_scope_empresa,id_usuario,id_tenant,id_empresa,fecha_inicio_utc,fecha_fin_utc,activo,creado_utc,actualizado_utc
    FROM seguridad.usuario_scope_empresa
    WHERE id_tenant=@ctx_id_tenant
      AND activo=1
      AND fecha_inicio_utc <= @now
      AND (fecha_fin_utc IS NULL OR fecha_fin_utc >= @now);
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_unidad_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;

    DECLARE @now DATETIME2 = SYSUTCDATETIME();
    SELECT id_usuario_scope_unidad,id_usuario,id_tenant,id_empresa,id_unidad_organizativa,fecha_inicio_utc,fecha_fin_utc,activo,creado_utc,actualizado_utc
    FROM seguridad.usuario_scope_unidad
    WHERE id_tenant=@ctx_id_tenant
      AND activo=1
      AND fecha_inicio_utc <= @now
      AND (fecha_fin_utc IS NULL OR fecha_fin_utc >= @now);
END
GO

/* ============================================================
   bigint_alignment
   ============================================================ */
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'actividad.comentario') AND name='creado_por' AND system_type_id=56)
    ALTER TABLE actividad.comentario ALTER COLUMN creado_por BIGINT NOT NULL;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'actividad.comentario') AND name='editado_por' AND system_type_id=56)
    ALTER TABLE actividad.comentario ALTER COLUMN editado_por BIGINT NULL;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'actividad.comentario_mencion') AND name='id_usuario' AND system_type_id=56)
    ALTER TABLE actividad.comentario_mencion ALTER COLUMN id_usuario BIGINT NOT NULL;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'documento.documento') AND name='creado_por' AND system_type_id=56)
    ALTER TABLE documento.documento ALTER COLUMN creado_por BIGINT NULL;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'documento.documento_entidad') AND name='id_usuario_asociacion' AND system_type_id=56)
    ALTER TABLE documento.documento_entidad ALTER COLUMN id_usuario_asociacion BIGINT NULL;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'documento.documento_version') AND name='creado_por' AND system_type_id=56)
    ALTER TABLE documento.documento_version ALTER COLUMN creado_por BIGINT NULL;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'etiqueta.etiqueta') AND name='creado_por' AND system_type_id=56)
    ALTER TABLE etiqueta.etiqueta ALTER COLUMN creado_por BIGINT NULL;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'etiqueta.etiqueta_entidad') AND name='asignado_por' AND system_type_id=56)
    ALTER TABLE etiqueta.etiqueta_entidad ALTER COLUMN asignado_por BIGINT NULL;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'tercero.tercero') AND name='creado_por' AND system_type_id=56)
    ALTER TABLE tercero.tercero ALTER COLUMN creado_por BIGINT NULL;
GO

/* ============================================================
   aux_tables_scope strategy (join obligatorio a entidad raiz)
   ============================================================ */
MERGE seguridad.entidad_alcance_dato AS t
USING (VALUES
(N'ACT.COMENTARIO', N'actividad.comentario', N'id_comentario', NULL, NULL, NULL, N'creado_por', N'JOIN_OBLIGATORIO_ENTIDAD_RAIZ', N'Comentarios: scope se resuelve por entidad raiz.'),
(N'DOC.DOCUMENTO_ENTIDAD', N'documento.documento_entidad', N'id_documento_entidad', NULL, NULL, NULL, N'id_usuario_asociacion', N'JOIN_OBLIGATORIO_ENTIDAD_RAIZ', N'Asociacion documento-entidad: scope por entidad raiz.'),
(N'ETQ.ENTIDAD', N'etiqueta.etiqueta_entidad', N'id_etiqueta_entidad', NULL, NULL, NULL, N'asignado_por', N'JOIN_OBLIGATORIO_ENTIDAD_RAIZ', N'Asignacion etiqueta-entidad: scope por entidad raiz.')
) AS s(codigo_entidad,nombre_tabla,columna_llave_primaria,columna_tenant,columna_empresa,columna_unidad_organizativa,columna_propietario,columna_contexto,descripcion)
ON t.codigo_entidad = s.codigo_entidad
WHEN MATCHED THEN
    UPDATE SET nombre_tabla=s.nombre_tabla,columna_llave_primaria=s.columna_llave_primaria,columna_tenant=s.columna_tenant,
               columna_empresa=s.columna_empresa,columna_unidad_organizativa=s.columna_unidad_organizativa,
               columna_propietario=s.columna_propietario,columna_contexto=s.columna_contexto,descripcion=s.descripcion,
               activo=1,actualizado_utc=SYSUTCDATETIME()
WHEN NOT MATCHED THEN
    INSERT(codigo_entidad,nombre_tabla,columna_llave_primaria,columna_tenant,columna_empresa,columna_unidad_organizativa,columna_propietario,columna_contexto,descripcion,activo,creado_utc,actualizado_utc)
    VALUES(s.codigo_entidad,s.nombre_tabla,s.columna_llave_primaria,s.columna_tenant,s.columna_empresa,s.columna_unidad_organizativa,s.columna_propietario,s.columna_contexto,s.descripcion,1,SYSUTCDATETIME(),SYSUTCDATETIME());
GO

/* ============================================================
   db_standards
   ============================================================ */
ALTER DATABASE CURRENT SET ALLOW_SNAPSHOT_ISOLATION ON;
ALTER DATABASE CURRENT SET READ_COMMITTED_SNAPSHOT ON WITH ROLLBACK IMMEDIATE;
ALTER DATABASE CURRENT SET ANSI_NULL_DEFAULT ON;
ALTER DATABASE CURRENT SET ANSI_NULLS ON;
ALTER DATABASE CURRENT SET ANSI_PADDING ON;
ALTER DATABASE CURRENT SET ANSI_WARNINGS ON;
ALTER DATABASE CURRENT SET ARITHABORT ON;
ALTER DATABASE CURRENT SET CONCAT_NULL_YIELDS_NULL ON;
ALTER DATABASE CURRENT SET QUOTED_IDENTIFIER ON;
GO
