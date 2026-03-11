CREATE OR ALTER PROCEDURE seguridad.usp_token_restablecimiento_clave_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_token_restablecimiento_clave], [id_usuario], [id_flujo_restablecimiento_clave], [token_hash], [expira_en_utc], [usado], [fecha_uso_utc], [ip_origen], [agente_usuario], [solicitud_id], [creado_utc]
    FROM seguridad.token_restablecimiento_clave;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_token_restablecimiento_clave_obtener
    @id_token_restablecimiento_clave uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_token_restablecimiento_clave], [id_usuario], [id_flujo_restablecimiento_clave], [token_hash], [expira_en_utc], [usado], [fecha_uso_utc], [ip_origen], [agente_usuario], [solicitud_id], [creado_utc]
    FROM seguridad.token_restablecimiento_clave
    WHERE [id_token_restablecimiento_clave] = @id_token_restablecimiento_clave;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_token_restablecimiento_clave_crear
    @id_token_restablecimiento_clave uniqueidentifier,
    @id_usuario bigint,
    @id_flujo_restablecimiento_clave uniqueidentifier,
    @token_hash binary(32),
    @expira_en_utc datetime2,
    @usado bit,
    @fecha_uso_utc datetime2,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @solicitud_id nvarchar(64),
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.token_restablecimiento_clave ([id_token_restablecimiento_clave], [id_usuario], [id_flujo_restablecimiento_clave], [token_hash], [expira_en_utc], [usado], [fecha_uso_utc], [ip_origen], [agente_usuario], [solicitud_id], [creado_utc])
    VALUES (@id_token_restablecimiento_clave, @id_usuario, @id_flujo_restablecimiento_clave, @token_hash, @expira_en_utc, @usado, @fecha_uso_utc, @ip_origen, @agente_usuario, @solicitud_id, @creado_utc);
    SELECT @id_token_restablecimiento_clave AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_token_restablecimiento_clave_actualizar
    @id_token_restablecimiento_clave uniqueidentifier,
    @id_usuario bigint,
    @id_flujo_restablecimiento_clave uniqueidentifier,
    @token_hash binary(32),
    @expira_en_utc datetime2,
    @usado bit,
    @fecha_uso_utc datetime2,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @solicitud_id nvarchar(64),
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.token_restablecimiento_clave
    SET [id_usuario] = @id_usuario,
        [id_flujo_restablecimiento_clave] = @id_flujo_restablecimiento_clave,
        [token_hash] = @token_hash,
        [expira_en_utc] = @expira_en_utc,
        [usado] = @usado,
        [fecha_uso_utc] = @fecha_uso_utc,
        [ip_origen] = @ip_origen,
        [agente_usuario] = @agente_usuario,
        [solicitud_id] = @solicitud_id,
        [creado_utc] = @creado_utc
    WHERE [id_token_restablecimiento_clave] = @id_token_restablecimiento_clave;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_token_restablecimiento_clave_desactivar
    @id_token_restablecimiento_clave uniqueidentifier,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM seguridad.token_restablecimiento_clave
    WHERE [id_token_restablecimiento_clave] = @id_token_restablecimiento_clave;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
