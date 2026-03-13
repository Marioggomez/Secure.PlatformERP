CREATE OR ALTER PROCEDURE seguridad.usp_rol_deber_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_rol], [id_deber], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.rol_deber;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_rol_deber_obtener
    @id_rol bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_rol], [id_deber], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.rol_deber
    WHERE [id_rol] = @id_rol;
END
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
        INSERT INTO seguridad.rol_deber ([id_rol], [id_deber], [activo], [creado_utc], [actualizado_utc])
        VALUES (@id_rol, @id_deber, @activo, @creado_utc, @actualizado_utc);
    END
    SELECT @id_rol AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_rol_deber_actualizar
    @id_rol bigint,
    @id_deber bigint,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.rol_deber
    SET [id_deber] = @id_deber,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_rol] = @id_rol;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_rol_deber_desactivar
    @id_rol bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.rol_deber
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_rol] = @id_rol;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
