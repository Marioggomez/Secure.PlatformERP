CREATE OR ALTER PROCEDURE seguridad.usp_usuario_empresa_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario_empresa], [id_usuario], [id_tenant], [id_empresa], [es_empresa_predeterminada], [puede_operar], [fecha_inicio_utc], [fecha_fin_utc], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.usuario_empresa;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_empresa_obtener
    @id_usuario_empresa bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario_empresa], [id_usuario], [id_tenant], [id_empresa], [es_empresa_predeterminada], [puede_operar], [fecha_inicio_utc], [fecha_fin_utc], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.usuario_empresa
    WHERE [id_usuario_empresa] = @id_usuario_empresa;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_empresa_crear
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @es_empresa_predeterminada bit,
    @puede_operar bit,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.usuario_empresa ([id_usuario], [id_tenant], [id_empresa], [es_empresa_predeterminada], [puede_operar], [fecha_inicio_utc], [fecha_fin_utc], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_usuario, @id_tenant, @id_empresa, @es_empresa_predeterminada, @puede_operar, @fecha_inicio_utc, @fecha_fin_utc, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_empresa_actualizar
    @id_usuario_empresa bigint,
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @es_empresa_predeterminada bit,
    @puede_operar bit,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.usuario_empresa
    SET [id_usuario] = @id_usuario,
        [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [es_empresa_predeterminada] = @es_empresa_predeterminada,
        [puede_operar] = @puede_operar,
        [fecha_inicio_utc] = @fecha_inicio_utc,
        [fecha_fin_utc] = @fecha_fin_utc,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_usuario_empresa] = @id_usuario_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_empresa_desactivar
    @id_usuario_empresa bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.usuario_empresa
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_usuario_empresa] = @id_usuario_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
