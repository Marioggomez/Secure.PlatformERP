CREATE OR ALTER PROCEDURE seguridad.usp_desafio_mfa_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_desafio_mfa], [id_usuario], [id_tenant], [id_empresa], [id_sesion_usuario], [id_flujo_autenticacion], [id_proposito_desafio_mfa], [id_canal_notificacion], [codigo_accion], [otp_hash], [otp_salt], [expira_en_utc], [usado], [intentos], [max_intentos], [creado_utc], [validado_utc]
    FROM seguridad.desafio_mfa;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_desafio_mfa_obtener
    @id_desafio_mfa uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_desafio_mfa], [id_usuario], [id_tenant], [id_empresa], [id_sesion_usuario], [id_flujo_autenticacion], [id_proposito_desafio_mfa], [id_canal_notificacion], [codigo_accion], [otp_hash], [otp_salt], [expira_en_utc], [usado], [intentos], [max_intentos], [creado_utc], [validado_utc]
    FROM seguridad.desafio_mfa
    WHERE [id_desafio_mfa] = @id_desafio_mfa;
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
    INSERT INTO seguridad.desafio_mfa ([id_desafio_mfa], [id_usuario], [id_tenant], [id_empresa], [id_sesion_usuario], [id_flujo_autenticacion], [id_proposito_desafio_mfa], [id_canal_notificacion], [codigo_accion], [otp_hash], [otp_salt], [expira_en_utc], [usado], [intentos], [max_intentos], [creado_utc], [validado_utc])
    VALUES (@id_desafio_mfa, @id_usuario, @id_tenant, @id_empresa, @id_sesion_usuario, @id_flujo_autenticacion, @id_proposito_desafio_mfa, @id_canal_notificacion, @codigo_accion, @otp_hash, @otp_salt, @expira_en_utc, @usado, @intentos, @max_intentos, @creado_utc, @validado_utc);
    SELECT @id_desafio_mfa AS id;
END
GO

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
    WHERE [id_desafio_mfa] = @id_desafio_mfa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_desafio_mfa_desactivar
    @id_desafio_mfa uniqueidentifier,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM seguridad.desafio_mfa
    WHERE [id_desafio_mfa] = @id_desafio_mfa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
