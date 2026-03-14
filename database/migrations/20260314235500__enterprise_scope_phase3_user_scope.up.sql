/*
    Migracion: 20260314235500__enterprise_scope_phase3_user_scope
    Objetivo: cerrar user-scope canonico, limpiar residuos legacy de metadata UI,
              endurecer metadata de alcance por entidad raiz y alinear tipos residuales.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ============================================================
   1) entidad_alcance_dato: columnas de estrategia formal
   ============================================================ */
IF COL_LENGTH('seguridad.entidad_alcance_dato', 'modo_scope') IS NULL
BEGIN
    ALTER TABLE seguridad.entidad_alcance_dato
        ADD modo_scope NVARCHAR(32) NULL;
END;

IF COL_LENGTH('seguridad.entidad_alcance_dato', 'codigo_entidad_raiz') IS NULL
BEGIN
    ALTER TABLE seguridad.entidad_alcance_dato
        ADD codigo_entidad_raiz NVARCHAR(256) NULL;
END;

IF NOT EXISTS (
    SELECT 1
    FROM sys.check_constraints
    WHERE parent_object_id = OBJECT_ID('seguridad.entidad_alcance_dato')
      AND name = 'CK_seguridad_entidad_alcance_dato_modo_scope'
)
BEGIN
    EXEC(N'
        ALTER TABLE seguridad.entidad_alcance_dato
            ADD CONSTRAINT CK_seguridad_entidad_alcance_dato_modo_scope
            CHECK (modo_scope IS NULL OR modo_scope IN (N''DIRECTO'', N''POR_ENTIDAD_RAIZ'', N''HIBRIDO''));
    ');
END;
GO

UPDATE seguridad.entidad_alcance_dato
SET modo_scope = CASE WHEN columna_contexto = N'JOIN_OBLIGATORIO_ENTIDAD_RAIZ' THEN N'POR_ENTIDAD_RAIZ' ELSE N'DIRECTO' END
WHERE modo_scope IS NULL;

/* Remover residuos legacy UI_LAYOUT_* de metadata de seguridad de datos */
DELETE FROM seguridad.entidad_alcance_dato
WHERE codigo_entidad LIKE N'UI_LAYOUT_%';
GO

/* ============================================================
   2) estrategia formal para tablas auxiliares / satelite
   ============================================================ */
MERGE seguridad.entidad_alcance_dato AS tgt
USING
(
    VALUES
    (N'ACT.COMENTARIO', N'actividad.comentario', N'id_comentario', NULL, NULL, NULL, N'creado_por', N'JOIN_OBLIGATORIO_ENTIDAD_RAIZ', N'POR_ENTIDAD_RAIZ', N'ACT.ENTIDAD_RAIZ', N'Comentario protegido por entidad raiz.'),
    (N'ACT.COMENTARIO_MENCION', N'actividad.comentario_mencion', N'id_comentario_mencion', NULL, NULL, NULL, N'id_usuario', N'JOIN_OBLIGATORIO_ENTIDAD_RAIZ', N'POR_ENTIDAD_RAIZ', N'ACT.COMENTARIO', N'Mencion protegida por comentario raiz.'),
    (N'DOC.DOCUMENTO', N'documento.documento', N'id_documento', NULL, NULL, NULL, N'creado_por', N'JOIN_OBLIGATORIO_ENTIDAD_RAIZ', N'POR_ENTIDAD_RAIZ', N'DOC.ENTIDAD_RAIZ', N'Documento protegido por entidad asociada.'),
    (N'DOC.DOCUMENTO_ENTIDAD', N'documento.documento_entidad', N'id_documento_entidad', NULL, NULL, NULL, N'id_usuario_asociacion', N'JOIN_OBLIGATORIO_ENTIDAD_RAIZ', N'POR_ENTIDAD_RAIZ', N'DOC.ENTIDAD_RAIZ', N'Asociacion documento-entidad protegida por entidad raiz.'),
    (N'DOC.DOCUMENTO_VERSION', N'documento.documento_version', N'id_documento_version', NULL, NULL, NULL, N'creado_por', N'JOIN_OBLIGATORIO_ENTIDAD_RAIZ', N'POR_ENTIDAD_RAIZ', N'DOC.DOCUMENTO', N'Version de documento protegida por documento raiz.'),
    (N'DOC.DOCUMENTO_FIRMA', N'documento.documento_firma', N'id_documento_firma', NULL, NULL, NULL, N'id_usuario_firmante', N'JOIN_OBLIGATORIO_ENTIDAD_RAIZ', N'POR_ENTIDAD_RAIZ', N'DOC.DOCUMENTO', N'Firma de documento protegida por documento raiz.'),
    (N'DOC.DOCUMENTO_WORKFLOW_PASO', N'documento.documento_workflow_paso', N'id_documento_workflow_paso', NULL, NULL, NULL, N'id_usuario_aprobador', N'JOIN_OBLIGATORIO_ENTIDAD_RAIZ', N'POR_ENTIDAD_RAIZ', N'DOC.DOCUMENTO', N'Paso workflow documento protegido por documento raiz.'),
    (N'ETQ.ENTIDAD', N'etiqueta.etiqueta_entidad', N'id_etiqueta_entidad', NULL, NULL, NULL, N'asignado_por', N'JOIN_OBLIGATORIO_ENTIDAD_RAIZ', N'POR_ENTIDAD_RAIZ', N'ETQ.ENTIDAD_RAIZ', N'Etiqueta-entidad protegida por entidad raiz.')
) AS src
(
    codigo_entidad,
    nombre_tabla,
    columna_llave_primaria,
    columna_tenant,
    columna_empresa,
    columna_unidad_organizativa,
    columna_propietario,
    columna_contexto,
    modo_scope,
    codigo_entidad_raiz,
    descripcion
)
ON tgt.codigo_entidad = src.codigo_entidad
WHEN MATCHED THEN
    UPDATE SET
        tgt.nombre_tabla = src.nombre_tabla,
        tgt.columna_llave_primaria = src.columna_llave_primaria,
        tgt.columna_tenant = src.columna_tenant,
        tgt.columna_empresa = src.columna_empresa,
        tgt.columna_unidad_organizativa = src.columna_unidad_organizativa,
        tgt.columna_propietario = src.columna_propietario,
        tgt.columna_contexto = src.columna_contexto,
        tgt.modo_scope = src.modo_scope,
        tgt.codigo_entidad_raiz = src.codigo_entidad_raiz,
        tgt.descripcion = src.descripcion,
        tgt.activo = 1,
        tgt.actualizado_utc = SYSUTCDATETIME()
WHEN NOT MATCHED THEN
    INSERT
    (
        codigo_entidad, nombre_tabla, columna_llave_primaria, columna_tenant, columna_empresa,
        columna_unidad_organizativa, columna_propietario, columna_contexto, descripcion,
        activo, creado_utc, actualizado_utc, modo_scope, codigo_entidad_raiz
    )
    VALUES
    (
        src.codigo_entidad, src.nombre_tabla, src.columna_llave_primaria, src.columna_tenant, src.columna_empresa,
        src.columna_unidad_organizativa, src.columna_propietario, src.columna_contexto, src.descripcion,
        1, SYSUTCDATETIME(), SYSUTCDATETIME(), src.modo_scope, src.codigo_entidad_raiz
    );
GO

/* ============================================================
   3) user scope canonico: seguridad.usuario_scope_usuario
   ============================================================ */
IF OBJECT_ID(N'seguridad.usuario_scope_usuario', N'U') IS NULL
BEGIN
    CREATE TABLE seguridad.usuario_scope_usuario
    (
        id_usuario_scope_usuario BIGINT IDENTITY(1,1) NOT NULL
            CONSTRAINT PK_seguridad_usuario_scope_usuario PRIMARY KEY,
        id_tenant BIGINT NOT NULL,
        id_empresa BIGINT NULL,
        id_usuario_origen BIGINT NOT NULL,
        id_usuario_destino BIGINT NOT NULL,
        fecha_inicio_utc DATETIME2 NOT NULL
            CONSTRAINT DF_seguridad_usuario_scope_usuario_fecha_inicio_utc DEFAULT SYSUTCDATETIME(),
        fecha_fin_utc DATETIME2 NULL,
        activo BIT NOT NULL
            CONSTRAINT DF_seguridad_usuario_scope_usuario_activo DEFAULT (1),
        creado_por BIGINT NULL,
        creado_utc DATETIME2 NOT NULL
            CONSTRAINT DF_seguridad_usuario_scope_usuario_creado_utc DEFAULT SYSUTCDATETIME(),
        actualizado_utc DATETIME2 NULL
    );
END;

IF NOT EXISTS (
    SELECT 1 FROM sys.check_constraints
    WHERE parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_usuario')
      AND name = N'CK_seguridad_usuario_scope_usuario_rango_fechas'
)
BEGIN
    ALTER TABLE seguridad.usuario_scope_usuario
        ADD CONSTRAINT CK_seguridad_usuario_scope_usuario_rango_fechas
        CHECK (fecha_fin_utc IS NULL OR fecha_fin_utc >= fecha_inicio_utc);
END;

IF NOT EXISTS (
    SELECT 1 FROM sys.check_constraints
    WHERE parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_usuario')
      AND name = N'CK_seguridad_usuario_scope_usuario_origen_destino'
)
BEGIN
    ALTER TABLE seguridad.usuario_scope_usuario
        ADD CONSTRAINT CK_seguridad_usuario_scope_usuario_origen_destino
        CHECK (id_usuario_origen > 0 AND id_usuario_destino > 0);
END;

IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_usuario')
      AND name = N'FK_seguridad_usuario_scope_usuario_usuario_origen'
)
BEGIN
    ALTER TABLE seguridad.usuario_scope_usuario WITH CHECK
        ADD CONSTRAINT FK_seguridad_usuario_scope_usuario_usuario_origen
        FOREIGN KEY (id_usuario_origen)
        REFERENCES seguridad.usuario(id_usuario);
