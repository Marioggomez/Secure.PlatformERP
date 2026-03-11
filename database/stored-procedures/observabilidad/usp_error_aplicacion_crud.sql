CREATE OR ALTER PROCEDURE observabilidad.usp_error_aplicacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_error_aplicacion], [fecha_utc], [id_tenant], [id_usuario], [id_empresa], [id_sesion_usuario], [solicitud_id], [endpoint], [metodo_http], [query_string], [ip_origen], [agente_usuario], [tipo_error], [mensaje_error], [traza_error], [mensaje_interno], [traza_interna], [origen_error], [codigo_http]
    FROM observabilidad.error_aplicacion;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_error_aplicacion_obtener
    @id_error_aplicacion bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_error_aplicacion], [fecha_utc], [id_tenant], [id_usuario], [id_empresa], [id_sesion_usuario], [solicitud_id], [endpoint], [metodo_http], [query_string], [ip_origen], [agente_usuario], [tipo_error], [mensaje_error], [traza_error], [mensaje_interno], [traza_interna], [origen_error], [codigo_http]
    FROM observabilidad.error_aplicacion
    WHERE [id_error_aplicacion] = @id_error_aplicacion;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_error_aplicacion_crear
    @fecha_utc datetime2,
    @id_tenant bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @id_sesion_usuario uniqueidentifier,
    @solicitud_id nvarchar(64),
    @endpoint nvarchar(200),
    @metodo_http nvarchar(10),
    @query_string nvarchar(2000),
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @tipo_error nvarchar(200),
    @mensaje_error nvarchar(max),
    @traza_error nvarchar(max),
    @mensaje_interno nvarchar(max),
    @traza_interna nvarchar(max),
    @origen_error nvarchar(200),
    @codigo_http int
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO observabilidad.error_aplicacion ([fecha_utc], [id_tenant], [id_usuario], [id_empresa], [id_sesion_usuario], [solicitud_id], [endpoint], [metodo_http], [query_string], [ip_origen], [agente_usuario], [tipo_error], [mensaje_error], [traza_error], [mensaje_interno], [traza_interna], [origen_error], [codigo_http])
    VALUES (@fecha_utc, @id_tenant, @id_usuario, @id_empresa, @id_sesion_usuario, @solicitud_id, @endpoint, @metodo_http, @query_string, @ip_origen, @agente_usuario, @tipo_error, @mensaje_error, @traza_error, @mensaje_interno, @traza_interna, @origen_error, @codigo_http);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_error_aplicacion_actualizar
    @id_error_aplicacion bigint,
    @fecha_utc datetime2,
    @id_tenant bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @id_sesion_usuario uniqueidentifier,
    @solicitud_id nvarchar(64),
    @endpoint nvarchar(200),
    @metodo_http nvarchar(10),
    @query_string nvarchar(2000),
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @tipo_error nvarchar(200),
    @mensaje_error nvarchar(max),
    @traza_error nvarchar(max),
    @mensaje_interno nvarchar(max),
    @traza_interna nvarchar(max),
    @origen_error nvarchar(200),
    @codigo_http int
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE observabilidad.error_aplicacion
    SET [fecha_utc] = @fecha_utc,
        [id_tenant] = @id_tenant,
        [id_usuario] = @id_usuario,
        [id_empresa] = @id_empresa,
        [id_sesion_usuario] = @id_sesion_usuario,
        [solicitud_id] = @solicitud_id,
        [endpoint] = @endpoint,
        [metodo_http] = @metodo_http,
        [query_string] = @query_string,
        [ip_origen] = @ip_origen,
        [agente_usuario] = @agente_usuario,
        [tipo_error] = @tipo_error,
        [mensaje_error] = @mensaje_error,
        [traza_error] = @traza_error,
        [mensaje_interno] = @mensaje_interno,
        [traza_interna] = @traza_interna,
        [origen_error] = @origen_error,
        [codigo_http] = @codigo_http
    WHERE [id_error_aplicacion] = @id_error_aplicacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_error_aplicacion_desactivar
    @id_error_aplicacion bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM observabilidad.error_aplicacion
    WHERE [id_error_aplicacion] = @id_error_aplicacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
