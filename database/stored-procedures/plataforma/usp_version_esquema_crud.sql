CREATE OR ALTER PROCEDURE plataforma.usp_version_esquema_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_version_esquema], [componente], [version_codigo], [checksum], [instalado_por], [instalado_utc], [notas]
    FROM plataforma.version_esquema;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_version_esquema_obtener
    @id_version_esquema bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_version_esquema], [componente], [version_codigo], [checksum], [instalado_por], [instalado_utc], [notas]
    FROM plataforma.version_esquema
    WHERE [id_version_esquema] = @id_version_esquema;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_version_esquema_crear
    @componente nvarchar(100),
    @version_codigo nvarchar(50),
    @checksum nvarchar(128),
    @instalado_por nvarchar(100),
    @instalado_utc datetime2,
    @notas nvarchar(500)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO plataforma.version_esquema ([componente], [version_codigo], [checksum], [instalado_por], [instalado_utc], [notas])
    VALUES (@componente, @version_codigo, @checksum, @instalado_por, @instalado_utc, @notas);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_version_esquema_actualizar
    @id_version_esquema bigint,
    @componente nvarchar(100),
    @version_codigo nvarchar(50),
    @checksum nvarchar(128),
    @instalado_por nvarchar(100),
    @instalado_utc datetime2,
    @notas nvarchar(500)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.version_esquema
    SET [componente] = @componente,
        [version_codigo] = @version_codigo,
        [checksum] = @checksum,
        [instalado_por] = @instalado_por,
        [instalado_utc] = @instalado_utc,
        [notas] = @notas
    WHERE [id_version_esquema] = @id_version_esquema;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_version_esquema_desactivar
    @id_version_esquema bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM plataforma.version_esquema
    WHERE [id_version_esquema] = @id_version_esquema;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