END;

IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_usuario')
      AND name = N'FK_seguridad_usuario_scope_usuario_usuario_destino'
)
BEGIN
    ALTER TABLE seguridad.usuario_scope_usuario WITH CHECK
        ADD CONSTRAINT FK_seguridad_usuario_scope_usuario_usuario_destino
        FOREIGN KEY (id_usuario_destino)
        REFERENCES seguridad.usuario(id_usuario);
END;

IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_usuario')
      AND name = N'FK_seguridad_usuario_scope_usuario_usuario_tenant_origen'
)
BEGIN
    ALTER TABLE seguridad.usuario_scope_usuario WITH CHECK
        ADD CONSTRAINT FK_seguridad_usuario_scope_usuario_usuario_tenant_origen
        FOREIGN KEY (id_usuario_origen, id_tenant)
        REFERENCES seguridad.usuario_tenant(id_usuario, id_tenant);
END;

IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_usuario')
      AND name = N'FK_seguridad_usuario_scope_usuario_usuario_tenant_destino'
)
BEGIN
    ALTER TABLE seguridad.usuario_scope_usuario WITH CHECK
        ADD CONSTRAINT FK_seguridad_usuario_scope_usuario_usuario_tenant_destino
        FOREIGN KEY (id_usuario_destino, id_tenant)
        REFERENCES seguridad.usuario_tenant(id_usuario, id_tenant);
