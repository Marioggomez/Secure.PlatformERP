CREATE OR ALTER PROCEDURE catalogo.usp_estado_aprobacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_estado_aprobacion], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.estado_aprobacion;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_estado_aprobacion_obtener
    @id_estado_aprobacion smallint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_estado_aprobacion], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.estado_aprobacion
    WHERE [id_estado_aprobacion] = @id_estado_aprobacion;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_estado_aprobacion_crear
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
    INSERT INTO catalogo.estado_aprobacion ([codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc])
    VALUES (@codigo, @nombre, @descripcion, @orden_visual, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS smallint) AS id;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_estado_aprobacion_actualizar
    @id_estado_aprobacion smallint,
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
    UPDATE catalogo.estado_aprobacion
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [orden_visual] = @orden_visual,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_estado_aprobacion] = @id_estado_aprobacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_estado_aprobacion_desactivar
    @id_estado_aprobacion smallint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE catalogo.estado_aprobacion
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_estado_aprobacion] = @id_estado_aprobacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
