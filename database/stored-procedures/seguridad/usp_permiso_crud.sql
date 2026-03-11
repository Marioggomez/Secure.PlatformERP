CREATE OR ALTER PROCEDURE seguridad.usp_permiso_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_permiso], [codigo], [modulo], [accion], [nombre], [descripcion], [es_sensible], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM seguridad.permiso;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_permiso_obtener
    @id_permiso int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_permiso], [codigo], [modulo], [accion], [nombre], [descripcion], [es_sensible], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM seguridad.permiso
    WHERE [id_permiso] = @id_permiso;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_permiso_crear
    @codigo nvarchar(150),
    @modulo nvarchar(80),
    @accion nvarchar(80),
    @nombre nvarchar(200),
    @descripcion nvarchar(400),
    @es_sensible bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.permiso ([codigo], [modulo], [accion], [nombre], [descripcion], [es_sensible], [activo], [creado_utc], [actualizado_utc])
    VALUES (@codigo, @modulo, @accion, @nombre, @descripcion, @es_sensible, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS int) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_permiso_actualizar
    @id_permiso int,
    @codigo nvarchar(150),
    @modulo nvarchar(80),
    @accion nvarchar(80),
    @nombre nvarchar(200),
    @descripcion nvarchar(400),
    @es_sensible bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.permiso
    SET [codigo] = @codigo,
        [modulo] = @modulo,
        [accion] = @accion,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [es_sensible] = @es_sensible,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_permiso] = @id_permiso;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_permiso_desactivar
    @id_permiso int,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.permiso
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_permiso] = @id_permiso;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