END;

IF NOT EXISTS (
    SELECT 1 FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_usuario')
      AND name = N'FK_seguridad_usuario_scope_usuario_empresa_tenant'
)
BEGIN
    ALTER TABLE seguridad.usuario_scope_usuario WITH CHECK
        ADD CONSTRAINT FK_seguridad_usuario_scope_usuario_empresa_tenant
        FOREIGN KEY (id_tenant, id_empresa)
        REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'seguridad.usuario_scope_usuario')
      AND name = N'IX_seguridad_usuario_scope_usuario_origen_tenant_activo'
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_seguridad_usuario_scope_usuario_origen_tenant_activo
        ON seguridad.usuario_scope_usuario(id_usuario_origen, id_tenant, activo, fecha_inicio_utc, fecha_fin_utc)
        INCLUDE (id_usuario_destino, id_empresa);
END;

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'seguridad.usuario_scope_usuario')
      AND name = N'IX_seguridad_usuario_scope_usuario_destino_tenant_activo'
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_seguridad_usuario_scope_usuario_destino_tenant_activo
        ON seguridad.usuario_scope_usuario(id_usuario_destino, id_tenant, activo, fecha_inicio_utc, fecha_fin_utc)
        INCLUDE (id_usuario_origen, id_empresa);
END;

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'seguridad.usuario_scope_usuario')
      AND name = N'UX_seguridad_usuario_scope_usuario_origen_destino_global_activo'
)
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_seguridad_usuario_scope_usuario_origen_destino_global_activo
        ON seguridad.usuario_scope_usuario(id_tenant, id_usuario_origen, id_usuario_destino)
        WHERE activo = 1 AND id_empresa IS NULL;
END;

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'seguridad.usuario_scope_usuario')
      AND name = N'UX_seguridad_usuario_scope_usuario_origen_destino_empresa_activo'
)
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_seguridad_usuario_scope_usuario_origen_destino_empresa_activo
        ON seguridad.usuario_scope_usuario(id_tenant, id_empresa, id_usuario_origen, id_usuario_destino)
        WHERE activo = 1 AND id_empresa IS NOT NULL;
END;
GO

/* ============================================================
   4) motor canonico: usuarios efectivos por alcance
   ============================================================ */
