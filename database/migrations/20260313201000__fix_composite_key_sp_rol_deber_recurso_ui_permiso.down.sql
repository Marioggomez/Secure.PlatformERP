SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE seguridad.usp_rol_deber_crear
    @id_deber bigint,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO seguridad.rol_deber (id_deber, activo, creado_utc, actualizado_utc)
    VALUES (@id_deber, @activo, @creado_utc, @actualizado_utc);

    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_recurso_ui_permiso_crear
    @id_permiso int,
    @activo bit,
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO seguridad.recurso_ui_permiso (id_permiso, activo, creado_utc)
    VALUES (@id_permiso, @activo, @creado_utc);

    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_recurso_ui_permiso_actualizar
    @id_recurso_ui bigint,
    @id_permiso int,
    @activo bit,
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE seguridad.recurso_ui_permiso
    SET id_permiso = @id_permiso,
        activo = @activo,
        creado_utc = @creado_utc
    WHERE id_recurso_ui = @id_recurso_ui;

    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
