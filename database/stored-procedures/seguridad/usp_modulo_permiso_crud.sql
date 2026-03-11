CREATE OR ALTER PROCEDURE seguridad.usp_modulo_permiso_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_modulo_permiso], [id_modulo], [id_permiso]
    FROM seguridad.modulo_permiso;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_modulo_permiso_obtener
    @id_modulo_permiso bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_modulo_permiso], [id_modulo], [id_permiso]
    FROM seguridad.modulo_permiso
    WHERE [id_modulo_permiso] = @id_modulo_permiso;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_modulo_permiso_crear
    @id_modulo int,
    @id_permiso bigint
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.modulo_permiso ([id_modulo], [id_permiso])
    VALUES (@id_modulo, @id_permiso);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_modulo_permiso_actualizar
    @id_modulo_permiso bigint,
    @id_modulo int,
    @id_permiso bigint
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.modulo_permiso
    SET [id_modulo] = @id_modulo,
        [id_permiso] = @id_permiso
    WHERE [id_modulo_permiso] = @id_modulo_permiso;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_modulo_permiso_desactivar
    @id_modulo_permiso bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM seguridad.modulo_permiso
    WHERE [id_modulo_permiso] = @id_modulo_permiso;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
