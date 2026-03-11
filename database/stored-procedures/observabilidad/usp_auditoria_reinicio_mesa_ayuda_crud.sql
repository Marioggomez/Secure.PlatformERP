CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_reinicio_mesa_ayuda_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_auditoria_reinicio_mesa_ayuda], [id_tenant], [id_empresa], [id_usuario_afectado], [id_usuario_administrador], [motivo], [ip_origen], [agente_usuario], [fecha_utc]
    FROM observabilidad.auditoria_reinicio_mesa_ayuda;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_reinicio_mesa_ayuda_obtener
    @id_auditoria_reinicio_mesa_ayuda bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_auditoria_reinicio_mesa_ayuda], [id_tenant], [id_empresa], [id_usuario_afectado], [id_usuario_administrador], [motivo], [ip_origen], [agente_usuario], [fecha_utc]
    FROM observabilidad.auditoria_reinicio_mesa_ayuda
    WHERE [id_auditoria_reinicio_mesa_ayuda] = @id_auditoria_reinicio_mesa_ayuda;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_reinicio_mesa_ayuda_crear
    @id_tenant bigint,
    @id_empresa bigint,
    @id_usuario_afectado bigint,
    @id_usuario_administrador bigint,
    @motivo nvarchar(300),
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @fecha_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO observabilidad.auditoria_reinicio_mesa_ayuda ([id_tenant], [id_empresa], [id_usuario_afectado], [id_usuario_administrador], [motivo], [ip_origen], [agente_usuario], [fecha_utc])
    VALUES (@id_tenant, @id_empresa, @id_usuario_afectado, @id_usuario_administrador, @motivo, @ip_origen, @agente_usuario, @fecha_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_reinicio_mesa_ayuda_actualizar
    @id_auditoria_reinicio_mesa_ayuda bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @id_usuario_afectado bigint,
    @id_usuario_administrador bigint,
    @motivo nvarchar(300),
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @fecha_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE observabilidad.auditoria_reinicio_mesa_ayuda
    SET [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [id_usuario_afectado] = @id_usuario_afectado,
        [id_usuario_administrador] = @id_usuario_administrador,
        [motivo] = @motivo,
        [ip_origen] = @ip_origen,
        [agente_usuario] = @agente_usuario,
        [fecha_utc] = @fecha_utc
    WHERE [id_auditoria_reinicio_mesa_ayuda] = @id_auditoria_reinicio_mesa_ayuda;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_reinicio_mesa_ayuda_desactivar
    @id_auditoria_reinicio_mesa_ayuda bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM observabilidad.auditoria_reinicio_mesa_ayuda
    WHERE [id_auditoria_reinicio_mesa_ayuda] = @id_auditoria_reinicio_mesa_ayuda;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
