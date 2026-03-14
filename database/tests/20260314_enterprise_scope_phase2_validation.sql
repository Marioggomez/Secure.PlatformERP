/* Validacion phase2 enterprise scope */
SET NOCOUNT ON;
SET XACT_ABORT ON;

PRINT '1) tenant isolation';
DECLARE @uA BIGINT = (SELECT TOP(1) id_usuario FROM seguridad.usuario_tenant WHERE activo=1 ORDER BY id_usuario);
DECLARE @tA BIGINT = (SELECT TOP(1) id_tenant FROM seguridad.usuario_tenant WHERE id_usuario=@uA AND activo=1 ORDER BY id_tenant);
DECLARE @tB BIGINT = (SELECT TOP(1) id_tenant FROM plataforma.tenant WHERE id_tenant<>@tA ORDER BY id_tenant);
IF @tB IS NOT NULL AND EXISTS(SELECT 1 FROM seguridad.fn_usuario_empresas_efectivas(@uA,@tB))
    THROW 51300, N'Fallo: fuga tenant->tenant detectada.', 1;

PRINT '2) single company user';
DECLARE @uSingle BIGINT = (
    SELECT TOP(1) ue.id_usuario
    FROM seguridad.usuario_empresa ue
    WHERE ue.activo=1 AND ue.puede_operar=1
    GROUP BY ue.id_usuario, ue.id_tenant
    HAVING COUNT(DISTINCT ue.id_empresa)=1
);
IF @uSingle IS NOT NULL
BEGIN
    DECLARE @tSingle BIGINT=(SELECT TOP(1) id_tenant FROM seguridad.usuario_empresa WHERE id_usuario=@uSingle ORDER BY id_tenant);
    DECLARE @eSingle BIGINT=(SELECT TOP(1) id_empresa FROM seguridad.usuario_empresa WHERE id_usuario=@uSingle AND id_tenant=@tSingle AND activo=1 AND puede_operar=1 ORDER BY id_empresa);
    IF EXISTS(SELECT 1 FROM seguridad.fn_usuario_empresas_efectivas(@uSingle,@tSingle) WHERE id_empresa<>@eSingle)
        THROW 51301, N'Fallo: usuario de empresa unica ve empresas no permitidas.', 1;
END;

PRINT '3) multi-company user';
DECLARE @uMulti BIGINT = (
    SELECT TOP(1) ue.id_usuario
    FROM seguridad.usuario_empresa ue
    WHERE ue.activo=1 AND ue.puede_operar=1
    GROUP BY ue.id_usuario, ue.id_tenant
    HAVING COUNT(DISTINCT ue.id_empresa)>1
);
IF @uMulti IS NOT NULL
BEGIN
    DECLARE @tMulti BIGINT=(SELECT TOP(1) id_tenant FROM seguridad.usuario_empresa WHERE id_usuario=@uMulti ORDER BY id_tenant);
    IF EXISTS(
        SELECT 1
        FROM seguridad.fn_usuario_empresas_efectivas(@uMulti,@tMulti) ef
        WHERE NOT EXISTS(
            SELECT 1 FROM seguridad.usuario_empresa ue
            WHERE ue.id_usuario=@uMulti AND ue.id_tenant=@tMulti AND ue.id_empresa=ef.id_empresa AND ue.activo=1 AND ue.puede_operar=1
        )
    )
        THROW 51302, N'Fallo: multi-company expone empresa fuera de asignacion.', 1;
END;

PRINT '4) unit scope';
DECLARE @uUnit BIGINT = (SELECT TOP(1) id_usuario FROM seguridad.usuario_scope_unidad WHERE activo=1 ORDER BY id_usuario);
IF @uUnit IS NOT NULL
BEGIN
    DECLARE @tUnit BIGINT=(SELECT TOP(1) id_tenant FROM seguridad.usuario_scope_unidad WHERE id_usuario=@uUnit AND activo=1 ORDER BY id_tenant);
    DECLARE @eUnit BIGINT=(SELECT TOP(1) id_empresa FROM seguridad.usuario_scope_unidad WHERE id_usuario=@uUnit AND id_tenant=@tUnit AND activo=1 ORDER BY id_empresa);
    IF EXISTS(
        SELECT 1 FROM seguridad.fn_usuario_unidades_efectivas(@uUnit,@tUnit,@eUnit) u
        WHERE NOT EXISTS(
            SELECT 1 FROM seguridad.usuario_scope_unidad su
            WHERE su.id_usuario=@uUnit AND su.id_tenant=@tUnit AND su.id_empresa=@eUnit AND su.id_unidad_organizativa=u.id_unidad_organizativa
              AND su.activo=1
              AND su.fecha_inicio_utc<=SYSUTCDATETIME()
              AND (su.fecha_fin_utc IS NULL OR su.fecha_fin_utc>=SYSUTCDATETIME())
        )
    )
        THROW 51303, N'Fallo: scope unidad inconsistente.', 1;
END;

