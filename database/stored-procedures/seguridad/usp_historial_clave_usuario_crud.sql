CREATE OR ALTER PROCEDURE seguridad.usp_historial_clave_usuario_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_historial_clave_usuario], [id_usuario], [hash_clave], [salt_clave], [algoritmo_clave], [iteraciones_clave], [creado_utc]
    FROM seguridad.historial_clave_usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_historial_clave_usuario_obtener
    @id_historial_clave_usuario bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_historial_clave_usuario], [id_usuario], [hash_clave], [salt_clave], [algoritmo_clave], [iteraciones_clave], [creado_utc]
    FROM seguridad.historial_clave_usuario
    WHERE [id_historial_clave_usuario] = @id_historial_clave_usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_historial_clave_usuario_crear
    @id_usuario bigint,
    @hash_clave varbinary(128),
    @salt_clave varbinary(32),
    @algoritmo_clave varchar(30),
    @iteraciones_clave int,
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.historial_clave_usuario ([id_usuario], [hash_clave], [salt_clave], [algoritmo_clave], [iteraciones_clave], [creado_utc])
    VALUES (@id_usuario, @hash_clave, @salt_clave, @algoritmo_clave, @iteraciones_clave, @creado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_historial_clave_usuario_actualizar
    @id_historial_clave_usuario bigint,
    @id_usuario bigint,
    @hash_clave varbinary(128),
    @salt_clave varbinary(32),
    @algoritmo_clave varchar(30),
    @iteraciones_clave int,
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.historial_clave_usuario
    SET [id_usuario] = @id_usuario,
        [hash_clave] = @hash_clave,
        [salt_clave] = @salt_clave,
        [algoritmo_clave] = @algoritmo_clave,
        [iteraciones_clave] = @iteraciones_clave,
        [creado_utc] = @creado_utc
    WHERE [id_historial_clave_usuario] = @id_historial_clave_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_historial_clave_usuario_desactivar
    @id_historial_clave_usuario bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM seguridad.historial_clave_usuario
    WHERE [id_historial_clave_usuario] = @id_historial_clave_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
