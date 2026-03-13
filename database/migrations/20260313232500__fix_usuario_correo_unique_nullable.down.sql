/*
    Rollback fix de unicidad en correo_normalizado.
*/

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

ALTER TABLE seguridad.usuario
ADD CONSTRAINT UQ_seguridad_usuario_correo_normalizado
    UNIQUE (correo_normalizado);
GO
