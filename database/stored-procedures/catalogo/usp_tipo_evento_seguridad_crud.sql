CREATE OR ALTER PROCEDURE catalogo.usp_tipo_evento_seguridad_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tipo_evento_seguridad], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.tipo_evento_seguridad;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_tipo_evento_seguridad_obtener
    @id_tipo_evento_seguridad smallint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tipo_evento_seguridad], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.tipo_evento_seguridad
    WHERE [id_tipo_evento_seguridad] = @id_tipo_evento_seguridad;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_tipo_evento_seguridad_crear
    @codigo varchar(30),
    @nombre nvarchar(120),
    @descripcion nvarchar(300),
    @orden_visual smallint,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO catalogo.tipo_evento_seguridad ([codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc])
    VALUES (@codigo, @nombre, @descripcion, @orden_visual, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS smallint) AS id;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_tipo_evento_seguridad_actualizar
    @id_tipo_evento_seguridad smallint,
    @codigo varchar(30),
    @nombre nvarchar(120),
    @descripcion nvarchar(300),
    @orden_visual smallint,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE catalogo.tipo_evento_seguridad
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [orden_visual] = @orden_visual,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_tipo_evento_seguridad] = @id_tipo_evento_seguridad;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_tipo_evento_seguridad_desactivar
    @id_tipo_evento_seguridad smallint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE catalogo.tipo_evento_seguridad
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_tipo_evento_seguridad] = @id_tipo_evento_seguridad;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
