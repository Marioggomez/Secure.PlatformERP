/*
    Migracion: 20260313181856__hardening_scope_operational_crud_claims
    Autor: Mario Gomez
    Fecha UTC: 2026-03-13 18:18:56
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

/* Rollback de CRUD operativos restantes a estado previo */

CREATE OR ALTER PROCEDURE cumplimiento.usp_accion_instancia_aprobacion_actualizar
    @id_accion_instancia_aprobacion bigint,
    @id_instancia_aprobacion bigint,
    @id_paso_instancia_aprobacion bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @id_unidad_organizativa bigint,
    @id_accion_aprobacion smallint,
    @comentario nvarchar(500),
    @mfa_validado bit,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @fecha_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cumplimiento.accion_instancia_aprobacion
    SET [id_instancia_aprobacion] = @id_instancia_aprobacion,
        [id_paso_instancia_aprobacion] = @id_paso_instancia_aprobacion,
        [id_usuario] = @id_usuario,
        [id_empresa] = @id_empresa,
        [id_unidad_organizativa] = @id_unidad_organizativa,
        [id_accion_aprobacion] = @id_accion_aprobacion,
        [comentario] = @comentario,
        [mfa_validado] = @mfa_validado,
        [ip_origen] = @ip_origen,
        [agente_usuario] = @agente_usuario,
        [fecha_utc] = @fecha_utc
    WHERE [id_accion_instancia_aprobacion] = @id_accion_instancia_aprobacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_accion_instancia_aprobacion_crear
    @id_instancia_aprobacion bigint,
    @id_paso_instancia_aprobacion bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @id_unidad_organizativa bigint,
    @id_accion_aprobacion smallint,
    @comentario nvarchar(500),
    @mfa_validado bit,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @fecha_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO cumplimiento.accion_instancia_aprobacion ([id_instancia_aprobacion], [id_paso_instancia_aprobacion], [id_usuario], [id_empresa], [id_unidad_organizativa], [id_accion_aprobacion], [comentario], [mfa_validado], [ip_origen], [agente_usuario], [fecha_utc])
    VALUES (@id_instancia_aprobacion, @id_paso_instancia_aprobacion, @id_usuario, @id_empresa, @id_unidad_organizativa, @id_accion_aprobacion, @comentario, @mfa_validado, @ip_origen, @agente_usuario, @fecha_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_accion_instancia_aprobacion_obtener
    @id_accion_instancia_aprobacion bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_accion_instancia_aprobacion], [id_instancia_aprobacion], [id_paso_instancia_aprobacion], [id_usuario], [id_empresa], [id_unidad_organizativa], [id_accion_aprobacion], [comentario], [mfa_validado], [ip_origen], [agente_usuario], [fecha_utc]
    FROM cumplimiento.accion_instancia_aprobacion
    WHERE [id_accion_instancia_aprobacion] = @id_accion_instancia_aprobacion;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_excepcion_sod_actualizar
    @id_excepcion_sod bigint,
    @id_regla_sod bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @motivo nvarchar(300),
    @aprobado_por bigint,
    @activo bit,
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cumplimiento.excepcion_sod
    SET [id_regla_sod] = @id_regla_sod,
        [id_usuario] = @id_usuario,
        [id_empresa] = @id_empresa,
        [fecha_inicio_utc] = @fecha_inicio_utc,
        [fecha_fin_utc] = @fecha_fin_utc,
        [motivo] = @motivo,
        [aprobado_por] = @aprobado_por,
        [activo] = @activo,
        [creado_utc] = @creado_utc
    WHERE [id_excepcion_sod] = @id_excepcion_sod;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_excepcion_sod_crear
    @id_regla_sod bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @motivo nvarchar(300),
    @aprobado_por bigint,
    @activo bit,
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO cumplimiento.excepcion_sod ([id_regla_sod], [id_usuario], [id_empresa], [fecha_inicio_utc], [fecha_fin_utc], [motivo], [aprobado_por], [activo], [creado_utc])
    VALUES (@id_regla_sod, @id_usuario, @id_empresa, @fecha_inicio_utc, @fecha_fin_utc, @motivo, @aprobado_por, @activo, @creado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_excepcion_sod_obtener
    @id_excepcion_sod bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_excepcion_sod], [id_regla_sod], [id_usuario], [id_empresa], [fecha_inicio_utc], [fecha_fin_utc], [motivo], [aprobado_por], [activo], [creado_utc]
    FROM cumplimiento.excepcion_sod
    WHERE [id_excepcion_sod] = @id_excepcion_sod;
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

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_autorizacion_actualizar
    @id_auditoria_autorizacion bigint,
    @fecha_utc datetime2,
    @id_tenant bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @id_sesion_usuario uniqueidentifier,
    @codigo_permiso nvarchar(150),
    @codigo_operacion nvarchar(150),
    @metodo_http nvarchar(10),
    @permitido bit,
    @motivo nvarchar(200),
    @codigo_entidad nvarchar(128),
    @id_objeto bigint,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @solicitud_id nvarchar(64)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE observabilidad.auditoria_autorizacion
    SET [fecha_utc] = @fecha_utc,
        [id_tenant] = @id_tenant,
        [id_usuario] = @id_usuario,
        [id_empresa] = @id_empresa,
        [id_sesion_usuario] = @id_sesion_usuario,
        [codigo_permiso] = @codigo_permiso,
        [codigo_operacion] = @codigo_operacion,
        [metodo_http] = @metodo_http,
        [permitido] = @permitido,
        [motivo] = @motivo,
        [codigo_entidad] = @codigo_entidad,
        [id_objeto] = @id_objeto,
        [ip_origen] = @ip_origen,
        [agente_usuario] = @agente_usuario,
        [solicitud_id] = @solicitud_id
    WHERE [id_auditoria_autorizacion] = @id_auditoria_autorizacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_autorizacion_crear
    @fecha_utc datetime2,
    @id_tenant bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @id_sesion_usuario uniqueidentifier,
    @codigo_permiso nvarchar(150),
    @codigo_operacion nvarchar(150),
    @metodo_http nvarchar(10),
    @permitido bit,
    @motivo nvarchar(200),
    @codigo_entidad nvarchar(128),
    @id_objeto bigint,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @solicitud_id nvarchar(64)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO observabilidad.auditoria_autorizacion ([fecha_utc], [id_tenant], [id_usuario], [id_empresa], [id_sesion_usuario], [codigo_permiso], [codigo_operacion], [metodo_http], [permitido], [motivo], [codigo_entidad], [id_objeto], [ip_origen], [agente_usuario], [solicitud_id])
    VALUES (@fecha_utc, @id_tenant, @id_usuario, @id_empresa, @id_sesion_usuario, @codigo_permiso, @codigo_operacion, @metodo_http, @permitido, @motivo, @codigo_entidad, @id_objeto, @ip_origen, @agente_usuario, @solicitud_id);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_autorizacion_obtener
    @id_auditoria_autorizacion bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_auditoria_autorizacion], [fecha_utc], [id_tenant], [id_usuario], [id_empresa], [id_sesion_usuario], [codigo_permiso], [codigo_operacion], [metodo_http], [permitido], [motivo], [codigo_entidad], [id_objeto], [ip_origen], [agente_usuario], [solicitud_id]
    FROM observabilidad.auditoria_autorizacion
    WHERE [id_auditoria_autorizacion] = @id_auditoria_autorizacion;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_evento_seguridad_actualizar
    @id_auditoria_evento_seguridad bigint,
    @fecha_utc datetime2,
    @id_tipo_evento_seguridad smallint,
    @id_tenant bigint,
    @id_empresa bigint,
    @id_usuario bigint,
    @id_sesion_usuario uniqueidentifier,
    @detalle nvarchar(500),
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @solicitud_id nvarchar(64)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE observabilidad.auditoria_evento_seguridad
    SET [fecha_utc] = @fecha_utc,
        [id_tipo_evento_seguridad] = @id_tipo_evento_seguridad,
        [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [id_usuario] = @id_usuario,
        [id_sesion_usuario] = @id_sesion_usuario,
        [detalle] = @detalle,
        [ip_origen] = @ip_origen,
        [agente_usuario] = @agente_usuario,
        [solicitud_id] = @solicitud_id
    WHERE [id_auditoria_evento_seguridad] = @id_auditoria_evento_seguridad;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_evento_seguridad_crear
    @fecha_utc datetime2,
    @id_tipo_evento_seguridad smallint,
    @id_tenant bigint,
    @id_empresa bigint,
    @id_usuario bigint,
    @id_sesion_usuario uniqueidentifier,
    @detalle nvarchar(500),
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @solicitud_id nvarchar(64)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO observabilidad.auditoria_evento_seguridad ([fecha_utc], [id_tipo_evento_seguridad], [id_tenant], [id_empresa], [id_usuario], [id_sesion_usuario], [detalle], [ip_origen], [agente_usuario], [solicitud_id])
    VALUES (@fecha_utc, @id_tipo_evento_seguridad, @id_tenant, @id_empresa, @id_usuario, @id_sesion_usuario, @detalle, @ip_origen, @agente_usuario, @solicitud_id);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_evento_seguridad_obtener
    @id_auditoria_evento_seguridad bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_auditoria_evento_seguridad], [fecha_utc], [id_tipo_evento_seguridad], [id_tenant], [id_empresa], [id_usuario], [id_sesion_usuario], [detalle], [ip_origen], [agente_usuario], [solicitud_id]
    FROM observabilidad.auditoria_evento_seguridad
    WHERE [id_auditoria_evento_seguridad] = @id_auditoria_evento_seguridad;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_reinicio_mesa_ayuda_actualizar
    @id_auditoria_reinicio_mesa_ayuda bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @id_usuario_afectado bigint,
    @id_usuario_administrador bigint,
    @motivo nvarchar(300),
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @fecha_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE observabilidad.auditoria_reinicio_mesa_ayuda
    SET [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [id_usuario_afectado] = @id_usuario_afectado,
        [id_usuario_administrador] = @id_usuario_administrador,
        [motivo] = @motivo,
        [ip_origen] = @ip_origen,
        [agente_usuario] = @agente_usuario,
        [fecha_utc] = @fecha_utc
    WHERE [id_auditoria_reinicio_mesa_ayuda] = @id_auditoria_reinicio_mesa_ayuda;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_reinicio_mesa_ayuda_crear
    @id_tenant bigint,
    @id_empresa bigint,
    @id_usuario_afectado bigint,
    @id_usuario_administrador bigint,
    @motivo nvarchar(300),
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @fecha_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO observabilidad.auditoria_reinicio_mesa_ayuda ([id_tenant], [id_empresa], [id_usuario_afectado], [id_usuario_administrador], [motivo], [ip_origen], [agente_usuario], [fecha_utc])
    VALUES (@id_tenant, @id_empresa, @id_usuario_afectado, @id_usuario_administrador, @motivo, @ip_origen, @agente_usuario, @fecha_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_reinicio_mesa_ayuda_obtener
    @id_auditoria_reinicio_mesa_ayuda bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_auditoria_reinicio_mesa_ayuda], [id_tenant], [id_empresa], [id_usuario_afectado], [id_usuario_administrador], [motivo], [ip_origen], [agente_usuario], [fecha_utc]
    FROM observabilidad.auditoria_reinicio_mesa_ayuda
    WHERE [id_auditoria_reinicio_mesa_ayuda] = @id_auditoria_reinicio_mesa_ayuda;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_error_aplicacion_actualizar
    @id_error_aplicacion bigint,
    @fecha_utc datetime2,
    @id_tenant bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @id_sesion_usuario uniqueidentifier,
    @solicitud_id nvarchar(64),
    @endpoint nvarchar(200),
    @metodo_http nvarchar(10),
    @query_string nvarchar(2000),
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @tipo_error nvarchar(200),
    @mensaje_error nvarchar(max),
    @traza_error nvarchar(max),
    @mensaje_interno nvarchar(max),
    @traza_interna nvarchar(max),
    @origen_error nvarchar(200),
    @codigo_http int
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE observabilidad.error_aplicacion
    SET [fecha_utc] = @fecha_utc,
        [id_tenant] = @id_tenant,
        [id_usuario] = @id_usuario,
        [id_empresa] = @id_empresa,
        [id_sesion_usuario] = @id_sesion_usuario,
        [solicitud_id] = @solicitud_id,
        [endpoint] = @endpoint,
        [metodo_http] = @metodo_http,
        [query_string] = @query_string,
        [ip_origen] = @ip_origen,
        [agente_usuario] = @agente_usuario,
        [tipo_error] = @tipo_error,
        [mensaje_error] = @mensaje_error,
        [traza_error] = @traza_error,
        [mensaje_interno] = @mensaje_interno,
        [traza_interna] = @traza_interna,
        [origen_error] = @origen_error,
        [codigo_http] = @codigo_http
    WHERE [id_error_aplicacion] = @id_error_aplicacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_error_aplicacion_crear
    @fecha_utc datetime2,
    @id_tenant bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @id_sesion_usuario uniqueidentifier,
    @solicitud_id nvarchar(64),
    @endpoint nvarchar(200),
    @metodo_http nvarchar(10),
    @query_string nvarchar(2000),
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @tipo_error nvarchar(200),
    @mensaje_error nvarchar(max),
    @traza_error nvarchar(max),
    @mensaje_interno nvarchar(max),
    @traza_interna nvarchar(max),
    @origen_error nvarchar(200),
    @codigo_http int
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO observabilidad.error_aplicacion ([fecha_utc], [id_tenant], [id_usuario], [id_empresa], [id_sesion_usuario], [solicitud_id], [endpoint], [metodo_http], [query_string], [ip_origen], [agente_usuario], [tipo_error], [mensaje_error], [traza_error], [mensaje_interno], [traza_interna], [origen_error], [codigo_http])
    VALUES (@fecha_utc, @id_tenant, @id_usuario, @id_empresa, @id_sesion_usuario, @solicitud_id, @endpoint, @metodo_http, @query_string, @ip_origen, @agente_usuario, @tipo_error, @mensaje_error, @traza_error, @mensaje_interno, @traza_interna, @origen_error, @codigo_http);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_error_aplicacion_obtener
    @id_error_aplicacion bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_error_aplicacion], [fecha_utc], [id_tenant], [id_usuario], [id_empresa], [id_sesion_usuario], [solicitud_id], [endpoint], [metodo_http], [query_string], [ip_origen], [agente_usuario], [tipo_error], [mensaje_error], [traza_error], [mensaje_interno], [traza_interna], [origen_error], [codigo_http]
    FROM observabilidad.error_aplicacion
    WHERE [id_error_aplicacion] = @id_error_aplicacion;
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

