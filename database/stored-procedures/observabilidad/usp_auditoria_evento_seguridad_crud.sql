CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_evento_seguridad_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_auditoria_evento_seguridad], [fecha_utc], [id_tipo_evento_seguridad], [id_tenant], [id_empresa], [id_usuario], [id_sesion_usuario], [detalle], [ip_origen], [agente_usuario], [solicitud_id]
    FROM observabilidad.auditoria_evento_seguridad;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_evento_seguridad_obtener
    @id_auditoria_evento_seguridad bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_auditoria_evento_seguridad], [fecha_utc], [id_tipo_evento_seguridad], [id_tenant], [id_empresa], [id_usuario], [id_sesion_usuario], [detalle], [ip_origen], [agente_usuario], [solicitud_id]
    FROM observabilidad.auditoria_evento_seguridad
    WHERE [id_auditoria_evento_seguridad] = @id_auditoria_evento_seguridad;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_evento_seguridad_crear
    @fecha_utc datetime2,
    @id_tipo_evento_seguridad smallint,
    @id_tenant bigint,
    @id_empresa bigint,
    @id_usuario bigint,
    @id_sesion_usuario uniqueidentifier,
    @detalle nvarchar(500),
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @solicitud_id nvarchar(64)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO observabilidad.auditoria_evento_seguridad ([fecha_utc], [id_tipo_evento_seguridad], [id_tenant], [id_empresa], [id_usuario], [id_sesion_usuario], [detalle], [ip_origen], [agente_usuario], [solicitud_id])
    VALUES (@fecha_utc, @id_tipo_evento_seguridad, @id_tenant, @id_empresa, @id_usuario, @id_sesion_usuario, @detalle, @ip_origen, @agente_usuario, @solicitud_id);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_evento_seguridad_actualizar
    @id_auditoria_evento_seguridad bigint,
    @fecha_utc datetime2,
    @id_tipo_evento_seguridad smallint,
    @id_tenant bigint,
    @id_empresa bigint,
    @id_usuario bigint,
    @id_sesion_usuario uniqueidentifier,
    @detalle nvarchar(500),
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @solicitud_id nvarchar(64)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE observabilidad.auditoria_evento_seguridad
    SET [fecha_utc] = @fecha_utc,
        [id_tipo_evento_seguridad] = @id_tipo_evento_seguridad,
        [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [id_usuario] = @id_usuario,
        [id_sesion_usuario] = @id_sesion_usuario,
        [detalle] = @detalle,
        [ip_origen] = @ip_origen,
        [agente_usuario] = @agente_usuario,
        [solicitud_id] = @solicitud_id
    WHERE [id_auditoria_evento_seguridad] = @id_auditoria_evento_seguridad;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_evento_seguridad_desactivar
    @id_auditoria_evento_seguridad bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM observabilidad.auditoria_evento_seguridad
    WHERE [id_auditoria_evento_seguridad] = @id_auditoria_evento_seguridad;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