CREATE OR ALTER FUNCTION seguridad.fn_usuario_usuarios_efectivos
(
    @id_usuario BIGINT,
    @id_tenant BIGINT,
    @id_empresa BIGINT = NULL
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
      AND ut.es_administrador_tenant = 1
),
scope_explicito AS
(
    SELECT usu.id_usuario_destino
    FROM seguridad.usuario_scope_usuario usu
    CROSS JOIN utc_now n
    WHERE usu.id_usuario_origen = @id_usuario
      AND usu.id_tenant = @id_tenant
      AND usu.activo = 1
      AND usu.fecha_inicio_utc <= n.now_utc
      AND (usu.fecha_fin_utc IS NULL OR usu.fecha_fin_utc >= n.now_utc)
      AND (usu.id_empresa IS NULL OR usu.id_empresa = @id_empresa)
)
SELECT DISTINCT x.id_usuario
FROM
(
    SELECT @id_usuario AS id_usuario

    UNION ALL

    SELECT se.id_usuario_destino
    FROM scope_explicito se

    UNION ALL

    SELECT ut2.id_usuario
    FROM admin_tenant at
    JOIN seguridad.usuario_tenant ut2
      ON ut2.id_tenant = @id_tenant
     AND ut2.activo = 1
    WHERE
        @id_empresa IS NULL
        OR EXISTS
        (
            SELECT 1
            FROM seguridad.usuario_empresa ue2
            CROSS JOIN utc_now n
            WHERE ue2.id_usuario = ut2.id_usuario
              AND ue2.id_tenant = @id_tenant
              AND ue2.id_empresa = @id_empresa
              AND ue2.activo = 1
              AND ue2.puede_operar = 1
              AND ue2.fecha_inicio_utc <= n.now_utc
              AND (ue2.fecha_fin_utc IS NULL OR ue2.fecha_fin_utc >= n.now_utc)
        )
) AS x;
GO

CREATE OR ALTER VIEW seguridad.vw_usuario_scope_usuario_efectivo
AS
SELECT
    ut.id_usuario AS id_usuario_origen,
    ut.id_tenant,
    emp.id_empresa,
    usr.id_usuario AS id_usuario_destino
FROM seguridad.usuario_tenant ut
CROSS APPLY seguridad.fn_usuario_empresas_efectivas(ut.id_usuario, ut.id_tenant) emp
CROSS APPLY seguridad.fn_usuario_usuarios_efectivos(ut.id_usuario, ut.id_tenant, emp.id_empresa) usr
WHERE ut.activo = 1;
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_usuario_listar
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL
        THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;

    DECLARE @now DATETIME2 = SYSUTCDATETIME();

    SELECT
        id_usuario_scope_usuario,
        id_tenant,
        id_empresa,
        id_usuario_origen,
        id_usuario_destino,
        fecha_inicio_utc,
        fecha_fin_utc,
        activo,
        creado_por,
        creado_utc,
        actualizado_utc
    FROM seguridad.usuario_scope_usuario
    WHERE id_tenant = @ctx_id_tenant
      AND activo = 1
      AND fecha_inicio_utc <= @now
      AND (fecha_fin_utc IS NULL OR fecha_fin_utc >= @now)
    ORDER BY id_usuario_origen, id_usuario_destino, id_empresa;
END;
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_usuario_guardar
    @id_tenant BIGINT,
    @id_usuario_origen BIGINT,
    @id_usuario_destino BIGINT,
    @id_empresa BIGINT = NULL,
    @fecha_inicio_utc DATETIME2 = NULL,
    @fecha_fin_utc DATETIME2 = NULL,
    @activo BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    IF @id_tenant IS NULL OR @id_usuario_origen IS NULL OR @id_usuario_destino IS NULL
        THROW 51230, N'Parametros obligatorios incompletos para usuario_scope_usuario.', 1;

    IF @fecha_inicio_utc IS NULL
        SET @fecha_inicio_utc = SYSUTCDATETIME();

    IF @fecha_fin_utc IS NOT NULL AND @fecha_fin_utc < @fecha_inicio_utc
        THROW 51231, N'Rango de fechas invalido para usuario_scope_usuario.', 1;

    DECLARE @creado_por BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_usuario') AS BIGINT);

    IF @activo = 1
    BEGIN
        UPDATE seguridad.usuario_scope_usuario
        SET activo = 0,
            actualizado_utc = SYSUTCDATETIME()
        WHERE id_tenant = @id_tenant
          AND id_usuario_origen = @id_usuario_origen
          AND id_usuario_destino = @id_usuario_destino
          AND ((id_empresa IS NULL AND @id_empresa IS NULL) OR id_empresa = @id_empresa)
          AND activo = 1;
    END;

    INSERT INTO seguridad.usuario_scope_usuario
    (
        id_tenant,
        id_empresa,
        id_usuario_origen,
        id_usuario_destino,
        fecha_inicio_utc,
        fecha_fin_utc,
        activo,
        creado_por,
        creado_utc,
        actualizado_utc
    )
    VALUES
    (
        @id_tenant,
        @id_empresa,
        @id_usuario_origen,
        @id_usuario_destino,
        @fecha_inicio_utc,
        @fecha_fin_utc,
        @activo,
        @creado_por,
        SYSUTCDATETIME(),
        NULL
    );

    SELECT SCOPE_IDENTITY() AS id_usuario_scope_usuario;
