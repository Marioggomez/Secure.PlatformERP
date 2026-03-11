CREATE OR ALTER PROCEDURE seguridad.usp_sesion_usuario_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_sesion_usuario], [id_usuario], [id_tenant], [id_empresa], [token_hash], [refresh_hash], [origen_autenticacion], [mfa_validado], [creado_utc], [expira_absoluta_utc], [ultima_actividad_utc], [ip_origen], [agente_usuario], [huella_dispositivo], [activo], [revocada_utc], [motivo_revocacion], [version_fila]
    FROM seguridad.sesion_usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_sesion_usuario_obtener
    @id_sesion_usuario uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_sesion_usuario], [id_usuario], [id_tenant], [id_empresa], [token_hash], [refresh_hash], [origen_autenticacion], [mfa_validado], [creado_utc], [expira_absoluta_utc], [ultima_actividad_utc], [ip_origen], [agente_usuario], [huella_dispositivo], [activo], [revocada_utc], [motivo_revocacion], [version_fila]
    FROM seguridad.sesion_usuario
    WHERE [id_sesion_usuario] = @id_sesion_usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_sesion_usuario_crear
    @id_sesion_usuario uniqueidentifier,
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @token_hash binary(32),
    @refresh_hash binary(32),
    @origen_autenticacion varchar(20),
    @mfa_validado bit,
    @creado_utc datetime2,
    @expira_absoluta_utc datetime2,
    @ultima_actividad_utc datetime2,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @huella_dispositivo nvarchar(200),
    @activo bit,
    @revocada_utc datetime2,
    @motivo_revocacion nvarchar(200)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.sesion_usuario ([id_sesion_usuario], [id_usuario], [id_tenant], [id_empresa], [token_hash], [refresh_hash], [origen_autenticacion], [mfa_validado], [creado_utc], [expira_absoluta_utc], [ultima_actividad_utc], [ip_origen], [agente_usuario], [huella_dispositivo], [activo], [revocada_utc], [motivo_revocacion])
    VALUES (@id_sesion_usuario, @id_usuario, @id_tenant, @id_empresa, @token_hash, @refresh_hash, @origen_autenticacion, @mfa_validado, @creado_utc, @expira_absoluta_utc, @ultima_actividad_utc, @ip_origen, @agente_usuario, @huella_dispositivo, @activo, @revocada_utc, @motivo_revocacion);
    SELECT @id_sesion_usuario AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_sesion_usuario_actualizar
    @id_sesion_usuario uniqueidentifier,
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @token_hash binary(32),
    @refresh_hash binary(32),
    @origen_autenticacion varchar(20),
    @mfa_validado bit,
    @creado_utc datetime2,
    @expira_absoluta_utc datetime2,
    @ultima_actividad_utc datetime2,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @huella_dispositivo nvarchar(200),
    @activo bit,
    @revocada_utc datetime2,
    @motivo_revocacion nvarchar(200)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.sesion_usuario
    SET [id_usuario] = @id_usuario,
        [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [token_hash] = @token_hash,
        [refresh_hash] = @refresh_hash,
        [origen_autenticacion] = @origen_autenticacion,
        [mfa_validado] = @mfa_validado,
        [creado_utc] = @creado_utc,
        [expira_absoluta_utc] = @expira_absoluta_utc,
        [ultima_actividad_utc] = @ultima_actividad_utc,
        [ip_origen] = @ip_origen,
        [agente_usuario] = @agente_usuario,
        [huella_dispositivo] = @huella_dispositivo,
        [activo] = @activo,
        [revocada_utc] = @revocada_utc,
        [motivo_revocacion] = @motivo_revocacion
    WHERE [id_sesion_usuario] = @id_sesion_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_sesion_usuario_desactivar
    @id_sesion_usuario uniqueidentifier,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.sesion_usuario
    SET [activo] = 0
    WHERE [id_sesion_usuario] = @id_sesion_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
