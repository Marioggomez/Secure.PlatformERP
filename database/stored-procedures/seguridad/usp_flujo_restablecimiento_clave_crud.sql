CREATE OR ALTER PROCEDURE seguridad.usp_flujo_restablecimiento_clave_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_flujo_restablecimiento_clave], [id_usuario], [id_tipo_verificacion_restablecimiento], [verificacion_completada], [expira_en_utc], [usado], [ip_origen], [agente_usuario], [solicitud_id], [creado_utc]
    FROM seguridad.flujo_restablecimiento_clave;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_flujo_restablecimiento_clave_obtener
    @id_flujo_restablecimiento_clave uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_flujo_restablecimiento_clave], [id_usuario], [id_tipo_verificacion_restablecimiento], [verificacion_completada], [expira_en_utc], [usado], [ip_origen], [agente_usuario], [solicitud_id], [creado_utc]
    FROM seguridad.flujo_restablecimiento_clave
    WHERE [id_flujo_restablecimiento_clave] = @id_flujo_restablecimiento_clave;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_flujo_restablecimiento_clave_crear
    @id_flujo_restablecimiento_clave uniqueidentifier,
    @id_usuario bigint,
    @id_tipo_verificacion_restablecimiento smallint,
    @verificacion_completada bit,
    @expira_en_utc datetime2,
    @usado bit,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @solicitud_id nvarchar(64),
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.flujo_restablecimiento_clave ([id_flujo_restablecimiento_clave], [id_usuario], [id_tipo_verificacion_restablecimiento], [verificacion_completada], [expira_en_utc], [usado], [ip_origen], [agente_usuario], [solicitud_id], [creado_utc])
    VALUES (@id_flujo_restablecimiento_clave, @id_usuario, @id_tipo_verificacion_restablecimiento, @verificacion_completada, @expira_en_utc, @usado, @ip_origen, @agente_usuario, @solicitud_id, @creado_utc);
    SELECT @id_flujo_restablecimiento_clave AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_flujo_restablecimiento_clave_actualizar
    @id_flujo_restablecimiento_clave uniqueidentifier,
    @id_usuario bigint,
    @id_tipo_verificacion_restablecimiento smallint,
    @verificacion_completada bit,
    @expira_en_utc datetime2,
    @usado bit,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @solicitud_id nvarchar(64),
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.flujo_restablecimiento_clave
    SET [id_usuario] = @id_usuario,
        [id_tipo_verificacion_restablecimiento] = @id_tipo_verificacion_restablecimiento,
        [verificacion_completada] = @verificacion_completada,
        [expira_en_utc] = @expira_en_utc,
        [usado] = @usado,
        [ip_origen] = @ip_origen,
        [agente_usuario] = @agente_usuario,
        [solicitud_id] = @solicitud_id,
        [creado_utc] = @creado_utc
    WHERE [id_flujo_restablecimiento_clave] = @id_flujo_restablecimiento_clave;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_flujo_restablecimiento_clave_desactivar
    @id_flujo_restablecimiento_clave uniqueidentifier,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM seguridad.flujo_restablecimiento_clave
    WHERE [id_flujo_restablecimiento_clave] = @id_flujo_restablecimiento_clave;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
