CREATE OR ALTER PROCEDURE seguridad.usp_credencial_local_usuario_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario], [hash_clave], [salt_clave], [algoritmo_clave], [iteraciones_clave], [cambio_clave_utc], [debe_cambiar_clave], [activo], [version_fila]
    FROM seguridad.credencial_local_usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_credencial_local_usuario_obtener
    @id_usuario bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario], [hash_clave], [salt_clave], [algoritmo_clave], [iteraciones_clave], [cambio_clave_utc], [debe_cambiar_clave], [activo], [version_fila]
    FROM seguridad.credencial_local_usuario
    WHERE [id_usuario] = @id_usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_credencial_local_usuario_crear
    @hash_clave varbinary(128),
    @salt_clave varbinary(32),
    @algoritmo_clave varchar(30),
    @iteraciones_clave int,
    @cambio_clave_utc datetime2,
    @debe_cambiar_clave bit,
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.credencial_local_usuario ([hash_clave], [salt_clave], [algoritmo_clave], [iteraciones_clave], [cambio_clave_utc], [debe_cambiar_clave], [activo])
    VALUES (@hash_clave, @salt_clave, @algoritmo_clave, @iteraciones_clave, @cambio_clave_utc, @debe_cambiar_clave, @activo);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_credencial_local_usuario_actualizar
    @id_usuario bigint,
    @hash_clave varbinary(128),
    @salt_clave varbinary(32),
    @algoritmo_clave varchar(30),
    @iteraciones_clave int,
    @cambio_clave_utc datetime2,
    @debe_cambiar_clave bit,
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.credencial_local_usuario
    SET [hash_clave] = @hash_clave,
        [salt_clave] = @salt_clave,
        [algoritmo_clave] = @algoritmo_clave,
        [iteraciones_clave] = @iteraciones_clave,
        [cambio_clave_utc] = @cambio_clave_utc,
        [debe_cambiar_clave] = @debe_cambiar_clave,
        [activo] = @activo
    WHERE [id_usuario] = @id_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_credencial_local_usuario_desactivar
    @id_usuario bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.credencial_local_usuario
    SET [activo] = 0
    WHERE [id_usuario] = @id_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
