CREATE OR ALTER PROCEDURE seguridad.usp_privilegio_permiso_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_privilegio], [id_permiso], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.privilegio_permiso;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_privilegio_permiso_obtener
    @id_privilegio bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_privilegio], [id_permiso], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.privilegio_permiso
    WHERE [id_privilegio] = @id_privilegio;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_privilegio_permiso_crear
    @id_permiso int,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.privilegio_permiso ([id_permiso], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_permiso, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_privilegio_permiso_actualizar
    @id_privilegio bigint,
    @id_permiso int,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.privilegio_permiso
    SET [id_permiso] = @id_permiso,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_privilegio] = @id_privilegio;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_privilegio_permiso_desactivar
    @id_privilegio bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.privilegio_permiso
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_privilegio] = @id_privilegio;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
