/*
    Migracion: 20260313194500__hotfix_observabilidad_preauth_session_context
    Autor: Mario Gomez
    Fecha UTC: 2026-03-13 19:45:00
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

/* Rollback: restablecer endurecimiento por SESSION_CONTEXT en operacion API log. */
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

    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL
        THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;

    SET @id_tenant = @ctx_id_tenant;

    INSERT INTO observabilidad.operacion_api_log
    (
        [correlation_id],
        [endpoint],
        [metodo_http],
        [usuario],
        [codigo_http],
        [duracion_ms],
        [ip],
        [fecha],
        [id_tenant]
    )
    VALUES
    (
        @correlation_id,
        @endpoint,
        @metodo_http,
        @usuario,
        @codigo_http,
        @duracion_ms,
        @ip,
        @fecha,
        @id_tenant
    );

    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

