CREATE OR ALTER PROCEDURE observabilidad.usp_operacion_api_log_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_operacion_api_log], [correlation_id], [endpoint], [metodo_http], [usuario], [codigo_http], [duracion_ms], [ip], [fecha], [id_tenant]
    FROM observabilidad.operacion_api_log;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_operacion_api_log_obtener
    @id_operacion_api_log bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_operacion_api_log], [correlation_id], [endpoint], [metodo_http], [usuario], [codigo_http], [duracion_ms], [ip], [fecha], [id_tenant]
    FROM observabilidad.operacion_api_log
    WHERE [id_operacion_api_log] = @id_operacion_api_log;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_operacion_api_log_crear
    @correlation_id uniqueidentifier,
    @endpoint varchar(300),
    @metodo_http varchar(10),
    @usuario varchar(150),
    @codigo_http int,
    @duracion_ms int,
    @ip varchar(50),
    @fecha datetime2,
    @id_tenant bigint
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO observabilidad.operacion_api_log ([correlation_id], [endpoint], [metodo_http], [usuario], [codigo_http], [duracion_ms], [ip], [fecha], [id_tenant])
    VALUES (@correlation_id, @endpoint, @metodo_http, @usuario, @codigo_http, @duracion_ms, @ip, @fecha, @id_tenant);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_operacion_api_log_actualizar
    @id_operacion_api_log bigint,
    @correlation_id uniqueidentifier,
    @endpoint varchar(300),
    @metodo_http varchar(10),
    @usuario varchar(150),
    @codigo_http int,
    @duracion_ms int,
    @ip varchar(50),
    @fecha datetime2,
    @id_tenant bigint
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE observabilidad.operacion_api_log
    SET [correlation_id] = @correlation_id,
        [endpoint] = @endpoint,
        [metodo_http] = @metodo_http,
        [usuario] = @usuario,
        [codigo_http] = @codigo_http,
        [duracion_ms] = @duracion_ms,
        [ip] = @ip,
        [fecha] = @fecha,
        [id_tenant] = @id_tenant
    WHERE [id_operacion_api_log] = @id_operacion_api_log;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_operacion_api_log_desactivar
    @id_operacion_api_log bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM observabilidad.operacion_api_log
    WHERE [id_operacion_api_log] = @id_operacion_api_log;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
