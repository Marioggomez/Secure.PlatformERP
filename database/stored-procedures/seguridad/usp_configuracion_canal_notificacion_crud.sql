CREATE OR ALTER PROCEDURE seguridad.usp_configuracion_canal_notificacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_configuracion_canal_notificacion], [id_tenant], [id_empresa], [id_canal_notificacion], [host], [puerto], [usa_ssl], [usuario_tecnico], [referencia_secreto], [secreto_cifrado], [remitente_correo], [nombre_remitente], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.configuracion_canal_notificacion;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_configuracion_canal_notificacion_obtener
    @id_configuracion_canal_notificacion bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_configuracion_canal_notificacion], [id_tenant], [id_empresa], [id_canal_notificacion], [host], [puerto], [usa_ssl], [usuario_tecnico], [referencia_secreto], [secreto_cifrado], [remitente_correo], [nombre_remitente], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.configuracion_canal_notificacion
    WHERE [id_configuracion_canal_notificacion] = @id_configuracion_canal_notificacion;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_configuracion_canal_notificacion_crear
    @id_tenant bigint,
    @id_empresa bigint,
    @id_canal_notificacion smallint,
    @host nvarchar(200),
    @puerto int,
    @usa_ssl bit,
    @usuario_tecnico nvarchar(200),
    @referencia_secreto nvarchar(300),
    @secreto_cifrado varbinary(max),
    @remitente_correo nvarchar(200),
    @nombre_remitente nvarchar(200),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.configuracion_canal_notificacion ([id_tenant], [id_empresa], [id_canal_notificacion], [host], [puerto], [usa_ssl], [usuario_tecnico], [referencia_secreto], [secreto_cifrado], [remitente_correo], [nombre_remitente], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @id_empresa, @id_canal_notificacion, @host, @puerto, @usa_ssl, @usuario_tecnico, @referencia_secreto, @secreto_cifrado, @remitente_correo, @nombre_remitente, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_configuracion_canal_notificacion_actualizar
    @id_configuracion_canal_notificacion bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @id_canal_notificacion smallint,
    @host nvarchar(200),
    @puerto int,
    @usa_ssl bit,
    @usuario_tecnico nvarchar(200),
    @referencia_secreto nvarchar(300),
    @secreto_cifrado varbinary(max),
    @remitente_correo nvarchar(200),
    @nombre_remitente nvarchar(200),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.configuracion_canal_notificacion
    SET [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [id_canal_notificacion] = @id_canal_notificacion,
        [host] = @host,
        [puerto] = @puerto,
        [usa_ssl] = @usa_ssl,
        [usuario_tecnico] = @usuario_tecnico,
        [referencia_secreto] = @referencia_secreto,
        [secreto_cifrado] = @secreto_cifrado,
        [remitente_correo] = @remitente_correo,
        [nombre_remitente] = @nombre_remitente,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_configuracion_canal_notificacion] = @id_configuracion_canal_notificacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_configuracion_canal_notificacion_desactivar
    @id_configuracion_canal_notificacion bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.configuracion_canal_notificacion
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_configuracion_canal_notificacion] = @id_configuracion_canal_notificacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
