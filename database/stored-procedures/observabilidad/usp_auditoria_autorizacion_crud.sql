CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_autorizacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_auditoria_autorizacion], [fecha_utc], [id_tenant], [id_usuario], [id_empresa], [id_sesion_usuario], [codigo_permiso], [codigo_operacion], [metodo_http], [permitido], [motivo], [codigo_entidad], [id_objeto], [ip_origen], [agente_usuario], [solicitud_id]
    FROM observabilidad.auditoria_autorizacion;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_autorizacion_obtener
    @id_auditoria_autorizacion bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_auditoria_autorizacion], [fecha_utc], [id_tenant], [id_usuario], [id_empresa], [id_sesion_usuario], [codigo_permiso], [codigo_operacion], [metodo_http], [permitido], [motivo], [codigo_entidad], [id_objeto], [ip_origen], [agente_usuario], [solicitud_id]
    FROM observabilidad.auditoria_autorizacion
    WHERE [id_auditoria_autorizacion] = @id_auditoria_autorizacion;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_autorizacion_crear
    @fecha_utc datetime2,
    @id_tenant bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @id_sesion_usuario uniqueidentifier,
    @codigo_permiso nvarchar(150),
    @codigo_operacion nvarchar(150),
    @metodo_http nvarchar(10),
    @permitido bit,
    @motivo nvarchar(200),
    @codigo_entidad nvarchar(128),
    @id_objeto bigint,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @solicitud_id nvarchar(64)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO observabilidad.auditoria_autorizacion ([fecha_utc], [id_tenant], [id_usuario], [id_empresa], [id_sesion_usuario], [codigo_permiso], [codigo_operacion], [metodo_http], [permitido], [motivo], [codigo_entidad], [id_objeto], [ip_origen], [agente_usuario], [solicitud_id])
    VALUES (@fecha_utc, @id_tenant, @id_usuario, @id_empresa, @id_sesion_usuario, @codigo_permiso, @codigo_operacion, @metodo_http, @permitido, @motivo, @codigo_entidad, @id_objeto, @ip_origen, @agente_usuario, @solicitud_id);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_autorizacion_actualizar
    @id_auditoria_autorizacion bigint,
    @fecha_utc datetime2,
    @id_tenant bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @id_sesion_usuario uniqueidentifier,
    @codigo_permiso nvarchar(150),
    @codigo_operacion nvarchar(150),
    @metodo_http nvarchar(10),
    @permitido bit,
    @motivo nvarchar(200),
    @codigo_entidad nvarchar(128),
    @id_objeto bigint,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @solicitud_id nvarchar(64)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE observabilidad.auditoria_autorizacion
    SET [fecha_utc] = @fecha_utc,
        [id_tenant] = @id_tenant,
        [id_usuario] = @id_usuario,
        [id_empresa] = @id_empresa,
        [id_sesion_usuario] = @id_sesion_usuario,
        [codigo_permiso] = @codigo_permiso,
        [codigo_operacion] = @codigo_operacion,
        [metodo_http] = @metodo_http,
        [permitido] = @permitido,
        [motivo] = @motivo,
        [codigo_entidad] = @codigo_entidad,
        [id_objeto] = @id_objeto,
        [ip_origen] = @ip_origen,
        [agente_usuario] = @agente_usuario,
        [solicitud_id] = @solicitud_id
    WHERE [id_auditoria_autorizacion] = @id_auditoria_autorizacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_autorizacion_desactivar
    @id_auditoria_autorizacion bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM observabilidad.auditoria_autorizacion
    WHERE [id_auditoria_autorizacion] = @id_auditoria_autorizacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
