CREATE OR ALTER PROCEDURE plataforma.usp_version_sistema_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_version_sistema], [version], [fecha_lanzamiento], [descripcion]
    FROM plataforma.version_sistema;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_version_sistema_obtener
    @id_version_sistema int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_version_sistema], [version], [fecha_lanzamiento], [descripcion]
    FROM plataforma.version_sistema
    WHERE [id_version_sistema] = @id_version_sistema;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_version_sistema_crear
    @version varchar(50),
    @fecha_lanzamiento datetime2,
    @descripcion varchar(500)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO plataforma.version_sistema ([version], [fecha_lanzamiento], [descripcion])
    VALUES (@version, @fecha_lanzamiento, @descripcion);
    SELECT CAST(SCOPE_IDENTITY() AS int) AS id;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_version_sistema_actualizar
    @id_version_sistema int,
    @version varchar(50),
    @fecha_lanzamiento datetime2,
    @descripcion varchar(500)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.version_sistema
    SET [version] = @version,
        [fecha_lanzamiento] = @fecha_lanzamiento,
        [descripcion] = @descripcion
    WHERE [id_version_sistema] = @id_version_sistema;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_version_sistema_desactivar
    @id_version_sistema int,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM plataforma.version_sistema
    WHERE [id_version_sistema] = @id_version_sistema;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
