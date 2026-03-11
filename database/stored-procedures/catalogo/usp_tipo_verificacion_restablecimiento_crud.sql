CREATE OR ALTER PROCEDURE catalogo.usp_tipo_verificacion_restablecimiento_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tipo_verificacion_restablecimiento], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.tipo_verificacion_restablecimiento;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_tipo_verificacion_restablecimiento_obtener
    @id_tipo_verificacion_restablecimiento smallint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tipo_verificacion_restablecimiento], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.tipo_verificacion_restablecimiento
    WHERE [id_tipo_verificacion_restablecimiento] = @id_tipo_verificacion_restablecimiento;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_tipo_verificacion_restablecimiento_crear
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
    INSERT INTO catalogo.tipo_verificacion_restablecimiento ([codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc])
    VALUES (@codigo, @nombre, @descripcion, @orden_visual, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS smallint) AS id;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_tipo_verificacion_restablecimiento_actualizar
    @id_tipo_verificacion_restablecimiento smallint,
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
    UPDATE catalogo.tipo_verificacion_restablecimiento
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [orden_visual] = @orden_visual,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_tipo_verificacion_restablecimiento] = @id_tipo_verificacion_restablecimiento;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_tipo_verificacion_restablecimiento_desactivar
    @id_tipo_verificacion_restablecimiento smallint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE catalogo.tipo_verificacion_restablecimiento
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_tipo_verificacion_restablecimiento] = @id_tipo_verificacion_restablecimiento;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
