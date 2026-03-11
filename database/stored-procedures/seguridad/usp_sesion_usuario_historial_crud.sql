CREATE OR ALTER PROCEDURE seguridad.usp_sesion_usuario_historial_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_historial], [id_sesion], [id_usuario], [fecha_inicio], [fecha_fin], [ip], [dispositivo], [motivo_cierre]
    FROM seguridad.sesion_usuario_historial;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_sesion_usuario_historial_obtener
    @id_historial bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_historial], [id_sesion], [id_usuario], [fecha_inicio], [fecha_fin], [ip], [dispositivo], [motivo_cierre]
    FROM seguridad.sesion_usuario_historial
    WHERE [id_historial] = @id_historial;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_sesion_usuario_historial_crear
    @id_sesion bigint,
    @id_usuario bigint,
    @fecha_inicio datetime2,
    @fecha_fin datetime2,
    @ip varchar(50),
    @dispositivo varchar(200),
    @motivo_cierre varchar(200)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.sesion_usuario_historial ([id_sesion], [id_usuario], [fecha_inicio], [fecha_fin], [ip], [dispositivo], [motivo_cierre])
    VALUES (@id_sesion, @id_usuario, @fecha_inicio, @fecha_fin, @ip, @dispositivo, @motivo_cierre);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_sesion_usuario_historial_actualizar
    @id_historial bigint,
    @id_sesion bigint,
    @id_usuario bigint,
    @fecha_inicio datetime2,
    @fecha_fin datetime2,
    @ip varchar(50),
    @dispositivo varchar(200),
    @motivo_cierre varchar(200)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.sesion_usuario_historial
    SET [id_sesion] = @id_sesion,
        [id_usuario] = @id_usuario,
        [fecha_inicio] = @fecha_inicio,
        [fecha_fin] = @fecha_fin,
        [ip] = @ip,
        [dispositivo] = @dispositivo,
        [motivo_cierre] = @motivo_cierre
    WHERE [id_historial] = @id_historial;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_sesion_usuario_historial_desactivar
    @id_historial bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM seguridad.sesion_usuario_historial
    WHERE [id_historial] = @id_historial;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
