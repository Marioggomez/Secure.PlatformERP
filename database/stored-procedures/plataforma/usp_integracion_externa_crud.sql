CREATE OR ALTER PROCEDURE plataforma.usp_integracion_externa_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_integracion], [codigo], [nombre], [endpoint], [activo]
    FROM plataforma.integracion_externa;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_integracion_externa_obtener
    @id_integracion bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_integracion], [codigo], [nombre], [endpoint], [activo]
    FROM plataforma.integracion_externa
    WHERE [id_integracion] = @id_integracion;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_integracion_externa_crear
    @codigo varchar(100),
    @nombre varchar(200),
    @endpoint varchar(500),
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO plataforma.integracion_externa ([codigo], [nombre], [endpoint], [activo])
    VALUES (@codigo, @nombre, @endpoint, @activo);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_integracion_externa_actualizar
    @id_integracion bigint,
    @codigo varchar(100),
    @nombre varchar(200),
    @endpoint varchar(500),
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.integracion_externa
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [endpoint] = @endpoint,
        [activo] = @activo
    WHERE [id_integracion] = @id_integracion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_integracion_externa_desactivar
    @id_integracion bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.integracion_externa
    SET [activo] = 0
    WHERE [id_integracion] = @id_integracion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
