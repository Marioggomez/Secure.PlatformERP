CREATE OR ALTER PROCEDURE catalogo.usp_efecto_permiso_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_efecto_permiso], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.efecto_permiso;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_efecto_permiso_obtener
    @id_efecto_permiso smallint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_efecto_permiso], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.efecto_permiso
    WHERE [id_efecto_permiso] = @id_efecto_permiso;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_efecto_permiso_crear
    @codigo varchar(30),
    @nombre nvarchar(120),
    @descripcion nvarchar(300),
    @orden_visual smallint,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO catalogo.efecto_permiso ([codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc])
    VALUES (@codigo, @nombre, @descripcion, @orden_visual, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS smallint) AS id;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_efecto_permiso_actualizar
    @id_efecto_permiso smallint,
    @codigo varchar(30),
    @nombre nvarchar(120),
    @descripcion nvarchar(300),
    @orden_visual smallint,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE catalogo.efecto_permiso
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [orden_visual] = @orden_visual,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_efecto_permiso] = @id_efecto_permiso;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_efecto_permiso_desactivar
    @id_efecto_permiso smallint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE catalogo.efecto_permiso
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_efecto_permiso] = @id_efecto_permiso;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
