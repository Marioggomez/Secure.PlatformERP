CREATE OR ALTER PROCEDURE catalogo.usp_modo_filtro_dato_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_modo_filtro_dato], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.modo_filtro_dato;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_modo_filtro_dato_obtener
    @id_modo_filtro_dato smallint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_modo_filtro_dato], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.modo_filtro_dato
    WHERE [id_modo_filtro_dato] = @id_modo_filtro_dato;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_modo_filtro_dato_crear
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
    INSERT INTO catalogo.modo_filtro_dato ([codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc])
    VALUES (@codigo, @nombre, @descripcion, @orden_visual, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS smallint) AS id;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_modo_filtro_dato_actualizar
    @id_modo_filtro_dato smallint,
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
    UPDATE catalogo.modo_filtro_dato
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [orden_visual] = @orden_visual,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_modo_filtro_dato] = @id_modo_filtro_dato;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_modo_filtro_dato_desactivar
    @id_modo_filtro_dato smallint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE catalogo.modo_filtro_dato
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_modo_filtro_dato] = @id_modo_filtro_dato;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
