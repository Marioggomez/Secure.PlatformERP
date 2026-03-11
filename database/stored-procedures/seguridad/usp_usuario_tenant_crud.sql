CREATE OR ALTER PROCEDURE seguridad.usp_usuario_tenant_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario], [id_tenant], [es_administrador_tenant], [es_cuenta_servicio], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.usuario_tenant;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_tenant_obtener
    @id_usuario bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario], [id_tenant], [es_administrador_tenant], [es_cuenta_servicio], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.usuario_tenant
    WHERE [id_usuario] = @id_usuario;
END
GO

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

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_tenant_actualizar
    @id_usuario bigint,
    @id_tenant bigint,
    @es_administrador_tenant bit,
    @es_cuenta_servicio bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.usuario_tenant
    SET [id_tenant] = @id_tenant,
        [es_administrador_tenant] = @es_administrador_tenant,
        [es_cuenta_servicio] = @es_cuenta_servicio,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_usuario] = @id_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_tenant_desactivar
    @id_usuario bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.usuario_tenant
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_usuario] = @id_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
