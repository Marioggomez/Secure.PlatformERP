CREATE OR ALTER PROCEDURE plataforma.usp_modulo_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_modulo], [codigo], [nombre], [descripcion], [orden]
    FROM plataforma.modulo;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_modulo_obtener
    @id_modulo int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_modulo], [codigo], [nombre], [descripcion], [orden]
    FROM plataforma.modulo
    WHERE [id_modulo] = @id_modulo;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_modulo_crear
    @codigo varchar(50),
    @nombre varchar(100),
    @descripcion varchar(200),
    @orden int
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO plataforma.modulo ([codigo], [nombre], [descripcion], [orden])
    VALUES (@codigo, @nombre, @descripcion, @orden);
    SELECT CAST(SCOPE_IDENTITY() AS int) AS id;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_modulo_actualizar
    @id_modulo int,
    @codigo varchar(50),
    @nombre varchar(100),
    @descripcion varchar(200),
    @orden int
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.modulo
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [orden] = @orden
    WHERE [id_modulo] = @id_modulo;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_modulo_desactivar
    @id_modulo int,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM plataforma.modulo
    WHERE [id_modulo] = @id_modulo;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
