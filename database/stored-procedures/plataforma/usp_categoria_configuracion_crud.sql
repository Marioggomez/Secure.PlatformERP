CREATE OR ALTER PROCEDURE plataforma.usp_categoria_configuracion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_categoria_configuracion], [codigo], [nombre], [descripcion]
    FROM plataforma.categoria_configuracion;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_categoria_configuracion_obtener
    @id_categoria_configuracion int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_categoria_configuracion], [codigo], [nombre], [descripcion]
    FROM plataforma.categoria_configuracion
    WHERE [id_categoria_configuracion] = @id_categoria_configuracion;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_categoria_configuracion_crear
    @codigo varchar(50),
    @nombre varchar(200),
    @descripcion varchar(500)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO plataforma.categoria_configuracion ([codigo], [nombre], [descripcion])
    VALUES (@codigo, @nombre, @descripcion);
    SELECT CAST(SCOPE_IDENTITY() AS int) AS id;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_categoria_configuracion_actualizar
    @id_categoria_configuracion int,
    @codigo varchar(50),
    @nombre varchar(200),
    @descripcion varchar(500)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.categoria_configuracion
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion
    WHERE [id_categoria_configuracion] = @id_categoria_configuracion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_categoria_configuracion_desactivar
    @id_categoria_configuracion int,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM plataforma.categoria_configuracion
    WHERE [id_categoria_configuracion] = @id_categoria_configuracion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
