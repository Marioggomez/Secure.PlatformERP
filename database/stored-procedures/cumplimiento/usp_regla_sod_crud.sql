CREATE OR ALTER PROCEDURE cumplimiento.usp_regla_sod_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_regla_sod], [id_tenant], [id_empresa], [id_permiso_a], [id_permiso_b], [codigo_entidad], [prohibe_mismo_usuario], [prohibe_misma_unidad], [prohibe_misma_sesion], [prohibe_mismo_dia], [id_severidad_sod], [activo], [creado_utc], [actualizado_utc]
    FROM cumplimiento.regla_sod;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_regla_sod_obtener
    @id_regla_sod bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_regla_sod], [id_tenant], [id_empresa], [id_permiso_a], [id_permiso_b], [codigo_entidad], [prohibe_mismo_usuario], [prohibe_misma_unidad], [prohibe_misma_sesion], [prohibe_mismo_dia], [id_severidad_sod], [activo], [creado_utc], [actualizado_utc]
    FROM cumplimiento.regla_sod
    WHERE [id_regla_sod] = @id_regla_sod;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_regla_sod_crear
    @id_tenant bigint,
    @id_empresa bigint,
    @id_permiso_a int,
    @id_permiso_b int,
    @codigo_entidad nvarchar(128),
    @prohibe_mismo_usuario bit,
    @prohibe_misma_unidad bit,
    @prohibe_misma_sesion bit,
    @prohibe_mismo_dia bit,
    @id_severidad_sod smallint,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO cumplimiento.regla_sod ([id_tenant], [id_empresa], [id_permiso_a], [id_permiso_b], [codigo_entidad], [prohibe_mismo_usuario], [prohibe_misma_unidad], [prohibe_misma_sesion], [prohibe_mismo_dia], [id_severidad_sod], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @id_empresa, @id_permiso_a, @id_permiso_b, @codigo_entidad, @prohibe_mismo_usuario, @prohibe_misma_unidad, @prohibe_misma_sesion, @prohibe_mismo_dia, @id_severidad_sod, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_regla_sod_actualizar
    @id_regla_sod bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @id_permiso_a int,
    @id_permiso_b int,
    @codigo_entidad nvarchar(128),
    @prohibe_mismo_usuario bit,
    @prohibe_misma_unidad bit,
    @prohibe_misma_sesion bit,
    @prohibe_mismo_dia bit,
    @id_severidad_sod smallint,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cumplimiento.regla_sod
    SET [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [id_permiso_a] = @id_permiso_a,
        [id_permiso_b] = @id_permiso_b,
        [codigo_entidad] = @codigo_entidad,
        [prohibe_mismo_usuario] = @prohibe_mismo_usuario,
        [prohibe_misma_unidad] = @prohibe_misma_unidad,
        [prohibe_misma_sesion] = @prohibe_misma_sesion,
        [prohibe_mismo_dia] = @prohibe_mismo_dia,
        [id_severidad_sod] = @id_severidad_sod,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_regla_sod] = @id_regla_sod;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_regla_sod_desactivar
    @id_regla_sod bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cumplimiento.regla_sod
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_regla_sod] = @id_regla_sod;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
