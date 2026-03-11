CREATE OR ALTER PROCEDURE dbo.usp_sysdiagrams_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [name], [principal_id], [diagram_id], [version], [definition]
    FROM dbo.sysdiagrams;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_sysdiagrams_obtener
    @diagram_id int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [name], [principal_id], [diagram_id], [version], [definition]
    FROM dbo.sysdiagrams
    WHERE [diagram_id] = @diagram_id;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_sysdiagrams_crear
    @name nvarchar(128),
    @principal_id int,
    @version int,
    @definition varbinary(max)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.sysdiagrams ([name], [principal_id], [version], [definition])
    VALUES (@name, @principal_id, @version, @definition);
    SELECT CAST(SCOPE_IDENTITY() AS int) AS id;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_sysdiagrams_actualizar
    @diagram_id int,
    @name nvarchar(128),
    @principal_id int,
    @version int,
    @definition varbinary(max)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.sysdiagrams
    SET [name] = @name,
        [principal_id] = @principal_id,
        [version] = @version,
        [definition] = @definition
    WHERE [diagram_id] = @diagram_id;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE dbo.usp_sysdiagrams_desactivar
    @diagram_id int,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.sysdiagrams
    WHERE [diagram_id] = @diagram_id;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
