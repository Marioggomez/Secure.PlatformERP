CREATE OR ALTER PROCEDURE seguridad.usp_dispositivo_usuario_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_dispositivo_usuario], [id_usuario], [fingerprint], [navegador], [sistema_operativo], [ip_ultimo_acceso], [fecha_registro], [fecha_ultimo_acceso]
    FROM seguridad.dispositivo_usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_dispositivo_usuario_obtener
    @id_dispositivo_usuario bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_dispositivo_usuario], [id_usuario], [fingerprint], [navegador], [sistema_operativo], [ip_ultimo_acceso], [fecha_registro], [fecha_ultimo_acceso]
    FROM seguridad.dispositivo_usuario
    WHERE [id_dispositivo_usuario] = @id_dispositivo_usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_dispositivo_usuario_crear
    @id_usuario bigint,
    @fingerprint varchar(200),
    @navegador varchar(200),
    @sistema_operativo varchar(200),
    @ip_ultimo_acceso varchar(50),
    @fecha_registro datetime2,
    @fecha_ultimo_acceso datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.dispositivo_usuario ([id_usuario], [fingerprint], [navegador], [sistema_operativo], [ip_ultimo_acceso], [fecha_registro], [fecha_ultimo_acceso])
    VALUES (@id_usuario, @fingerprint, @navegador, @sistema_operativo, @ip_ultimo_acceso, @fecha_registro, @fecha_ultimo_acceso);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_dispositivo_usuario_actualizar
    @id_dispositivo_usuario bigint,
    @id_usuario bigint,
    @fingerprint varchar(200),
    @navegador varchar(200),
    @sistema_operativo varchar(200),
    @ip_ultimo_acceso varchar(50),
    @fecha_registro datetime2,
    @fecha_ultimo_acceso datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.dispositivo_usuario
    SET [id_usuario] = @id_usuario,
        [fingerprint] = @fingerprint,
        [navegador] = @navegador,
        [sistema_operativo] = @sistema_operativo,
        [ip_ultimo_acceso] = @ip_ultimo_acceso,
        [fecha_registro] = @fecha_registro,
        [fecha_ultimo_acceso] = @fecha_ultimo_acceso
    WHERE [id_dispositivo_usuario] = @id_dispositivo_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_dispositivo_usuario_desactivar
    @id_dispositivo_usuario bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM seguridad.dispositivo_usuario
    WHERE [id_dispositivo_usuario] = @id_dispositivo_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
