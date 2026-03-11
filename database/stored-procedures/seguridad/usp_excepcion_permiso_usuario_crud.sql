CREATE OR ALTER PROCEDURE seguridad.usp_excepcion_permiso_usuario_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_excepcion_permiso_usuario], [id_usuario], [id_tenant], [id_permiso], [id_efecto_permiso], [id_alcance_asignacion], [id_grupo_empresarial], [id_empresa], [id_unidad_organizativa], [fecha_inicio_utc], [fecha_fin_utc], [concedido_por], [motivo], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.excepcion_permiso_usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_excepcion_permiso_usuario_obtener
    @id_excepcion_permiso_usuario bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_excepcion_permiso_usuario], [id_usuario], [id_tenant], [id_permiso], [id_efecto_permiso], [id_alcance_asignacion], [id_grupo_empresarial], [id_empresa], [id_unidad_organizativa], [fecha_inicio_utc], [fecha_fin_utc], [concedido_por], [motivo], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.excepcion_permiso_usuario
    WHERE [id_excepcion_permiso_usuario] = @id_excepcion_permiso_usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_excepcion_permiso_usuario_crear
    @id_usuario bigint,
    @id_tenant bigint,
    @id_permiso int,
    @id_efecto_permiso smallint,
    @id_alcance_asignacion smallint,
    @id_grupo_empresarial bigint,
    @id_empresa bigint,
    @id_unidad_organizativa bigint,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @concedido_por bigint,
    @motivo nvarchar(300),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.excepcion_permiso_usuario ([id_usuario], [id_tenant], [id_permiso], [id_efecto_permiso], [id_alcance_asignacion], [id_grupo_empresarial], [id_empresa], [id_unidad_organizativa], [fecha_inicio_utc], [fecha_fin_utc], [concedido_por], [motivo], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_usuario, @id_tenant, @id_permiso, @id_efecto_permiso, @id_alcance_asignacion, @id_grupo_empresarial, @id_empresa, @id_unidad_organizativa, @fecha_inicio_utc, @fecha_fin_utc, @concedido_por, @motivo, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_excepcion_permiso_usuario_actualizar
    @id_excepcion_permiso_usuario bigint,
    @id_usuario bigint,
    @id_tenant bigint,
    @id_permiso int,
    @id_efecto_permiso smallint,
    @id_alcance_asignacion smallint,
    @id_grupo_empresarial bigint,
    @id_empresa bigint,
    @id_unidad_organizativa bigint,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @concedido_por bigint,
    @motivo nvarchar(300),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.excepcion_permiso_usuario
    SET [id_usuario] = @id_usuario,
        [id_tenant] = @id_tenant,
        [id_permiso] = @id_permiso,
        [id_efecto_permiso] = @id_efecto_permiso,
        [id_alcance_asignacion] = @id_alcance_asignacion,
        [id_grupo_empresarial] = @id_grupo_empresarial,
        [id_empresa] = @id_empresa,
        [id_unidad_organizativa] = @id_unidad_organizativa,
        [fecha_inicio_utc] = @fecha_inicio_utc,
        [fecha_fin_utc] = @fecha_fin_utc,
        [concedido_por] = @concedido_por,
        [motivo] = @motivo,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_excepcion_permiso_usuario] = @id_excepcion_permiso_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_excepcion_permiso_usuario_desactivar
    @id_excepcion_permiso_usuario bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.excepcion_permiso_usuario
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_excepcion_permiso_usuario] = @id_excepcion_permiso_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
