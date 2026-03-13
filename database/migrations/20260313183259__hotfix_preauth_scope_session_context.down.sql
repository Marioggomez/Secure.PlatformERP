/*
    Migracion: 20260313183259__hotfix_preauth_scope_session_context
    Autor: Mario Gomez
    Fecha UTC: 2026-03-13 18:32:59
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

/* Rollback: restaurar endurecimiento SESSION_CONTEXT en flujos pre-auth. */

CREATE OR ALTER PROCEDURE seguridad.usp_desafio_mfa_actualizar
    @id_desafio_mfa uniqueidentifier,
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @id_sesion_usuario uniqueidentifier,
    @id_flujo_autenticacion uniqueidentifier,
    @id_proposito_desafio_mfa smallint,
    @id_canal_notificacion smallint,
    @codigo_accion nvarchar(100),
    @otp_hash binary(32),
    @otp_salt varbinary(16),
    @expira_en_utc datetime2,
    @usado bit,
    @intentos smallint,
    @max_intentos smallint,
    @creado_utc datetime2,
    @validado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SET @id_tenant = @ctx_id_tenant;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SET @id_empresa = @ctx_id_empresa;
    UPDATE seguridad.desafio_mfa
    SET [id_usuario] = @id_usuario,
        [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [id_sesion_usuario] = @id_sesion_usuario,
        [id_flujo_autenticacion] = @id_flujo_autenticacion,
        [id_proposito_desafio_mfa] = @id_proposito_desafio_mfa,
        [id_canal_notificacion] = @id_canal_notificacion,
        [codigo_accion] = @codigo_accion,
        [otp_hash] = @otp_hash,
        [otp_salt] = @otp_salt,
        [expira_en_utc] = @expira_en_utc,
        [usado] = @usado,
        [intentos] = @intentos,
        [max_intentos] = @max_intentos,
        [creado_utc] = @creado_utc,
        [validado_utc] = @validado_utc
    WHERE [id_desafio_mfa] = @id_desafio_mfa AND [id_tenant] = @ctx_id_tenant AND [id_empresa] = @ctx_id_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_desafio_mfa_crear
    @id_desafio_mfa uniqueidentifier,
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @id_sesion_usuario uniqueidentifier,
    @id_flujo_autenticacion uniqueidentifier,
    @id_proposito_desafio_mfa smallint,
    @id_canal_notificacion smallint,
    @codigo_accion nvarchar(100),
    @otp_hash binary(32),
    @otp_salt varbinary(16),
    @expira_en_utc datetime2,
    @usado bit,
    @intentos smallint,
    @max_intentos smallint,
    @creado_utc datetime2,
    @validado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SET @id_tenant = @ctx_id_tenant;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SET @id_empresa = @ctx_id_empresa;
    INSERT INTO seguridad.desafio_mfa ([id_desafio_mfa], [id_usuario], [id_tenant], [id_empresa], [id_sesion_usuario], [id_flujo_autenticacion], [id_proposito_desafio_mfa], [id_canal_notificacion], [codigo_accion], [otp_hash], [otp_salt], [expira_en_utc], [usado], [intentos], [max_intentos], [creado_utc], [validado_utc])
    VALUES (@id_desafio_mfa, @id_usuario, @id_tenant, @id_empresa, @id_sesion_usuario, @id_flujo_autenticacion, @id_proposito_desafio_mfa, @id_canal_notificacion, @codigo_accion, @otp_hash, @otp_salt, @expira_en_utc, @usado, @intentos, @max_intentos, @creado_utc, @validado_utc);
    SELECT @id_desafio_mfa AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_desafio_mfa_obtener
    @id_desafio_mfa uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT [id_desafio_mfa], [id_usuario], [id_tenant], [id_empresa], [id_sesion_usuario], [id_flujo_autenticacion], [id_proposito_desafio_mfa], [id_canal_notificacion], [codigo_accion], [otp_hash], [otp_salt], [expira_en_utc], [usado], [intentos], [max_intentos], [creado_utc], [validado_utc]
    FROM seguridad.desafio_mfa
    WHERE [id_desafio_mfa] = @id_desafio_mfa AND [id_tenant] = @ctx_id_tenant AND [id_empresa] = @ctx_id_empresa;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_flujo_autenticacion_actualizar
    @id_flujo_autenticacion uniqueidentifier,
    @id_usuario bigint,
    @id_tenant bigint,
    @mfa_requerido bit,
    @mfa_validado bit,
    @expira_en_utc datetime2,
    @usado bit,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @huella_dispositivo nvarchar(200),
    @solicitud_id nvarchar(64),
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SET @id_tenant = @ctx_id_tenant;
    UPDATE seguridad.flujo_autenticacion
    SET [id_usuario] = @id_usuario,
        [id_tenant] = @id_tenant,
        [mfa_requerido] = @mfa_requerido,
        [mfa_validado] = @mfa_validado,
        [expira_en_utc] = @expira_en_utc,
        [usado] = @usado,
        [ip_origen] = @ip_origen,
        [agente_usuario] = @agente_usuario,
        [huella_dispositivo] = @huella_dispositivo,
        [solicitud_id] = @solicitud_id,
        [creado_utc] = @creado_utc
    WHERE [id_flujo_autenticacion] = @id_flujo_autenticacion AND [id_tenant] = @ctx_id_tenant;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_flujo_autenticacion_crear
    @id_flujo_autenticacion uniqueidentifier,
    @id_usuario bigint,
    @id_tenant bigint,
    @mfa_requerido bit,
    @mfa_validado bit,
    @expira_en_utc datetime2,
    @usado bit,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @huella_dispositivo nvarchar(200),
    @solicitud_id nvarchar(64),
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SET @id_tenant = @ctx_id_tenant;
    INSERT INTO seguridad.flujo_autenticacion ([id_flujo_autenticacion], [id_usuario], [id_tenant], [mfa_requerido], [mfa_validado], [expira_en_utc], [usado], [ip_origen], [agente_usuario], [huella_dispositivo], [solicitud_id], [creado_utc])
    VALUES (@id_flujo_autenticacion, @id_usuario, @id_tenant, @mfa_requerido, @mfa_validado, @expira_en_utc, @usado, @ip_origen, @agente_usuario, @huella_dispositivo, @solicitud_id, @creado_utc);
    SELECT @id_flujo_autenticacion AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_flujo_autenticacion_obtener
    @id_flujo_autenticacion uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SELECT [id_flujo_autenticacion], [id_usuario], [id_tenant], [mfa_requerido], [mfa_validado], [expira_en_utc], [usado], [ip_origen], [agente_usuario], [huella_dispositivo], [solicitud_id], [creado_utc]
    FROM seguridad.flujo_autenticacion
    WHERE [id_flujo_autenticacion] = @id_flujo_autenticacion AND [id_tenant] = @ctx_id_tenant;
END
GO

