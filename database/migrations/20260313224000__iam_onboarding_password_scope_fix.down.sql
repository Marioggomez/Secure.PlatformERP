/*
    Rollback hotfix IAM onboarding.
*/

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_tenant_crear
    @id_tenant bigint,
    @es_administrador_tenant bit,
    @es_cuenta_servicio bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.usuario_tenant ([id_tenant], [es_administrador_tenant], [es_cuenta_servicio], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @es_administrador_tenant, @es_cuenta_servicio, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_credencial_local_usuario_crear
    @hash_clave varbinary(128),
    @salt_clave varbinary(32),
    @algoritmo_clave varchar(30),
    @iteraciones_clave int,
    @cambio_clave_utc datetime2,
    @debe_cambiar_clave bit,
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.credencial_local_usuario ([hash_clave], [salt_clave], [algoritmo_clave], [iteraciones_clave], [cambio_clave_utc], [debe_cambiar_clave], [activo])
    VALUES (@hash_clave, @salt_clave, @algoritmo_clave, @iteraciones_clave, @cambio_clave_utc, @debe_cambiar_clave, @activo);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO
