CREATE OR ALTER PROCEDURE catalogo.usp_estado_usuario_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_estado_usuario], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.estado_usuario;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_estado_usuario_obtener
    @id_estado_usuario smallint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_estado_usuario], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.estado_usuario
    WHERE [id_estado_usuario] = @id_estado_usuario;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_estado_usuario_crear
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
    INSERT INTO catalogo.estado_usuario ([codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc])
    VALUES (@codigo, @nombre, @descripcion, @orden_visual, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS smallint) AS id;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_estado_usuario_actualizar
    @id_estado_usuario smallint,
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
    UPDATE catalogo.estado_usuario
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [orden_visual] = @orden_visual,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_estado_usuario] = @id_estado_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_estado_usuario_desactivar
    @id_estado_usuario smallint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE catalogo.estado_usuario
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_estado_usuario] = @id_estado_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
