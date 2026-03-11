CREATE OR ALTER PROCEDURE plataforma.usp_configuracion_empresa_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_configuracion_empresa], [id_empresa], [id_parametro_configuracion], [valor], [fecha_creacion]
    FROM plataforma.configuracion_empresa;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_configuracion_empresa_obtener
    @id_configuracion_empresa bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_configuracion_empresa], [id_empresa], [id_parametro_configuracion], [valor], [fecha_creacion]
    FROM plataforma.configuracion_empresa
    WHERE [id_configuracion_empresa] = @id_configuracion_empresa;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_configuracion_empresa_crear
    @id_empresa bigint,
    @id_parametro_configuracion int,
    @valor varchar(500),
    @fecha_creacion datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO plataforma.configuracion_empresa ([id_empresa], [id_parametro_configuracion], [valor], [fecha_creacion])
    VALUES (@id_empresa, @id_parametro_configuracion, @valor, @fecha_creacion);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_configuracion_empresa_actualizar
    @id_configuracion_empresa bigint,
    @id_empresa bigint,
    @id_parametro_configuracion int,
    @valor varchar(500),
    @fecha_creacion datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.configuracion_empresa
    SET [id_empresa] = @id_empresa,
        [id_parametro_configuracion] = @id_parametro_configuracion,
        [valor] = @valor,
        [fecha_creacion] = @fecha_creacion
    WHERE [id_configuracion_empresa] = @id_configuracion_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_configuracion_empresa_desactivar
    @id_configuracion_empresa bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM plataforma.configuracion_empresa
    WHERE [id_configuracion_empresa] = @id_configuracion_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
