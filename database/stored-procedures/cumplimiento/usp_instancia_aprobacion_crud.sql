CREATE OR ALTER PROCEDURE cumplimiento.usp_instancia_aprobacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_instancia_aprobacion], [id_tenant], [id_empresa], [id_unidad_organizativa], [id_perfil_aprobacion], [codigo_entidad], [id_objeto], [nivel_actual], [id_estado_aprobacion], [solicitado_por], [solicitado_utc], [expira_utc], [motivo], [hash_payload], [activo]
    FROM cumplimiento.instancia_aprobacion;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_instancia_aprobacion_obtener
    @id_instancia_aprobacion bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_instancia_aprobacion], [id_tenant], [id_empresa], [id_unidad_organizativa], [id_perfil_aprobacion], [codigo_entidad], [id_objeto], [nivel_actual], [id_estado_aprobacion], [solicitado_por], [solicitado_utc], [expira_utc], [motivo], [hash_payload], [activo]
    FROM cumplimiento.instancia_aprobacion
    WHERE [id_instancia_aprobacion] = @id_instancia_aprobacion;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_instancia_aprobacion_crear
    @id_tenant bigint,
    @id_empresa bigint,
    @id_unidad_organizativa bigint,
    @id_perfil_aprobacion bigint,
    @codigo_entidad nvarchar(128),
    @id_objeto bigint,
    @nivel_actual tinyint,
    @id_estado_aprobacion smallint,
    @solicitado_por bigint,
    @solicitado_utc datetime2,
    @expira_utc datetime2,
    @motivo nvarchar(300),
    @hash_payload binary(32),
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO cumplimiento.instancia_aprobacion ([id_tenant], [id_empresa], [id_unidad_organizativa], [id_perfil_aprobacion], [codigo_entidad], [id_objeto], [nivel_actual], [id_estado_aprobacion], [solicitado_por], [solicitado_utc], [expira_utc], [motivo], [hash_payload], [activo])
    VALUES (@id_tenant, @id_empresa, @id_unidad_organizativa, @id_perfil_aprobacion, @codigo_entidad, @id_objeto, @nivel_actual, @id_estado_aprobacion, @solicitado_por, @solicitado_utc, @expira_utc, @motivo, @hash_payload, @activo);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_instancia_aprobacion_actualizar
    @id_instancia_aprobacion bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @id_unidad_organizativa bigint,
    @id_perfil_aprobacion bigint,
    @codigo_entidad nvarchar(128),
    @id_objeto bigint,
    @nivel_actual tinyint,
    @id_estado_aprobacion smallint,
    @solicitado_por bigint,
    @solicitado_utc datetime2,
    @expira_utc datetime2,
    @motivo nvarchar(300),
    @hash_payload binary(32),
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cumplimiento.instancia_aprobacion
    SET [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [id_unidad_organizativa] = @id_unidad_organizativa,
        [id_perfil_aprobacion] = @id_perfil_aprobacion,
        [codigo_entidad] = @codigo_entidad,
        [id_objeto] = @id_objeto,
        [nivel_actual] = @nivel_actual,
        [id_estado_aprobacion] = @id_estado_aprobacion,
        [solicitado_por] = @solicitado_por,
        [solicitado_utc] = @solicitado_utc,
        [expira_utc] = @expira_utc,
        [motivo] = @motivo,
        [hash_payload] = @hash_payload,
        [activo] = @activo
    WHERE [id_instancia_aprobacion] = @id_instancia_aprobacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_instancia_aprobacion_desactivar
    @id_instancia_aprobacion bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cumplimiento.instancia_aprobacion
    SET [activo] = 0
    WHERE [id_instancia_aprobacion] = @id_instancia_aprobacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
