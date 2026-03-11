CREATE OR ALTER PROCEDURE cumplimiento.usp_accion_instancia_aprobacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_accion_instancia_aprobacion], [id_instancia_aprobacion], [id_paso_instancia_aprobacion], [id_usuario], [id_empresa], [id_unidad_organizativa], [id_accion_aprobacion], [comentario], [mfa_validado], [ip_origen], [agente_usuario], [fecha_utc]
    FROM cumplimiento.accion_instancia_aprobacion;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_accion_instancia_aprobacion_obtener
    @id_accion_instancia_aprobacion bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_accion_instancia_aprobacion], [id_instancia_aprobacion], [id_paso_instancia_aprobacion], [id_usuario], [id_empresa], [id_unidad_organizativa], [id_accion_aprobacion], [comentario], [mfa_validado], [ip_origen], [agente_usuario], [fecha_utc]
    FROM cumplimiento.accion_instancia_aprobacion
    WHERE [id_accion_instancia_aprobacion] = @id_accion_instancia_aprobacion;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_accion_instancia_aprobacion_crear
    @id_instancia_aprobacion bigint,
    @id_paso_instancia_aprobacion bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @id_unidad_organizativa bigint,
    @id_accion_aprobacion smallint,
    @comentario nvarchar(500),
    @mfa_validado bit,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @fecha_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO cumplimiento.accion_instancia_aprobacion ([id_instancia_aprobacion], [id_paso_instancia_aprobacion], [id_usuario], [id_empresa], [id_unidad_organizativa], [id_accion_aprobacion], [comentario], [mfa_validado], [ip_origen], [agente_usuario], [fecha_utc])
    VALUES (@id_instancia_aprobacion, @id_paso_instancia_aprobacion, @id_usuario, @id_empresa, @id_unidad_organizativa, @id_accion_aprobacion, @comentario, @mfa_validado, @ip_origen, @agente_usuario, @fecha_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_accion_instancia_aprobacion_actualizar
    @id_accion_instancia_aprobacion bigint,
    @id_instancia_aprobacion bigint,
    @id_paso_instancia_aprobacion bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @id_unidad_organizativa bigint,
    @id_accion_aprobacion smallint,
    @comentario nvarchar(500),
    @mfa_validado bit,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @fecha_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cumplimiento.accion_instancia_aprobacion
    SET [id_instancia_aprobacion] = @id_instancia_aprobacion,
        [id_paso_instancia_aprobacion] = @id_paso_instancia_aprobacion,
        [id_usuario] = @id_usuario,
        [id_empresa] = @id_empresa,
        [id_unidad_organizativa] = @id_unidad_organizativa,
        [id_accion_aprobacion] = @id_accion_aprobacion,
        [comentario] = @comentario,
        [mfa_validado] = @mfa_validado,
        [ip_origen] = @ip_origen,
        [agente_usuario] = @agente_usuario,
        [fecha_utc] = @fecha_utc
    WHERE [id_accion_instancia_aprobacion] = @id_accion_instancia_aprobacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_accion_instancia_aprobacion_desactivar
    @id_accion_instancia_aprobacion bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM cumplimiento.accion_instancia_aprobacion
    WHERE [id_accion_instancia_aprobacion] = @id_accion_instancia_aprobacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