PRINT '5) no effective scope';
DECLARE @uNo BIGINT = (
    SELECT TOP(1) u.id_usuario
    FROM seguridad.usuario u
    WHERE NOT EXISTS(SELECT 1 FROM seguridad.usuario_empresa ue WHERE ue.id_usuario=u.id_usuario AND ue.activo=1 AND ue.puede_operar=1)
      AND EXISTS(SELECT 1 FROM seguridad.usuario_tenant ut WHERE ut.id_usuario=u.id_usuario AND ut.activo=1)
);
IF @uNo IS NOT NULL
BEGIN
    DECLARE @tNo BIGINT=(SELECT TOP(1) id_tenant FROM seguridad.usuario_tenant WHERE id_usuario=@uNo AND activo=1 ORDER BY id_tenant);
    IF EXISTS(SELECT 1 FROM seguridad.fn_usuario_empresas_efectivas(@uNo,@tNo))
        THROW 51304, N'Fallo: usuario sin scope ve empresas.', 1;
END;

PRINT '6) fallback auth disabled';
IF EXISTS(
    SELECT 1
    FROM sys.sql_modules m
    JOIN sys.objects o ON o.object_id=m.object_id
    JOIN sys.schemas s ON s.schema_id=o.schema_id
    WHERE s.name='seguridad' AND o.name='usp_auth_obtener_usuario_para_autenticacion'
      AND m.definition LIKE '%COALESCE(ue.id_empresa, emp.id_empresa)%'
)
    THROW 51305, N'Fallo: fallback de empresa aun presente.', 1;

PRINT '7) ui layout separated';
IF EXISTS(
    SELECT 1 FROM seguridad.filtro_dato_usuario f
    JOIN catalogo.modo_filtro_dato m ON m.id_modo_filtro_dato=f.id_modo_filtro_dato
    WHERE UPPER(m.codigo)='UI_LAYOUT'
)
    THROW 51306, N'Fallo: UI_LAYOUT sigue en filtro_dato_usuario.', 1;

PRINT '8) fk simple+composite duplicates = 0';
IF OBJECT_ID('tempdb..#fk_cols') IS NOT NULL DROP TABLE #fk_cols;
CREATE TABLE #fk_cols
(
    fk_name SYSNAME NOT NULL,
    parent_table NVARCHAR(260) NOT NULL,
    ref_table NVARCHAR(260) NOT NULL,
    parent_cols NVARCHAR(4000) NOT NULL
);

INSERT INTO #fk_cols (fk_name, parent_table, ref_table, parent_cols)
SELECT fk.name AS fk_name,
       schp.name + '.' + tp.name AS parent_table,
       schr.name + '.' + tr.name AS ref_table,
       STRING_AGG(cp.name, ',') WITHIN GROUP (ORDER BY fkc.constraint_column_id) AS parent_cols
FROM sys.foreign_keys fk
JOIN sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
JOIN sys.tables tp ON tp.object_id = fk.parent_object_id
JOIN sys.schemas schp ON schp.schema_id = tp.schema_id
JOIN sys.columns cp ON cp.object_id = tp.object_id AND cp.column_id = fkc.parent_column_id
JOIN sys.tables tr ON tr.object_id = fk.referenced_object_id
JOIN sys.schemas schr ON schr.schema_id = tr.schema_id
GROUP BY fk.name, schp.name, tp.name, schr.name, tr.name;

IF EXISTS(
    SELECT 1
    FROM #fk_cols a
    JOIN #fk_cols b ON b.parent_table=a.parent_table AND b.ref_table=a.ref_table
    WHERE a.ref_table='organizacion.empresa' AND a.parent_cols='id_empresa' AND b.parent_cols='id_tenant,id_empresa'
)
    THROW 51307, N'Fallo: persisten FKs simples duplicadas.', 1;

PRINT '9) bigint alignment';
IF EXISTS(
    SELECT 1
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE COLUMN_NAME IN ('id_usuario','creado_por','editado_por','asignado_por','id_usuario_asociacion')
      AND DATA_TYPE='int'
      AND TABLE_SCHEMA IN ('actividad','documento','etiqueta','tercero')
)
    THROW 51308, N'Fallo: persisten columnas de usuario como int.', 1;

PRINT '10) db standards';
IF EXISTS(
    SELECT 1
    FROM sys.databases
    WHERE name=DB_NAME()
      AND (
           is_read_committed_snapshot_on=0 OR snapshot_isolation_state_desc<>'ON'
        OR is_ansi_null_default_on=0 OR is_ansi_nulls_on=0 OR is_ansi_padding_on=0
        OR is_ansi_warnings_on=0 OR is_arithabort_on=0
        OR is_concat_null_yields_null_on=0 OR is_quoted_identifier_on=0
      )
)
    THROW 51309, N'Fallo: DB settings no cumplen baseline enterprise.', 1;

PRINT '11) idempotency rerun migration success required (manual gate)';
PRINT 'OK phase2';
