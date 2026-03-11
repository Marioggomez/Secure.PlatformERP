CREATE OR ALTER PROCEDURE catalogo.usp_severidad_sod_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_severidad_sod], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.severidad_sod;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_severidad_sod_obtener
    @id_severidad_sod smallint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_severidad_sod], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.severidad_sod
    WHERE [id_severidad_sod] = @id_severidad_sod;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_severidad_sod_crear
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
    INSERT INTO catalogo.severidad_sod ([codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc])
    VALUES (@codigo, @nombre, @descripcion, @orden_visual, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS smallint) AS id;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_severidad_sod_actualizar
    @id_severidad_sod smallint,
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
    UPDATE catalogo.severidad_sod
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [orden_visual] = @orden_visual,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_severidad_sod] = @id_severidad_sod;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_severidad_sod_desactivar
    @id_severidad_sod smallint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE catalogo.severidad_sod
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_severidad_sod] = @id_severidad_sod;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