END;
GO

/* ============================================================
   5) residual bigint alignment
   ============================================================ */
IF EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'documento.documento_firma')
      AND name = N'id_usuario_firmante'
      AND system_type_id = 56
)
BEGIN
    ALTER TABLE documento.documento_firma
        ALTER COLUMN id_usuario_firmante BIGINT NOT NULL;
END;

IF EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'documento.documento_workflow_paso')
      AND name = N'id_usuario_aprobador'
      AND system_type_id = 56
)
BEGIN
    ALTER TABLE documento.documento_workflow_paso
        ALTER COLUMN id_usuario_aprobador BIGINT NOT NULL;
END;
GO

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'documento.documento_firma')
      AND name = N'IX_documento_firma_id_usuario_firmante'
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_documento_firma_id_usuario_firmante
        ON documento.documento_firma(id_usuario_firmante)
        INCLUDE (id_documento, fecha_firma, estado_firma);
END;

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'documento.documento_workflow_paso')
      AND name = N'IX_documento_workflow_paso_id_usuario_aprobador'
)
BEGIN
    CREATE NONCLUSTERED INDEX IX_documento_workflow_paso_id_usuario_aprobador
        ON documento.documento_workflow_paso(id_usuario_aprobador)
        INCLUDE (id_documento_workflow, estado_paso, fecha_aprobacion);
END;
GO

/* ============================================================
   6) catalogo entidad para user-scope explicito
   ============================================================ */
MERGE seguridad.entidad_alcance_dato AS tgt
USING
(
    VALUES
    (N'SEG.USUARIO_SCOPE_USUARIO', N'seguridad.usuario_scope_usuario', N'id_usuario_scope_usuario', N'id_tenant', N'id_empresa', NULL, N'id_usuario_destino', N'SCOPE_EXPLICITO_USUARIO', N'DIRECTO', NULL, N'Reglas explicitas de visibilidad usuario-a-usuario.')
) AS src
(
    codigo_entidad,
    nombre_tabla,
    columna_llave_primaria,
    columna_tenant,
    columna_empresa,
    columna_unidad_organizativa,
    columna_propietario,
    columna_contexto,
    modo_scope,
    codigo_entidad_raiz,
    descripcion
)
ON tgt.codigo_entidad = src.codigo_entidad
WHEN MATCHED THEN
    UPDATE SET
        tgt.nombre_tabla = src.nombre_tabla,
        tgt.columna_llave_primaria = src.columna_llave_primaria,
        tgt.columna_tenant = src.columna_tenant,
        tgt.columna_empresa = src.columna_empresa,
        tgt.columna_unidad_organizativa = src.columna_unidad_organizativa,
        tgt.columna_propietario = src.columna_propietario,
        tgt.columna_contexto = src.columna_contexto,
        tgt.modo_scope = src.modo_scope,
        tgt.codigo_entidad_raiz = src.codigo_entidad_raiz,
        tgt.descripcion = src.descripcion,
        tgt.activo = 1,
        tgt.actualizado_utc = SYSUTCDATETIME()
WHEN NOT MATCHED THEN
    INSERT
    (
        codigo_entidad, nombre_tabla, columna_llave_primaria, columna_tenant, columna_empresa,
        columna_unidad_organizativa, columna_propietario, columna_contexto, descripcion,
        activo, creado_utc, actualizado_utc, modo_scope, codigo_entidad_raiz
    )
    VALUES
    (
        src.codigo_entidad, src.nombre_tabla, src.columna_llave_primaria, src.columna_tenant, src.columna_empresa,
        src.columna_unidad_organizativa, src.columna_propietario, src.columna_contexto, src.descripcion,
        1, SYSUTCDATETIME(), SYSUTCDATETIME(), src.modo_scope, src.codigo_entidad_raiz
    );
GO
