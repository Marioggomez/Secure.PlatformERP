CREATE OR ALTER PROCEDURE seguridad.usp_operacion_api_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_operacion_api], [codigo], [modulo], [controlador], [accion], [metodo_http], [ruta], [descripcion], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.operacion_api;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_operacion_api_obtener
    @id_operacion_api bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_operacion_api], [codigo], [modulo], [controlador], [accion], [metodo_http], [ruta], [descripcion], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.operacion_api
    WHERE [id_operacion_api] = @id_operacion_api;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_operacion_api_crear
    @codigo nvarchar(150),
    @modulo nvarchar(80),
    @controlador nvarchar(120),
    @accion nvarchar(120),
    @metodo_http nvarchar(10),
    @ruta nvarchar(300),
    @descripcion nvarchar(300),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.operacion_api ([codigo], [modulo], [controlador], [accion], [metodo_http], [ruta], [descripcion], [activo], [creado_utc], [actualizado_utc])
    VALUES (@codigo, @modulo, @controlador, @accion, @metodo_http, @ruta, @descripcion, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_operacion_api_actualizar
    @id_operacion_api bigint,
    @codigo nvarchar(150),
    @modulo nvarchar(80),
    @controlador nvarchar(120),
    @accion nvarchar(120),
    @metodo_http nvarchar(10),
    @ruta nvarchar(300),
    @descripcion nvarchar(300),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.operacion_api
    SET [codigo] = @codigo,
        [modulo] = @modulo,
        [controlador] = @controlador,
        [accion] = @accion,
        [metodo_http] = @metodo_http,
        [ruta] = @ruta,
        [descripcion] = @descripcion,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_operacion_api] = @id_operacion_api;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_operacion_api_desactivar
    @id_operacion_api bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.operacion_api
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_operacion_api] = @id_operacion_api;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
