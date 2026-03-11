CREATE OR ALTER PROCEDURE plataforma.usp_notificacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_notificacion], [id_usuario], [titulo], [mensaje], [leida], [fecha_creacion]
    FROM plataforma.notificacion;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_notificacion_obtener
    @id_notificacion bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_notificacion], [id_usuario], [titulo], [mensaje], [leida], [fecha_creacion]
    FROM plataforma.notificacion
    WHERE [id_notificacion] = @id_notificacion;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_notificacion_crear
    @id_usuario bigint,
    @titulo varchar(200),
    @mensaje varchar(2000),
    @leida bit,
    @fecha_creacion datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO plataforma.notificacion ([id_usuario], [titulo], [mensaje], [leida], [fecha_creacion])
    VALUES (@id_usuario, @titulo, @mensaje, @leida, @fecha_creacion);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_notificacion_actualizar
    @id_notificacion bigint,
    @id_usuario bigint,
    @titulo varchar(200),
    @mensaje varchar(2000),
    @leida bit,
    @fecha_creacion datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.notificacion
    SET [id_usuario] = @id_usuario,
        [titulo] = @titulo,
        [mensaje] = @mensaje,
        [leida] = @leida,
        [fecha_creacion] = @fecha_creacion
    WHERE [id_notificacion] = @id_notificacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_notificacion_desactivar
    @id_notificacion bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM plataforma.notificacion
    WHERE [id_notificacion] = @id_notificacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
