SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

CREATE OR ALTER PROCEDURE seguridad.usp_rol_deber_crear
    @id_rol bigint,
    @id_deber bigint,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM seguridad.rol_deber
        WHERE id_rol = @id_rol
          AND id_deber = @id_deber
    )
    BEGIN
        UPDATE seguridad.rol_deber
        SET activo = @activo,
            actualizado_utc = @actualizado_utc
        WHERE id_rol = @id_rol
          AND id_deber = @id_deber;
    END
    ELSE
    BEGIN
        INSERT INTO seguridad.rol_deber (id_rol, id_deber, activo, creado_utc, actualizado_utc)
        VALUES (@id_rol, @id_deber, @activo, @creado_utc, @actualizado_utc);
    END

    SELECT @id_rol AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_recurso_ui_permiso_crear
    @id_recurso_ui bigint,
    @id_permiso int,
    @activo bit,
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM seguridad.recurso_ui_permiso
        WHERE id_recurso_ui = @id_recurso_ui
          AND id_permiso = @id_permiso
    )
    BEGIN
        UPDATE seguridad.recurso_ui_permiso
        SET activo = @activo
        WHERE id_recurso_ui = @id_recurso_ui
          AND id_permiso = @id_permiso;
    END
    ELSE
    BEGIN
        INSERT INTO seguridad.recurso_ui_permiso (id_recurso_ui, id_permiso, activo, creado_utc)
        VALUES (@id_recurso_ui, @id_permiso, @activo, @creado_utc);
    END

    SELECT @id_recurso_ui AS id;
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
    SET activo = @activo
    WHERE id_recurso_ui = @id_recurso_ui
      AND id_permiso = @id_permiso;

    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
