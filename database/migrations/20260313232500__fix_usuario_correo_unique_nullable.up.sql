/*
    Fix enterprise IAM:
    Permitir multiples usuarios sin correo (NULL) y mantener unicidad
    solo para valores informados de correo_normalizado.
*/

IF EXISTS (
    SELECT 1
    FROM sys.key_constraints kc
    INNER JOIN sys.tables t ON t.object_id = kc.parent_object_id
    INNER JOIN sys.schemas s ON s.schema_id = t.schema_id
    WHERE s.name = 'seguridad'
      AND t.name = 'usuario'
      AND kc.name = 'UQ_seguridad_usuario_correo_normalizado'
)
BEGIN
    ALTER TABLE seguridad.usuario
    DROP CONSTRAINT UQ_seguridad_usuario_correo_normalizado;
END
GO

IF EXISTS (
    SELECT 1
    FROM sys.indexes i
    INNER JOIN sys.tables t ON t.object_id = i.object_id
    INNER JOIN sys.schemas s ON s.schema_id = t.schema_id
    WHERE s.name = 'seguridad'
      AND t.name = 'usuario'
      AND i.name = 'UQ_seguridad_usuario_correo_normalizado'
)
BEGIN
    DROP INDEX UQ_seguridad_usuario_correo_normalizado
    ON seguridad.usuario;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE UNIQUE NONCLUSTERED INDEX UQ_seguridad_usuario_correo_normalizado
    ON seguridad.usuario (correo_normalizado)
    WHERE correo_normalizado IS NOT NULL;
GO
