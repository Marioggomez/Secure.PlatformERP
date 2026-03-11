CREATE OR ALTER PROCEDURE seguridad.usp_control_intentos_login_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_control_intento], [login_usuario], [ip], [intentos], [fecha_ultimo_intento], [bloqueado_hasta]
    FROM seguridad.control_intentos_login;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_control_intentos_login_obtener
    @id_control_intento bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_control_intento], [login_usuario], [ip], [intentos], [fecha_ultimo_intento], [bloqueado_hasta]
    FROM seguridad.control_intentos_login
    WHERE [id_control_intento] = @id_control_intento;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_control_intentos_login_crear
    @login_usuario varchar(150),
    @ip varchar(50),
    @intentos int,
    @fecha_ultimo_intento datetime2,
    @bloqueado_hasta datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.control_intentos_login ([login_usuario], [ip], [intentos], [fecha_ultimo_intento], [bloqueado_hasta])
    VALUES (@login_usuario, @ip, @intentos, @fecha_ultimo_intento, @bloqueado_hasta);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_control_intentos_login_actualizar
    @id_control_intento bigint,
    @login_usuario varchar(150),
    @ip varchar(50),
    @intentos int,
    @fecha_ultimo_intento datetime2,
    @bloqueado_hasta datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.control_intentos_login
    SET [login_usuario] = @login_usuario,
        [ip] = @ip,
        [intentos] = @intentos,
        [fecha_ultimo_intento] = @fecha_ultimo_intento,
        [bloqueado_hasta] = @bloqueado_hasta
    WHERE [id_control_intento] = @id_control_intento;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_control_intentos_login_desactivar
    @id_control_intento bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM seguridad.control_intentos_login
    WHERE [id_control_intento] = @id_control_intento;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
