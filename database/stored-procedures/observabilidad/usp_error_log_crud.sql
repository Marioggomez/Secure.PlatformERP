CREATE OR ALTER PROCEDURE observabilidad.usp_error_log_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_error_log], [correlation_id], [usuario], [endpoint], [mensaje_error], [stacktrace], [payload], [fecha]
    FROM observabilidad.error_log;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_error_log_obtener
    @id_error_log bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_error_log], [correlation_id], [usuario], [endpoint], [mensaje_error], [stacktrace], [payload], [fecha]
    FROM observabilidad.error_log
    WHERE [id_error_log] = @id_error_log;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_error_log_crear
    @correlation_id uniqueidentifier,
    @usuario varchar(150),
    @endpoint varchar(300),
    @mensaje_error varchar(max),
    @stacktrace varchar(max),
    @payload varchar(max),
    @fecha datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO observabilidad.error_log ([correlation_id], [usuario], [endpoint], [mensaje_error], [stacktrace], [payload], [fecha])
    VALUES (@correlation_id, @usuario, @endpoint, @mensaje_error, @stacktrace, @payload, @fecha);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_error_log_actualizar
    @id_error_log bigint,
    @correlation_id uniqueidentifier,
    @usuario varchar(150),
    @endpoint varchar(300),
    @mensaje_error varchar(max),
    @stacktrace varchar(max),
    @payload varchar(max),
    @fecha datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE observabilidad.error_log
    SET [correlation_id] = @correlation_id,
        [usuario] = @usuario,
        [endpoint] = @endpoint,
        [mensaje_error] = @mensaje_error,
        [stacktrace] = @stacktrace,
        [payload] = @payload,
        [fecha] = @fecha
    WHERE [id_error_log] = @id_error_log;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_error_log_desactivar
    @id_error_log bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM observabilidad.error_log
    WHERE [id_error_log] = @id_error_log;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
