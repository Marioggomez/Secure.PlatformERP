CREATE OR ALTER PROCEDURE seguridad.usp_ip_bloqueada_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_ip_bloqueada], [ip], [motivo], [fecha_bloqueo], [fecha_expiracion]
    FROM seguridad.ip_bloqueada;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_ip_bloqueada_obtener
    @id_ip_bloqueada bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_ip_bloqueada], [ip], [motivo], [fecha_bloqueo], [fecha_expiracion]
    FROM seguridad.ip_bloqueada
    WHERE [id_ip_bloqueada] = @id_ip_bloqueada;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_ip_bloqueada_crear
    @ip varchar(50),
    @motivo varchar(200),
    @fecha_bloqueo datetime2,
    @fecha_expiracion datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.ip_bloqueada ([ip], [motivo], [fecha_bloqueo], [fecha_expiracion])
    VALUES (@ip, @motivo, @fecha_bloqueo, @fecha_expiracion);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_ip_bloqueada_actualizar
    @id_ip_bloqueada bigint,
    @ip varchar(50),
    @motivo varchar(200),
    @fecha_bloqueo datetime2,
    @fecha_expiracion datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.ip_bloqueada
    SET [ip] = @ip,
        [motivo] = @motivo,
        [fecha_bloqueo] = @fecha_bloqueo,
        [fecha_expiracion] = @fecha_expiracion
    WHERE [id_ip_bloqueada] = @id_ip_bloqueada;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_ip_bloqueada_desactivar
    @id_ip_bloqueada bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM seguridad.ip_bloqueada
    WHERE [id_ip_bloqueada] = @id_ip_bloqueada;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
