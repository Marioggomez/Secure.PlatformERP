CREATE OR ALTER PROCEDURE catalogo.usp_canal_notificacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_canal_notificacion], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.canal_notificacion;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_canal_notificacion_obtener
    @id_canal_notificacion smallint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_canal_notificacion], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.canal_notificacion
    WHERE [id_canal_notificacion] = @id_canal_notificacion;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_canal_notificacion_crear
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
    INSERT INTO catalogo.canal_notificacion ([codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc])
    VALUES (@codigo, @nombre, @descripcion, @orden_visual, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS smallint) AS id;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_canal_notificacion_actualizar
    @id_canal_notificacion smallint,
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
    UPDATE catalogo.canal_notificacion
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [orden_visual] = @orden_visual,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_canal_notificacion] = @id_canal_notificacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_canal_notificacion_desactivar
    @id_canal_notificacion smallint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE catalogo.canal_notificacion
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_canal_notificacion] = @id_canal_notificacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
