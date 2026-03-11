CREATE OR ALTER PROCEDURE seguridad.usp_deber_privilegio_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_deber], [id_privilegio], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.deber_privilegio;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_deber_privilegio_obtener
    @id_deber bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_deber], [id_privilegio], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.deber_privilegio
    WHERE [id_deber] = @id_deber;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_deber_privilegio_crear
    @id_privilegio bigint,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.deber_privilegio ([id_privilegio], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_privilegio, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_deber_privilegio_actualizar
    @id_deber bigint,
    @id_privilegio bigint,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.deber_privilegio
    SET [id_privilegio] = @id_privilegio,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_deber] = @id_deber;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_deber_privilegio_desactivar
    @id_deber bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.deber_privilegio
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_deber] = @id_deber;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
