CREATE OR ALTER PROCEDURE seguridad.usp_politica_operacion_api_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_politica_operacion_api], [id_operacion_api], [id_permiso], [requiere_autenticacion], [requiere_sesion], [requiere_empresa], [requiere_unidad_organizativa], [requiere_mfa], [requiere_auditoria], [requiere_aprobacion], [codigo_entidad], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.politica_operacion_api;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_operacion_api_obtener
    @id_politica_operacion_api bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_politica_operacion_api], [id_operacion_api], [id_permiso], [requiere_autenticacion], [requiere_sesion], [requiere_empresa], [requiere_unidad_organizativa], [requiere_mfa], [requiere_auditoria], [requiere_aprobacion], [codigo_entidad], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.politica_operacion_api
    WHERE [id_politica_operacion_api] = @id_politica_operacion_api;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_operacion_api_crear
    @id_operacion_api bigint,
    @id_permiso int,
    @requiere_autenticacion bit,
    @requiere_sesion bit,
    @requiere_empresa bit,
    @requiere_unidad_organizativa bit,
    @requiere_mfa bit,
    @requiere_auditoria bit,
    @requiere_aprobacion bit,
    @codigo_entidad nvarchar(128),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.politica_operacion_api ([id_operacion_api], [id_permiso], [requiere_autenticacion], [requiere_sesion], [requiere_empresa], [requiere_unidad_organizativa], [requiere_mfa], [requiere_auditoria], [requiere_aprobacion], [codigo_entidad], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_operacion_api, @id_permiso, @requiere_autenticacion, @requiere_sesion, @requiere_empresa, @requiere_unidad_organizativa, @requiere_mfa, @requiere_auditoria, @requiere_aprobacion, @codigo_entidad, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_operacion_api_actualizar
    @id_politica_operacion_api bigint,
    @id_operacion_api bigint,
    @id_permiso int,
    @requiere_autenticacion bit,
    @requiere_sesion bit,
    @requiere_empresa bit,
    @requiere_unidad_organizativa bit,
    @requiere_mfa bit,
    @requiere_auditoria bit,
    @requiere_aprobacion bit,
    @codigo_entidad nvarchar(128),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.politica_operacion_api
    SET [id_operacion_api] = @id_operacion_api,
        [id_permiso] = @id_permiso,
        [requiere_autenticacion] = @requiere_autenticacion,
        [requiere_sesion] = @requiere_sesion,
        [requiere_empresa] = @requiere_empresa,
        [requiere_unidad_organizativa] = @requiere_unidad_organizativa,
        [requiere_mfa] = @requiere_mfa,
        [requiere_auditoria] = @requiere_auditoria,
        [requiere_aprobacion] = @requiere_aprobacion,
        [codigo_entidad] = @codigo_entidad,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_politica_operacion_api] = @id_politica_operacion_api;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_operacion_api_desactivar
    @id_politica_operacion_api bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.politica_operacion_api
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_politica_operacion_api] = @id_politica_operacion_api;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
