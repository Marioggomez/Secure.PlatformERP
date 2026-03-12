CREATE OR ALTER PROCEDURE tercero.usp_tercero_rol_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tercero_rol], [id_tercero], [id_rol_tercero], [id_empresa], [activo]
    FROM tercero.tercero_rol;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tercero_rol_obtener
    @id_tercero_rol bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tercero_rol], [id_tercero], [id_rol_tercero], [id_empresa], [activo]
    FROM tercero.tercero_rol
    WHERE [id_tercero_rol] = @id_tercero_rol;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tercero_rol_crear
    @id_tercero bigint,
    @id_rol_tercero int,
    @id_empresa bigint,
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tercero.tercero_rol ([id_tercero], [id_rol_tercero], [id_empresa], [activo])
    VALUES (@id_tercero, @id_rol_tercero, @id_empresa, @activo);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tercero_rol_actualizar
    @id_tercero_rol bigint,
    @id_tercero bigint,
    @id_rol_tercero int,
    @id_empresa bigint,
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tercero.tercero_rol
    SET [id_tercero] = @id_tercero,
        [id_rol_tercero] = @id_rol_tercero,
        [id_empresa] = @id_empresa,
        [activo] = @activo
    WHERE [id_tercero_rol] = @id_tercero_rol;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tercero_rol_desactivar
    @id_tercero_rol bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tercero.tercero_rol
    SET [activo] = 0
    WHERE [id_tercero_rol] = @id_tercero_rol;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
