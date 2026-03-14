/*
    Rollback: 20260314235500__enterprise_scope_phase3_user_scope
    Nota: rollback conservador. Revierte objetos nuevos y metadatos agregados por phase3.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/* ============================================================
   1) remover procedimientos y vista nuevos de user-scope
   ============================================================ */
IF OBJECT_ID(N'seguridad.usp_usuario_scope_usuario_guardar', N'P') IS NOT NULL
    DROP PROCEDURE seguridad.usp_usuario_scope_usuario_guardar;

IF OBJECT_ID(N'seguridad.usp_usuario_scope_usuario_listar', N'P') IS NOT NULL
    DROP PROCEDURE seguridad.usp_usuario_scope_usuario_listar;

IF OBJECT_ID(N'seguridad.vw_usuario_scope_usuario_efectivo', N'V') IS NOT NULL
    DROP VIEW seguridad.vw_usuario_scope_usuario_efectivo;

IF OBJECT_ID(N'seguridad.fn_usuario_usuarios_efectivos', N'IF') IS NOT NULL
    DROP FUNCTION seguridad.fn_usuario_usuarios_efectivos;
GO

/* ============================================================
   2) eliminar tabla canonica usuario_scope_usuario
   ============================================================ */
IF OBJECT_ID(N'seguridad.usuario_scope_usuario', N'U') IS NOT NULL
    DROP TABLE seguridad.usuario_scope_usuario;
GO

/* ============================================================
   3) revertir bigint alignment residual (solo si seguro)
   ============================================================ */
IF EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'documento.documento_firma')
      AND name = N'id_usuario_firmante'
      AND system_type_id = 127
)
AND NOT EXISTS (
    SELECT 1
    FROM documento.documento_firma
    WHERE id_usuario_firmante > 2147483647 OR id_usuario_firmante < -2147483648
)
BEGIN
    ALTER TABLE documento.documento_firma
        ALTER COLUMN id_usuario_firmante INT NOT NULL;
END;

IF EXISTS (
    SELECT 1
    FROM sys.columns
    WHERE object_id = OBJECT_ID(N'documento.documento_workflow_paso')
      AND name = N'id_usuario_aprobador'
      AND system_type_id = 127
)
AND NOT EXISTS (
    SELECT 1
    FROM documento.documento_workflow_paso
    WHERE id_usuario_aprobador > 2147483647 OR id_usuario_aprobador < -2147483648
)
BEGIN
    ALTER TABLE documento.documento_workflow_paso
        ALTER COLUMN id_usuario_aprobador INT NOT NULL;
END;
GO

IF EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'documento.documento_firma')
      AND name = N'IX_documento_firma_id_usuario_firmante'
)
BEGIN
    DROP INDEX IX_documento_firma_id_usuario_firmante
        ON documento.documento_firma;
END;

IF EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'documento.documento_workflow_paso')
      AND name = N'IX_documento_workflow_paso_id_usuario_aprobador'
)
BEGIN
    DROP INDEX IX_documento_workflow_paso_id_usuario_aprobador
        ON documento.documento_workflow_paso;
END;
GO

/* ============================================================
   4) metadata scope: revertir solo lo agregado en phase3
   ============================================================ */
DELETE FROM seguridad.entidad_alcance_dato
WHERE codigo_entidad IN
(
    N'ACT.COMENTARIO_MENCION',
    N'DOC.DOCUMENTO',
    N'DOC.DOCUMENTO_VERSION',
    N'DOC.DOCUMENTO_FIRMA',
    N'DOC.DOCUMENTO_WORKFLOW_PASO',
    N'SEG.USUARIO_SCOPE_USUARIO'
);

UPDATE seguridad.entidad_alcance_dato
SET modo_scope = NULL,
    codigo_entidad_raiz = NULL,
    actualizado_utc = SYSUTCDATETIME()
WHERE codigo_entidad IN
(
    N'ACT.COMENTARIO',
    N'DOC.DOCUMENTO_ENTIDAD',
    N'ETQ.ENTIDAD'
);
GO

/* ============================================================
   5) columnas adicionales (conservador: no se eliminan si tienen uso)
   ============================================================ */
IF COL_LENGTH('seguridad.entidad_alcance_dato', 'modo_scope') IS NOT NULL
AND NOT EXISTS (
    SELECT 1
    FROM seguridad.entidad_alcance_dato
    WHERE modo_scope IS NOT NULL
)
BEGIN
    ALTER TABLE seguridad.entidad_alcance_dato DROP CONSTRAINT CK_seguridad_entidad_alcance_dato_modo_scope;
    ALTER TABLE seguridad.entidad_alcance_dato DROP COLUMN modo_scope;
END;

IF COL_LENGTH('seguridad.entidad_alcance_dato', 'codigo_entidad_raiz') IS NOT NULL
AND NOT EXISTS (
    SELECT 1
    FROM seguridad.entidad_alcance_dato
    WHERE codigo_entidad_raiz IS NOT NULL
)
BEGIN
    ALTER TABLE seguridad.entidad_alcance_dato DROP COLUMN codigo_entidad_raiz;
END;
GO