CREATE OR ALTER PROCEDURE organizacion.usp_empresa_actualizar
    @id_empresa bigint,
    @id_tenant bigint,
    @codigo nvarchar(50),
    @nombre nvarchar(250),
    @nombre_legal nvarchar(300),
    @id_tipo_empresa smallint,
    @id_estado_empresa smallint,
    @identificacion_fiscal nvarchar(50),
    @moneda_base char(3),
    @zona_horaria nvarchar(80),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE organizacion.empresa
    SET [id_tenant] = @id_tenant,
        [codigo] = @codigo,
        [nombre] = @nombre,
        [nombre_legal] = @nombre_legal,
        [id_tipo_empresa] = @id_tipo_empresa,
        [id_estado_empresa] = @id_estado_empresa,
        [identificacion_fiscal] = @identificacion_fiscal,
        [moneda_base] = @moneda_base,
        [zona_horaria] = @zona_horaria,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_empresa] = @id_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_empresa_crear
    @id_tenant bigint,
    @codigo nvarchar(50),
    @nombre nvarchar(250),
    @nombre_legal nvarchar(300),
    @id_tipo_empresa smallint,
    @id_estado_empresa smallint,
    @identificacion_fiscal nvarchar(50),
    @moneda_base char(3),
    @zona_horaria nvarchar(80),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO organizacion.empresa ([id_tenant], [codigo], [nombre], [nombre_legal], [id_tipo_empresa], [id_estado_empresa], [identificacion_fiscal], [moneda_base], [zona_horaria], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @codigo, @nombre, @nombre_legal, @id_tipo_empresa, @id_estado_empresa, @identificacion_fiscal, @moneda_base, @zona_horaria, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_empresa_desactivar
    @id_empresa bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE organizacion.empresa
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_empresa] = @id_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_empresa_obtener
    @id_empresa bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_empresa], [id_tenant], [codigo], [nombre], [nombre_legal], [id_tipo_empresa], [id_estado_empresa], [identificacion_fiscal], [moneda_base], [zona_horaria], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM organizacion.empresa
    WHERE [id_empresa] = @id_empresa;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_actualizar
    @id_grupo_empresarial bigint,
    @id_tenant bigint,
    @codigo nvarchar(50),
    @nombre nvarchar(200),
    @descripcion nvarchar(300),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE organizacion.grupo_empresarial
    SET [id_tenant] = @id_tenant,
        [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_grupo_empresarial] = @id_grupo_empresarial;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_crear
    @id_tenant bigint,
    @codigo nvarchar(50),
    @nombre nvarchar(200),
    @descripcion nvarchar(300),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO organizacion.grupo_empresarial ([id_tenant], [codigo], [nombre], [descripcion], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @codigo, @nombre, @descripcion, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_empresa_actualizar
    @id_grupo_empresarial bigint,
    @id_empresa bigint,
    @activo bit,
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE organizacion.grupo_empresarial_empresa
    SET [id_empresa] = @id_empresa,
        [activo] = @activo,
        [creado_utc] = @creado_utc
    WHERE [id_grupo_empresarial] = @id_grupo_empresarial;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_empresa_crear
    @id_empresa bigint,
    @activo bit,
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO organizacion.grupo_empresarial_empresa ([id_empresa], [activo], [creado_utc])
    VALUES (@id_empresa, @activo, @creado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_empresa_obtener
    @id_grupo_empresarial bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_grupo_empresarial], [id_empresa], [activo], [creado_utc]
    FROM organizacion.grupo_empresarial_empresa
    WHERE [id_grupo_empresarial] = @id_grupo_empresarial;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_obtener
    @id_grupo_empresarial bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_grupo_empresarial], [id_tenant], [codigo], [nombre], [descripcion], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM organizacion.grupo_empresarial
    WHERE [id_grupo_empresarial] = @id_grupo_empresarial;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_relacion_empresa_actualizar
    @id_relacion_empresa bigint,
    @id_tenant bigint,
    @id_empresa_origen bigint,
    @id_empresa_destino bigint,
    @id_tipo_relacion_empresa smallint,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @observacion nvarchar(500),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE organizacion.relacion_empresa
    SET [id_tenant] = @id_tenant,
        [id_empresa_origen] = @id_empresa_origen,
        [id_empresa_destino] = @id_empresa_destino,
        [id_tipo_relacion_empresa] = @id_tipo_relacion_empresa,
        [fecha_inicio_utc] = @fecha_inicio_utc,
        [fecha_fin_utc] = @fecha_fin_utc,
        [observacion] = @observacion,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_relacion_empresa] = @id_relacion_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_relacion_empresa_crear
    @id_tenant bigint,
    @id_empresa_origen bigint,
    @id_empresa_destino bigint,
    @id_tipo_relacion_empresa smallint,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @observacion nvarchar(500),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO organizacion.relacion_empresa ([id_tenant], [id_empresa_origen], [id_empresa_destino], [id_tipo_relacion_empresa], [fecha_inicio_utc], [fecha_fin_utc], [observacion], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @id_empresa_origen, @id_empresa_destino, @id_tipo_relacion_empresa, @fecha_inicio_utc, @fecha_fin_utc, @observacion, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_relacion_empresa_obtener
    @id_relacion_empresa bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_relacion_empresa], [id_tenant], [id_empresa_origen], [id_empresa_destino], [id_tipo_relacion_empresa], [fecha_inicio_utc], [fecha_fin_utc], [observacion], [activo], [creado_utc], [actualizado_utc]
    FROM organizacion.relacion_empresa
    WHERE [id_relacion_empresa] = @id_relacion_empresa;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_unidad_organizativa_actualizar
    @id_unidad_organizativa bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @id_tipo_unidad_organizativa smallint,
    @id_unidad_padre bigint,
    @codigo nvarchar(60),
    @nombre nvarchar(200),
    @nivel_jerarquia smallint,
    @ruta_jerarquia nvarchar(500),
    @es_hoja bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE organizacion.unidad_organizativa
    SET [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [id_tipo_unidad_organizativa] = @id_tipo_unidad_organizativa,
        [id_unidad_padre] = @id_unidad_padre,
        [codigo] = @codigo,
        [nombre] = @nombre,
        [nivel_jerarquia] = @nivel_jerarquia,
        [ruta_jerarquia] = @ruta_jerarquia,
        [es_hoja] = @es_hoja,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_unidad_organizativa] = @id_unidad_organizativa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_unidad_organizativa_crear
    @id_tenant bigint,
    @id_empresa bigint,
    @id_tipo_unidad_organizativa smallint,
    @id_unidad_padre bigint,
    @codigo nvarchar(60),
    @nombre nvarchar(200),
    @nivel_jerarquia smallint,
    @ruta_jerarquia nvarchar(500),
    @es_hoja bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO organizacion.unidad_organizativa ([id_tenant], [id_empresa], [id_tipo_unidad_organizativa], [id_unidad_padre], [codigo], [nombre], [nivel_jerarquia], [ruta_jerarquia], [es_hoja], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @id_empresa, @id_tipo_unidad_organizativa, @id_unidad_padre, @codigo, @nombre, @nivel_jerarquia, @ruta_jerarquia, @es_hoja, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_unidad_organizativa_obtener
    @id_unidad_organizativa bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_unidad_organizativa], [id_tenant], [id_empresa], [id_tipo_unidad_organizativa], [id_unidad_padre], [codigo], [nombre], [nivel_jerarquia], [ruta_jerarquia], [es_hoja], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM organizacion.unidad_organizativa
    WHERE [id_unidad_organizativa] = @id_unidad_organizativa;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_configuracion_empresa_actualizar
    @id_configuracion_empresa bigint,
    @id_empresa bigint,
    @id_parametro_configuracion int,
    @valor varchar(500),
    @fecha_creacion datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.configuracion_empresa
    SET [id_empresa] = @id_empresa,
        [id_parametro_configuracion] = @id_parametro_configuracion,
        [valor] = @valor,
        [fecha_creacion] = @fecha_creacion
    WHERE [id_configuracion_empresa] = @id_configuracion_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_configuracion_empresa_crear
    @id_empresa bigint,
    @id_parametro_configuracion int,
    @valor varchar(500),
    @fecha_creacion datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO plataforma.configuracion_empresa ([id_empresa], [id_parametro_configuracion], [valor], [fecha_creacion])
    VALUES (@id_empresa, @id_parametro_configuracion, @valor, @fecha_creacion);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_configuracion_empresa_obtener
    @id_configuracion_empresa bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_configuracion_empresa], [id_empresa], [id_parametro_configuracion], [valor], [fecha_creacion]
    FROM plataforma.configuracion_empresa
    WHERE [id_configuracion_empresa] = @id_configuracion_empresa;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_tenant_actualizar
    @id_tenant bigint,
    @codigo nvarchar(50),
    @nombre nvarchar(200),
    @descripcion nvarchar(500),
    @dominio_principal nvarchar(200),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2,
    @es_entrenamiento bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.tenant
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [dominio_principal] = @dominio_principal,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc,
        [es_entrenamiento] = @es_entrenamiento
    WHERE [id_tenant] = @id_tenant;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_tenant_desactivar
    @id_tenant bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.tenant
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_tenant] = @id_tenant;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_tenant_feature_actualizar
    @id_tenant_feature bigint,
    @id_tenant bigint,
    @id_feature bigint,
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.tenant_feature
    SET [id_tenant] = @id_tenant,
        [id_feature] = @id_feature,
        [activo] = @activo
    WHERE [id_tenant_feature] = @id_tenant_feature;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_tenant_feature_crear
    @id_tenant bigint,
    @id_feature bigint,
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO plataforma.tenant_feature ([id_tenant], [id_feature], [activo])
    VALUES (@id_tenant, @id_feature, @activo);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_tenant_feature_desactivar
    @id_tenant_feature bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.tenant_feature
    SET [activo] = 0
    WHERE [id_tenant_feature] = @id_tenant_feature;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_tenant_feature_obtener
    @id_tenant_feature bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tenant_feature], [id_tenant], [id_feature], [activo]
    FROM plataforma.tenant_feature
    WHERE [id_tenant_feature] = @id_tenant_feature;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_tenant_obtener
    @id_tenant bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tenant], [codigo], [nombre], [descripcion], [dominio_principal], [activo], [creado_utc], [actualizado_utc], [version_fila], [es_entrenamiento]
    FROM plataforma.tenant
    WHERE [id_tenant] = @id_tenant;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_asignacion_rol_usuario_actualizar
    @id_asignacion_rol_usuario bigint,
    @id_usuario bigint,
    @id_tenant bigint,
    @id_rol bigint,
    @id_alcance_asignacion smallint,
    @id_grupo_empresarial bigint,
    @id_empresa bigint,
    @id_unidad_organizativa bigint,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @concedido_por bigint,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.asignacion_rol_usuario
    SET [id_usuario] = @id_usuario,
        [id_tenant] = @id_tenant,
        [id_rol] = @id_rol,
        [id_alcance_asignacion] = @id_alcance_asignacion,
        [id_grupo_empresarial] = @id_grupo_empresarial,
        [id_empresa] = @id_empresa,
        [id_unidad_organizativa] = @id_unidad_organizativa,
        [fecha_inicio_utc] = @fecha_inicio_utc,
        [fecha_fin_utc] = @fecha_fin_utc,
        [concedido_por] = @concedido_por,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_asignacion_rol_usuario] = @id_asignacion_rol_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_asignacion_rol_usuario_crear
    @id_usuario bigint,
    @id_tenant bigint,
    @id_rol bigint,
    @id_alcance_asignacion smallint,
    @id_grupo_empresarial bigint,
    @id_empresa bigint,
    @id_unidad_organizativa bigint,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @concedido_por bigint,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.asignacion_rol_usuario ([id_usuario], [id_tenant], [id_rol], [id_alcance_asignacion], [id_grupo_empresarial], [id_empresa], [id_unidad_organizativa], [fecha_inicio_utc], [fecha_fin_utc], [concedido_por], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_usuario, @id_tenant, @id_rol, @id_alcance_asignacion, @id_grupo_empresarial, @id_empresa, @id_unidad_organizativa, @fecha_inicio_utc, @fecha_fin_utc, @concedido_por, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_asignacion_rol_usuario_obtener
    @id_asignacion_rol_usuario bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_asignacion_rol_usuario], [id_usuario], [id_tenant], [id_rol], [id_alcance_asignacion], [id_grupo_empresarial], [id_empresa], [id_unidad_organizativa], [fecha_inicio_utc], [fecha_fin_utc], [concedido_por], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.asignacion_rol_usuario
    WHERE [id_asignacion_rol_usuario] = @id_asignacion_rol_usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_configuracion_canal_notificacion_actualizar
    @id_configuracion_canal_notificacion bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @id_canal_notificacion smallint,
    @host nvarchar(200),
    @puerto int,
    @usa_ssl bit,
    @usuario_tecnico nvarchar(200),
    @referencia_secreto nvarchar(300),
    @secreto_cifrado varbinary(max),
    @remitente_correo nvarchar(200),
    @nombre_remitente nvarchar(200),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.configuracion_canal_notificacion
    SET [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [id_canal_notificacion] = @id_canal_notificacion,
        [host] = @host,
        [puerto] = @puerto,
        [usa_ssl] = @usa_ssl,
        [usuario_tecnico] = @usuario_tecnico,
        [referencia_secreto] = @referencia_secreto,
        [secreto_cifrado] = @secreto_cifrado,
        [remitente_correo] = @remitente_correo,
        [nombre_remitente] = @nombre_remitente,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_configuracion_canal_notificacion] = @id_configuracion_canal_notificacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_configuracion_canal_notificacion_crear
    @id_tenant bigint,
    @id_empresa bigint,
    @id_canal_notificacion smallint,
    @host nvarchar(200),
    @puerto int,
    @usa_ssl bit,
    @usuario_tecnico nvarchar(200),
    @referencia_secreto nvarchar(300),
    @secreto_cifrado varbinary(max),
    @remitente_correo nvarchar(200),
    @nombre_remitente nvarchar(200),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.configuracion_canal_notificacion ([id_tenant], [id_empresa], [id_canal_notificacion], [host], [puerto], [usa_ssl], [usuario_tecnico], [referencia_secreto], [secreto_cifrado], [remitente_correo], [nombre_remitente], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @id_empresa, @id_canal_notificacion, @host, @puerto, @usa_ssl, @usuario_tecnico, @referencia_secreto, @secreto_cifrado, @remitente_correo, @nombre_remitente, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_configuracion_canal_notificacion_obtener
    @id_configuracion_canal_notificacion bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_configuracion_canal_notificacion], [id_tenant], [id_empresa], [id_canal_notificacion], [host], [puerto], [usa_ssl], [usuario_tecnico], [referencia_secreto], [secreto_cifrado], [remitente_correo], [nombre_remitente], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.configuracion_canal_notificacion
    WHERE [id_configuracion_canal_notificacion] = @id_configuracion_canal_notificacion;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_contador_rate_limit_actualizar
    @id_contador_rate_limit bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @ambito nvarchar(20),
    @llave nvarchar(200),
    @endpoint nvarchar(200),
    @inicio_ventana_utc datetime2,
    @conteo int
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.contador_rate_limit
    SET [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [ambito] = @ambito,
        [llave] = @llave,
        [endpoint] = @endpoint,
        [inicio_ventana_utc] = @inicio_ventana_utc,
        [conteo] = @conteo
    WHERE [id_contador_rate_limit] = @id_contador_rate_limit;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_contador_rate_limit_crear
    @id_tenant bigint,
    @id_empresa bigint,
    @ambito nvarchar(20),
    @llave nvarchar(200),
    @endpoint nvarchar(200),
    @inicio_ventana_utc datetime2,
    @conteo int
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.contador_rate_limit ([id_tenant], [id_empresa], [ambito], [llave], [endpoint], [inicio_ventana_utc], [conteo])
    VALUES (@id_tenant, @id_empresa, @ambito, @llave, @endpoint, @inicio_ventana_utc, @conteo);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_contador_rate_limit_obtener
    @id_contador_rate_limit bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_contador_rate_limit], [id_tenant], [id_empresa], [ambito], [llave], [endpoint], [inicio_ventana_utc], [conteo]
    FROM seguridad.contador_rate_limit
    WHERE [id_contador_rate_limit] = @id_contador_rate_limit;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_deber_actualizar
    @id_deber bigint,
    @id_tenant bigint,
    @codigo nvarchar(120),
    @nombre nvarchar(150),
    @descripcion nvarchar(300),
    @es_sistema bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.deber
    SET [id_tenant] = @id_tenant,
        [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [es_sistema] = @es_sistema,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_deber] = @id_deber;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_deber_crear
    @id_tenant bigint,
    @codigo nvarchar(120),
    @nombre nvarchar(150),
    @descripcion nvarchar(300),
    @es_sistema bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.deber ([id_tenant], [codigo], [nombre], [descripcion], [es_sistema], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @codigo, @nombre, @descripcion, @es_sistema, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_deber_obtener
    @id_deber bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_deber], [id_tenant], [codigo], [nombre], [descripcion], [es_sistema], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM seguridad.deber
    WHERE [id_deber] = @id_deber;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_desafio_mfa_actualizar
    @id_desafio_mfa uniqueidentifier,
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @id_sesion_usuario uniqueidentifier,
    @id_flujo_autenticacion uniqueidentifier,
    @id_proposito_desafio_mfa smallint,
    @id_canal_notificacion smallint,
    @codigo_accion nvarchar(100),
    @otp_hash binary(32),
    @otp_salt varbinary(16),
    @expira_en_utc datetime2,
    @usado bit,
    @intentos smallint,
    @max_intentos smallint,
    @creado_utc datetime2,
    @validado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.desafio_mfa
    SET [id_usuario] = @id_usuario,
        [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [id_sesion_usuario] = @id_sesion_usuario,
        [id_flujo_autenticacion] = @id_flujo_autenticacion,
        [id_proposito_desafio_mfa] = @id_proposito_desafio_mfa,
        [id_canal_notificacion] = @id_canal_notificacion,
        [codigo_accion] = @codigo_accion,
        [otp_hash] = @otp_hash,
        [otp_salt] = @otp_salt,
        [expira_en_utc] = @expira_en_utc,
        [usado] = @usado,
        [intentos] = @intentos,
        [max_intentos] = @max_intentos,
        [creado_utc] = @creado_utc,
        [validado_utc] = @validado_utc
    WHERE [id_desafio_mfa] = @id_desafio_mfa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_desafio_mfa_crear
    @id_desafio_mfa uniqueidentifier,
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @id_sesion_usuario uniqueidentifier,
    @id_flujo_autenticacion uniqueidentifier,
    @id_proposito_desafio_mfa smallint,
    @id_canal_notificacion smallint,
    @codigo_accion nvarchar(100),
    @otp_hash binary(32),
    @otp_salt varbinary(16),
    @expira_en_utc datetime2,
    @usado bit,
    @intentos smallint,
    @max_intentos smallint,
    @creado_utc datetime2,
    @validado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.desafio_mfa ([id_desafio_mfa], [id_usuario], [id_tenant], [id_empresa], [id_sesion_usuario], [id_flujo_autenticacion], [id_proposito_desafio_mfa], [id_canal_notificacion], [codigo_accion], [otp_hash], [otp_salt], [expira_en_utc], [usado], [intentos], [max_intentos], [creado_utc], [validado_utc])
    VALUES (@id_desafio_mfa, @id_usuario, @id_tenant, @id_empresa, @id_sesion_usuario, @id_flujo_autenticacion, @id_proposito_desafio_mfa, @id_canal_notificacion, @codigo_accion, @otp_hash, @otp_salt, @expira_en_utc, @usado, @intentos, @max_intentos, @creado_utc, @validado_utc);
    SELECT @id_desafio_mfa AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_desafio_mfa_obtener
    @id_desafio_mfa uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_desafio_mfa], [id_usuario], [id_tenant], [id_empresa], [id_sesion_usuario], [id_flujo_autenticacion], [id_proposito_desafio_mfa], [id_canal_notificacion], [codigo_accion], [otp_hash], [otp_salt], [expira_en_utc], [usado], [intentos], [max_intentos], [creado_utc], [validado_utc]
    FROM seguridad.desafio_mfa
    WHERE [id_desafio_mfa] = @id_desafio_mfa;
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

CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_actualizar
    @id_filtro_dato_usuario bigint,
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @codigo_entidad nvarchar(128),
    @id_modo_filtro_dato smallint,
    @valor_filtro nvarchar(150),
    @id_unidad_organizativa bigint,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.filtro_dato_usuario
    SET [id_usuario] = @id_usuario,
        [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [codigo_entidad] = @codigo_entidad,
        [id_modo_filtro_dato] = @id_modo_filtro_dato,
        [valor_filtro] = @valor_filtro,
        [id_unidad_organizativa] = @id_unidad_organizativa,
        [fecha_inicio_utc] = @fecha_inicio_utc,
        [fecha_fin_utc] = @fecha_fin_utc,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_filtro_dato_usuario] = @id_filtro_dato_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_crear
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @codigo_entidad nvarchar(128),
    @id_modo_filtro_dato smallint,
    @valor_filtro nvarchar(150),
    @id_unidad_organizativa bigint,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.filtro_dato_usuario ([id_usuario], [id_tenant], [id_empresa], [codigo_entidad], [id_modo_filtro_dato], [valor_filtro], [id_unidad_organizativa], [fecha_inicio_utc], [fecha_fin_utc], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_usuario, @id_tenant, @id_empresa, @codigo_entidad, @id_modo_filtro_dato, @valor_filtro, @id_unidad_organizativa, @fecha_inicio_utc, @fecha_fin_utc, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_obtener
    @id_filtro_dato_usuario bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_filtro_dato_usuario], [id_usuario], [id_tenant], [id_empresa], [codigo_entidad], [id_modo_filtro_dato], [valor_filtro], [id_unidad_organizativa], [fecha_inicio_utc], [fecha_fin_utc], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.filtro_dato_usuario
    WHERE [id_filtro_dato_usuario] = @id_filtro_dato_usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_flujo_autenticacion_actualizar
    @id_flujo_autenticacion uniqueidentifier,
    @id_usuario bigint,
    @id_tenant bigint,
    @mfa_requerido bit,
    @mfa_validado bit,
    @expira_en_utc datetime2,
    @usado bit,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @huella_dispositivo nvarchar(200),
    @solicitud_id nvarchar(64),
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.flujo_autenticacion
    SET [id_usuario] = @id_usuario,
        [id_tenant] = @id_tenant,
        [mfa_requerido] = @mfa_requerido,
        [mfa_validado] = @mfa_validado,
        [expira_en_utc] = @expira_en_utc,
        [usado] = @usado,
        [ip_origen] = @ip_origen,
        [agente_usuario] = @agente_usuario,
        [huella_dispositivo] = @huella_dispositivo,
        [solicitud_id] = @solicitud_id,
        [creado_utc] = @creado_utc
    WHERE [id_flujo_autenticacion] = @id_flujo_autenticacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_flujo_autenticacion_crear
    @id_flujo_autenticacion uniqueidentifier,
    @id_usuario bigint,
    @id_tenant bigint,
    @mfa_requerido bit,
    @mfa_validado bit,
    @expira_en_utc datetime2,
    @usado bit,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @huella_dispositivo nvarchar(200),
    @solicitud_id nvarchar(64),
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.flujo_autenticacion ([id_flujo_autenticacion], [id_usuario], [id_tenant], [mfa_requerido], [mfa_validado], [expira_en_utc], [usado], [ip_origen], [agente_usuario], [huella_dispositivo], [solicitud_id], [creado_utc])
    VALUES (@id_flujo_autenticacion, @id_usuario, @id_tenant, @mfa_requerido, @mfa_validado, @expira_en_utc, @usado, @ip_origen, @agente_usuario, @huella_dispositivo, @solicitud_id, @creado_utc);
    SELECT @id_flujo_autenticacion AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_flujo_autenticacion_obtener
    @id_flujo_autenticacion uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_flujo_autenticacion], [id_usuario], [id_tenant], [mfa_requerido], [mfa_validado], [expira_en_utc], [usado], [ip_origen], [agente_usuario], [huella_dispositivo], [solicitud_id], [creado_utc]
    FROM seguridad.flujo_autenticacion
    WHERE [id_flujo_autenticacion] = @id_flujo_autenticacion;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_empresa_override_actualizar
    @id_empresa bigint,
    @timeout_inactividad_min_override int,
    @timeout_absoluto_min_override int,
    @mfa_obligatorio_override bit,
    @max_intentos_login_override tinyint,
    @minutos_bloqueo_override int,
    @requiere_politica_ip_override bit,
    @requiere_mfa_aprobaciones_override bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.politica_empresa_override
    SET [timeout_inactividad_min_override] = @timeout_inactividad_min_override,
        [timeout_absoluto_min_override] = @timeout_absoluto_min_override,
        [mfa_obligatorio_override] = @mfa_obligatorio_override,
        [max_intentos_login_override] = @max_intentos_login_override,
        [minutos_bloqueo_override] = @minutos_bloqueo_override,
        [requiere_politica_ip_override] = @requiere_politica_ip_override,
        [requiere_mfa_aprobaciones_override] = @requiere_mfa_aprobaciones_override,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_empresa] = @id_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_empresa_override_desactivar
    @id_empresa bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM seguridad.politica_empresa_override
    WHERE [id_empresa] = @id_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_empresa_override_obtener
    @id_empresa bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_empresa], [timeout_inactividad_min_override], [timeout_absoluto_min_override], [mfa_obligatorio_override], [max_intentos_login_override], [minutos_bloqueo_override], [requiere_politica_ip_override], [requiere_mfa_aprobaciones_override], [creado_utc], [actualizado_utc], [version_fila]
    FROM seguridad.politica_empresa_override
    WHERE [id_empresa] = @id_empresa;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_ip_actualizar
    @id_politica_ip bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @ip_o_cidr nvarchar(64),
    @accion varchar(10),
    @prioridad int,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.politica_ip
    SET [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [ip_o_cidr] = @ip_o_cidr,
        [accion] = @accion,
        [prioridad] = @prioridad,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_politica_ip] = @id_politica_ip;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_ip_crear
    @id_tenant bigint,
    @id_empresa bigint,
    @ip_o_cidr nvarchar(64),
    @accion varchar(10),
    @prioridad int,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.politica_ip ([id_tenant], [id_empresa], [ip_o_cidr], [accion], [prioridad], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @id_empresa, @ip_o_cidr, @accion, @prioridad, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_ip_obtener
    @id_politica_ip bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_politica_ip], [id_tenant], [id_empresa], [ip_o_cidr], [accion], [prioridad], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.politica_ip
    WHERE [id_politica_ip] = @id_politica_ip;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_tenant_actualizar
    @id_tenant bigint,
    @timeout_inactividad_min int,
    @timeout_absoluto_min int,
    @longitud_minima_clave tinyint,
    @requiere_mayuscula bit,
    @requiere_minuscula bit,
    @requiere_numero bit,
    @requiere_especial bit,
    @historial_claves tinyint,
    @max_intentos_login tinyint,
    @minutos_bloqueo int,
    @mfa_obligatorio bit,
    @permite_login_local bit,
    @permite_sso bit,
    @requiere_mfa_aprobaciones bit,
    @requiere_politica_ip bit,
    @limite_rate_por_minuto int,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.politica_tenant
    SET [timeout_inactividad_min] = @timeout_inactividad_min,
        [timeout_absoluto_min] = @timeout_absoluto_min,
        [longitud_minima_clave] = @longitud_minima_clave,
        [requiere_mayuscula] = @requiere_mayuscula,
        [requiere_minuscula] = @requiere_minuscula,
        [requiere_numero] = @requiere_numero,
        [requiere_especial] = @requiere_especial,
        [historial_claves] = @historial_claves,
        [max_intentos_login] = @max_intentos_login,
        [minutos_bloqueo] = @minutos_bloqueo,
        [mfa_obligatorio] = @mfa_obligatorio,
        [permite_login_local] = @permite_login_local,
        [permite_sso] = @permite_sso,
        [requiere_mfa_aprobaciones] = @requiere_mfa_aprobaciones,
        [requiere_politica_ip] = @requiere_politica_ip,
        [limite_rate_por_minuto] = @limite_rate_por_minuto,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_tenant] = @id_tenant;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_tenant_desactivar
    @id_tenant bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM seguridad.politica_tenant
    WHERE [id_tenant] = @id_tenant;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_tenant_obtener
    @id_tenant bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tenant], [timeout_inactividad_min], [timeout_absoluto_min], [longitud_minima_clave], [requiere_mayuscula], [requiere_minuscula], [requiere_numero], [requiere_especial], [historial_claves], [max_intentos_login], [minutos_bloqueo], [mfa_obligatorio], [permite_login_local], [permite_sso], [requiere_mfa_aprobaciones], [requiere_politica_ip], [limite_rate_por_minuto], [creado_utc], [actualizado_utc], [version_fila]
    FROM seguridad.politica_tenant
    WHERE [id_tenant] = @id_tenant;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_privilegio_actualizar
    @id_privilegio bigint,
    @id_tenant bigint,
    @codigo nvarchar(120),
    @nombre nvarchar(150),
    @descripcion nvarchar(300),
    @es_sistema bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.privilegio
    SET [id_tenant] = @id_tenant,
        [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [es_sistema] = @es_sistema,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_privilegio] = @id_privilegio;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_privilegio_crear
    @id_tenant bigint,
    @codigo nvarchar(120),
    @nombre nvarchar(150),
    @descripcion nvarchar(300),
    @es_sistema bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.privilegio ([id_tenant], [codigo], [nombre], [descripcion], [es_sistema], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @codigo, @nombre, @descripcion, @es_sistema, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_privilegio_obtener
    @id_privilegio bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_privilegio], [id_tenant], [codigo], [nombre], [descripcion], [es_sistema], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM seguridad.privilegio
    WHERE [id_privilegio] = @id_privilegio;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_rol_actualizar
    @id_rol bigint,
    @id_tenant bigint,
    @codigo nvarchar(120),
    @nombre nvarchar(150),
    @descripcion nvarchar(300),
    @es_sistema bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.rol
    SET [id_tenant] = @id_tenant,
        [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [es_sistema] = @es_sistema,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_rol] = @id_rol;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_rol_crear
    @id_tenant bigint,
    @codigo nvarchar(120),
    @nombre nvarchar(150),
    @descripcion nvarchar(300),
    @es_sistema bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.rol ([id_tenant], [codigo], [nombre], [descripcion], [es_sistema], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @codigo, @nombre, @descripcion, @es_sistema, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_rol_obtener
    @id_rol bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_rol], [id_tenant], [codigo], [nombre], [descripcion], [es_sistema], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM seguridad.rol
    WHERE [id_rol] = @id_rol;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_sesion_usuario_actualizar
    @id_sesion_usuario uniqueidentifier,
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @token_hash binary(32),
    @refresh_hash binary(32),
    @origen_autenticacion varchar(20),
    @mfa_validado bit,
    @creado_utc datetime2,
    @expira_absoluta_utc datetime2,
    @ultima_actividad_utc datetime2,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @huella_dispositivo nvarchar(200),
    @activo bit,
    @revocada_utc datetime2,
    @motivo_revocacion nvarchar(200)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.sesion_usuario
    SET [id_usuario] = @id_usuario,
        [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [token_hash] = @token_hash,
        [refresh_hash] = @refresh_hash,
        [origen_autenticacion] = @origen_autenticacion,
        [mfa_validado] = @mfa_validado,
        [creado_utc] = @creado_utc,
        [expira_absoluta_utc] = @expira_absoluta_utc,
        [ultima_actividad_utc] = @ultima_actividad_utc,
        [ip_origen] = @ip_origen,
        [agente_usuario] = @agente_usuario,
        [huella_dispositivo] = @huella_dispositivo,
        [activo] = @activo,
        [revocada_utc] = @revocada_utc,
        [motivo_revocacion] = @motivo_revocacion
    WHERE [id_sesion_usuario] = @id_sesion_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_sesion_usuario_crear
    @id_sesion_usuario uniqueidentifier,
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @token_hash binary(32),
    @refresh_hash binary(32),
    @origen_autenticacion varchar(20),
    @mfa_validado bit,
    @creado_utc datetime2,
    @expira_absoluta_utc datetime2,
    @ultima_actividad_utc datetime2,
    @ip_origen nvarchar(45),
    @agente_usuario nvarchar(300),
    @huella_dispositivo nvarchar(200),
    @activo bit,
    @revocada_utc datetime2,
    @motivo_revocacion nvarchar(200)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.sesion_usuario ([id_sesion_usuario], [id_usuario], [id_tenant], [id_empresa], [token_hash], [refresh_hash], [origen_autenticacion], [mfa_validado], [creado_utc], [expira_absoluta_utc], [ultima_actividad_utc], [ip_origen], [agente_usuario], [huella_dispositivo], [activo], [revocada_utc], [motivo_revocacion])
    VALUES (@id_sesion_usuario, @id_usuario, @id_tenant, @id_empresa, @token_hash, @refresh_hash, @origen_autenticacion, @mfa_validado, @creado_utc, @expira_absoluta_utc, @ultima_actividad_utc, @ip_origen, @agente_usuario, @huella_dispositivo, @activo, @revocada_utc, @motivo_revocacion);
    SELECT @id_sesion_usuario AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_sesion_usuario_obtener
    @id_sesion_usuario uniqueidentifier
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_sesion_usuario], [id_usuario], [id_tenant], [id_empresa], [token_hash], [refresh_hash], [origen_autenticacion], [mfa_validado], [creado_utc], [expira_absoluta_utc], [ultima_actividad_utc], [ip_origen], [agente_usuario], [huella_dispositivo], [activo], [revocada_utc], [motivo_revocacion], [version_fila]
    FROM seguridad.sesion_usuario
    WHERE [id_sesion_usuario] = @id_sesion_usuario;
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

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_empresa_actualizar
    @id_usuario_scope_empresa bigint,
    @id_usuario bigint,
    @id_empresa bigint
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.usuario_scope_empresa
    SET [id_usuario] = @id_usuario,
        [id_empresa] = @id_empresa
    WHERE [id_usuario_scope_empresa] = @id_usuario_scope_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_empresa_crear
    @id_usuario bigint,
    @id_empresa bigint
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.usuario_scope_empresa ([id_usuario], [id_empresa])
    VALUES (@id_usuario, @id_empresa);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_empresa_obtener
    @id_usuario_scope_empresa bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario_scope_empresa], [id_usuario], [id_empresa]
    FROM seguridad.usuario_scope_empresa
    WHERE [id_usuario_scope_empresa] = @id_usuario_scope_empresa;
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

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_unidad_organizativa_actualizar
    @id_usuario_unidad_organizativa bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @id_unidad_organizativa bigint,
    @rol_operativo nvarchar(50),
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.usuario_unidad_organizativa
    SET [id_usuario] = @id_usuario,
        [id_empresa] = @id_empresa,
        [id_unidad_organizativa] = @id_unidad_organizativa,
        [rol_operativo] = @rol_operativo,
        [fecha_inicio_utc] = @fecha_inicio_utc,
        [fecha_fin_utc] = @fecha_fin_utc,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_usuario_unidad_organizativa] = @id_usuario_unidad_organizativa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_unidad_organizativa_crear
    @id_usuario bigint,
    @id_empresa bigint,
    @id_unidad_organizativa bigint,
    @rol_operativo nvarchar(50),
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.usuario_unidad_organizativa ([id_usuario], [id_empresa], [id_unidad_organizativa], [rol_operativo], [fecha_inicio_utc], [fecha_fin_utc], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_usuario, @id_empresa, @id_unidad_organizativa, @rol_operativo, @fecha_inicio_utc, @fecha_fin_utc, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_unidad_organizativa_obtener
    @id_usuario_unidad_organizativa bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario_unidad_organizativa], [id_usuario], [id_empresa], [id_unidad_organizativa], [rol_operativo], [fecha_inicio_utc], [fecha_fin_utc], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.usuario_unidad_organizativa
    WHERE [id_usuario_unidad_organizativa] = @id_usuario_unidad_organizativa;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tercero_rol_actualizar
    @id_tercero_rol bigint,
    @id_tercero bigint,
    @id_rol_tercero int,
    @id_empresa bigint,
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tercero.tercero_rol
    SET [id_tercero] = @id_tercero,
        [id_rol_tercero] = @id_rol_tercero,
        [id_empresa] = @id_empresa,
        [activo] = @activo
    WHERE [id_tercero_rol] = @id_tercero_rol;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tercero_rol_crear
    @id_tercero bigint,
    @id_rol_tercero int,
    @id_empresa bigint,
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tercero.tercero_rol ([id_tercero], [id_rol_tercero], [id_empresa], [activo])
    VALUES (@id_tercero, @id_rol_tercero, @id_empresa, @activo);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tercero_rol_obtener
    @id_tercero_rol bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tercero_rol], [id_tercero], [id_rol_tercero], [id_empresa], [activo]
    FROM tercero.tercero_rol
    WHERE [id_tercero_rol] = @id_tercero_rol;
END
GO

