CREATE OR ALTER PROCEDURE cumplimiento.usp_perfil_aprobacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_perfil_aprobacion], [id_tenant], [id_empresa], [codigo], [codigo_entidad], [tipo_proceso], [requiere_mfa], [impide_autoaprobacion], [impide_misma_unidad], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM cumplimiento.perfil_aprobacion;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_perfil_aprobacion_obtener
    @id_perfil_aprobacion bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_perfil_aprobacion], [id_tenant], [id_empresa], [codigo], [codigo_entidad], [tipo_proceso], [requiere_mfa], [impide_autoaprobacion], [impide_misma_unidad], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM cumplimiento.perfil_aprobacion
    WHERE [id_perfil_aprobacion] = @id_perfil_aprobacion;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_perfil_aprobacion_crear
    @id_tenant bigint,
    @id_empresa bigint,
    @codigo nvarchar(80),
    @codigo_entidad nvarchar(128),
    @tipo_proceso varchar(40),
    @requiere_mfa bit,
    @impide_autoaprobacion bit,
    @impide_misma_unidad bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO cumplimiento.perfil_aprobacion ([id_tenant], [id_empresa], [codigo], [codigo_entidad], [tipo_proceso], [requiere_mfa], [impide_autoaprobacion], [impide_misma_unidad], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @id_empresa, @codigo, @codigo_entidad, @tipo_proceso, @requiere_mfa, @impide_autoaprobacion, @impide_misma_unidad, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_perfil_aprobacion_actualizar
    @id_perfil_aprobacion bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @codigo nvarchar(80),
    @codigo_entidad nvarchar(128),
    @tipo_proceso varchar(40),
    @requiere_mfa bit,
    @impide_autoaprobacion bit,
    @impide_misma_unidad bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cumplimiento.perfil_aprobacion
    SET [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [codigo] = @codigo,
        [codigo_entidad] = @codigo_entidad,
        [tipo_proceso] = @tipo_proceso,
        [requiere_mfa] = @requiere_mfa,
        [impide_autoaprobacion] = @impide_autoaprobacion,
        [impide_misma_unidad] = @impide_misma_unidad,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_perfil_aprobacion] = @id_perfil_aprobacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_perfil_aprobacion_desactivar
    @id_perfil_aprobacion bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cumplimiento.perfil_aprobacion
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_perfil_aprobacion] = @id_perfil_aprobacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
