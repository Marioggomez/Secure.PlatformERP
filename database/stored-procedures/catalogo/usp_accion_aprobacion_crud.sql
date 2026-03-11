CREATE OR ALTER PROCEDURE catalogo.usp_accion_aprobacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_accion_aprobacion], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.accion_aprobacion;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_accion_aprobacion_obtener
    @id_accion_aprobacion smallint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_accion_aprobacion], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.accion_aprobacion
    WHERE [id_accion_aprobacion] = @id_accion_aprobacion;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_accion_aprobacion_crear
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
    INSERT INTO catalogo.accion_aprobacion ([codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc])
    VALUES (@codigo, @nombre, @descripcion, @orden_visual, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS smallint) AS id;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_accion_aprobacion_actualizar
    @id_accion_aprobacion smallint,
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
    UPDATE catalogo.accion_aprobacion
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [orden_visual] = @orden_visual,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_accion_aprobacion] = @id_accion_aprobacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_accion_aprobacion_desactivar
    @id_accion_aprobacion smallint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE catalogo.accion_aprobacion
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_accion_aprobacion] = @id_accion_aprobacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
