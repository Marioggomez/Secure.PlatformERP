CREATE OR ALTER PROCEDURE seguridad.usp_flujo_autenticacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_flujo_autenticacion], [id_usuario], [id_tenant], [mfa_requerido], [mfa_validado], [expira_en_utc], [usado], [ip_origen], [agente_usuario], [huella_dispositivo], [solicitud_id], [creado_utc]
    FROM seguridad.flujo_autenticacion;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_flujo_autenticacion_obtener
    @id_flujo_autenticacion uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_flujo_autenticacion], [id_usuario], [id_tenant], [mfa_requerido], [mfa_validado], [expira_en_utc], [usado], [ip_origen], [agente_usuario], [huella_dispositivo], [solicitud_id], [creado_utc]
    FROM seguridad.flujo_autenticacion
    WHERE [id_flujo_autenticacion] = @id_flujo_autenticacion;
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
    INSERT INTO seguridad.flujo_autenticacion ([id_flujo_autenticacion], [id_usuario], [id_tenant], [mfa_requerido], [mfa_validado], [expira_en_utc], [usado], [ip_origen], [agente_usuario], [huella_dispositivo], [solicitud_id], [creado_utc])
    VALUES (@id_flujo_autenticacion, @id_usuario, @id_tenant, @mfa_requerido, @mfa_validado, @expira_en_utc, @usado, @ip_origen, @agente_usuario, @huella_dispositivo, @solicitud_id, @creado_utc);
    SELECT @id_flujo_autenticacion AS id;
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
    WHERE [id_flujo_autenticacion] = @id_flujo_autenticacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_flujo_autenticacion_desactivar
    @id_flujo_autenticacion uniqueidentifier,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM seguridad.flujo_autenticacion
    WHERE [id_flujo_autenticacion] = @id_flujo_autenticacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
