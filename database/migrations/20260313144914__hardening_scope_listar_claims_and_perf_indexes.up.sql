/*
    Migracion: 20260313144914__hardening_scope_listar_claims_and_perf_indexes
    Autor: Mario Gomez
    Fecha UTC: 2026-03-13 08:55:00
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO
/* Endurecimiento de listados scoped usando SESSION_CONTEXT */
CREATE OR ALTER PROCEDURE cumplimiento.usp_accion_instancia_aprobacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_accion_instancia_aprobacion], t.[id_instancia_aprobacion], t.[id_paso_instancia_aprobacion], t.[id_usuario], t.[id_empresa], t.[id_unidad_organizativa], t.[id_accion_aprobacion], t.[comentario], t.[mfa_validado], t.[ip_origen], t.[agente_usuario], t.[fecha_utc]
    FROM cumplimiento.accion_instancia_aprobacion AS t
    INNER JOIN organizacion.empresa e ON e.id_empresa = t.id_empresa
    WHERE t.id_empresa = @ctx_id_empresa AND e.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE cumplimiento.usp_excepcion_sod_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_excepcion_sod], t.[id_regla_sod], t.[id_usuario], t.[id_empresa], t.[fecha_inicio_utc], t.[fecha_fin_utc], t.[motivo], t.[aprobado_por], t.[activo], t.[creado_utc]
    FROM cumplimiento.excepcion_sod AS t
    INNER JOIN organizacion.empresa e ON e.id_empresa = t.id_empresa
    WHERE t.id_empresa = @ctx_id_empresa AND e.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE cumplimiento.usp_instancia_aprobacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_instancia_aprobacion], t.[id_tenant], t.[id_empresa], t.[id_unidad_organizativa], t.[id_perfil_aprobacion], t.[codigo_entidad], t.[id_objeto], t.[nivel_actual], t.[id_estado_aprobacion], t.[solicitado_por], t.[solicitado_utc], t.[expira_utc], t.[motivo], t.[hash_payload], t.[activo]
    FROM cumplimiento.instancia_aprobacion AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE cumplimiento.usp_perfil_aprobacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_perfil_aprobacion], t.[id_tenant], t.[id_empresa], t.[codigo], t.[codigo_entidad], t.[tipo_proceso], t.[requiere_mfa], t.[impide_autoaprobacion], t.[impide_misma_unidad], t.[activo], t.[creado_utc], t.[actualizado_utc], t.[version_fila]
    FROM cumplimiento.perfil_aprobacion AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE cumplimiento.usp_regla_sod_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_regla_sod], t.[id_tenant], t.[id_empresa], t.[id_permiso_a], t.[id_permiso_b], t.[codigo_entidad], t.[prohibe_mismo_usuario], t.[prohibe_misma_unidad], t.[prohibe_misma_sesion], t.[prohibe_mismo_dia], t.[id_severidad_sod], t.[activo], t.[creado_utc], t.[actualizado_utc]
    FROM cumplimiento.regla_sod AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_autorizacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_auditoria_autorizacion], t.[fecha_utc], t.[id_tenant], t.[id_usuario], t.[id_empresa], t.[id_sesion_usuario], t.[codigo_permiso], t.[codigo_operacion], t.[metodo_http], t.[permitido], t.[motivo], t.[codigo_entidad], t.[id_objeto], t.[ip_origen], t.[agente_usuario], t.[solicitud_id]
    FROM observabilidad.auditoria_autorizacion AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_evento_seguridad_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_auditoria_evento_seguridad], t.[fecha_utc], t.[id_tipo_evento_seguridad], t.[id_tenant], t.[id_empresa], t.[id_usuario], t.[id_sesion_usuario], t.[detalle], t.[ip_origen], t.[agente_usuario], t.[solicitud_id]
    FROM observabilidad.auditoria_evento_seguridad AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE observabilidad.usp_auditoria_reinicio_mesa_ayuda_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_auditoria_reinicio_mesa_ayuda], t.[id_tenant], t.[id_empresa], t.[id_usuario_afectado], t.[id_usuario_administrador], t.[motivo], t.[ip_origen], t.[agente_usuario], t.[fecha_utc]
    FROM observabilidad.auditoria_reinicio_mesa_ayuda AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE observabilidad.usp_error_aplicacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_error_aplicacion], t.[fecha_utc], t.[id_tenant], t.[id_usuario], t.[id_empresa], t.[id_sesion_usuario], t.[solicitud_id], t.[endpoint], t.[metodo_http], t.[query_string], t.[ip_origen], t.[agente_usuario], t.[tipo_error], t.[mensaje_error], t.[traza_error], t.[mensaje_interno], t.[traza_interna], t.[origen_error], t.[codigo_http]
    FROM observabilidad.error_aplicacion AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE observabilidad.usp_operacion_api_log_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_operacion_api_log], t.[correlation_id], t.[endpoint], t.[metodo_http], t.[usuario], t.[codigo_http], t.[duracion_ms], t.[ip], t.[fecha], t.[id_tenant]
    FROM observabilidad.operacion_api_log AS t
    
    WHERE t.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE organizacion.usp_empresa_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_empresa], t.[id_tenant], t.[codigo], t.[nombre], t.[nombre_legal], t.[id_tipo_empresa], t.[id_estado_empresa], t.[identificacion_fiscal], t.[moneda_base], t.[zona_horaria], t.[activo], t.[creado_utc], t.[actualizado_utc], t.[version_fila]
    FROM organizacion.empresa AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_grupo_empresarial], t.[id_tenant], t.[codigo], t.[nombre], t.[descripcion], t.[activo], t.[creado_utc], t.[actualizado_utc], t.[version_fila]
    FROM organizacion.grupo_empresarial AS t
    
    WHERE t.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_empresa_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_grupo_empresarial], t.[id_empresa], t.[activo], t.[creado_utc]
    FROM organizacion.grupo_empresarial_empresa AS t
    INNER JOIN organizacion.empresa e ON e.id_empresa = t.id_empresa
    WHERE t.id_empresa = @ctx_id_empresa AND e.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE organizacion.usp_relacion_empresa_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_relacion_empresa], t.[id_tenant], t.[id_empresa_origen], t.[id_empresa_destino], t.[id_tipo_relacion_empresa], t.[fecha_inicio_utc], t.[fecha_fin_utc], t.[observacion], t.[activo], t.[creado_utc], t.[actualizado_utc]
    FROM organizacion.relacion_empresa AS t
    
    WHERE t.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE organizacion.usp_unidad_organizativa_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_unidad_organizativa], t.[id_tenant], t.[id_empresa], t.[id_tipo_unidad_organizativa], t.[id_unidad_padre], t.[codigo], t.[nombre], t.[nivel_jerarquia], t.[ruta_jerarquia], t.[es_hoja], t.[activo], t.[creado_utc], t.[actualizado_utc], t.[version_fila]
    FROM organizacion.unidad_organizativa AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE plataforma.usp_configuracion_empresa_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_configuracion_empresa], t.[id_empresa], t.[id_parametro_configuracion], t.[valor], t.[fecha_creacion]
    FROM plataforma.configuracion_empresa AS t
    INNER JOIN organizacion.empresa e ON e.id_empresa = t.id_empresa
    WHERE t.id_empresa = @ctx_id_empresa AND e.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE plataforma.usp_tenant_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_tenant], t.[codigo], t.[nombre], t.[descripcion], t.[dominio_principal], t.[activo], t.[creado_utc], t.[actualizado_utc], t.[version_fila], t.[es_entrenamiento]
    FROM plataforma.tenant AS t
    
    WHERE t.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE plataforma.usp_tenant_feature_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_tenant_feature], t.[id_tenant], t.[id_feature], t.[activo]
    FROM plataforma.tenant_feature AS t
    
    WHERE t.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_asignacion_rol_usuario_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_asignacion_rol_usuario], t.[id_usuario], t.[id_tenant], t.[id_rol], t.[id_alcance_asignacion], t.[id_grupo_empresarial], t.[id_empresa], t.[id_unidad_organizativa], t.[fecha_inicio_utc], t.[fecha_fin_utc], t.[concedido_por], t.[activo], t.[creado_utc], t.[actualizado_utc]
    FROM seguridad.asignacion_rol_usuario AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_configuracion_canal_notificacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_configuracion_canal_notificacion], t.[id_tenant], t.[id_empresa], t.[id_canal_notificacion], t.[host], t.[puerto], t.[usa_ssl], t.[usuario_tecnico], t.[referencia_secreto], t.[secreto_cifrado], t.[remitente_correo], t.[nombre_remitente], t.[activo], t.[creado_utc], t.[actualizado_utc]
    FROM seguridad.configuracion_canal_notificacion AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_contador_rate_limit_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_contador_rate_limit], t.[id_tenant], t.[id_empresa], t.[ambito], t.[llave], t.[endpoint], t.[inicio_ventana_utc], t.[conteo]
    FROM seguridad.contador_rate_limit AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_deber_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_deber], t.[id_tenant], t.[codigo], t.[nombre], t.[descripcion], t.[es_sistema], t.[activo], t.[creado_utc], t.[actualizado_utc], t.[version_fila]
    FROM seguridad.deber AS t
    
    WHERE t.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_desafio_mfa_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_desafio_mfa], t.[id_usuario], t.[id_tenant], t.[id_empresa], t.[id_sesion_usuario], t.[id_flujo_autenticacion], t.[id_proposito_desafio_mfa], t.[id_canal_notificacion], t.[codigo_accion], t.[otp_hash], t.[otp_salt], t.[expira_en_utc], t.[usado], t.[intentos], t.[max_intentos], t.[creado_utc], t.[validado_utc]
    FROM seguridad.desafio_mfa AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_excepcion_permiso_usuario_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_excepcion_permiso_usuario], t.[id_usuario], t.[id_tenant], t.[id_permiso], t.[id_efecto_permiso], t.[id_alcance_asignacion], t.[id_grupo_empresarial], t.[id_empresa], t.[id_unidad_organizativa], t.[fecha_inicio_utc], t.[fecha_fin_utc], t.[concedido_por], t.[motivo], t.[activo], t.[creado_utc], t.[actualizado_utc]
    FROM seguridad.excepcion_permiso_usuario AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_filtro_dato_usuario], t.[id_usuario], t.[id_tenant], t.[id_empresa], t.[codigo_entidad], t.[id_modo_filtro_dato], t.[valor_filtro], t.[id_unidad_organizativa], t.[fecha_inicio_utc], t.[fecha_fin_utc], t.[activo], t.[creado_utc], t.[actualizado_utc]
    FROM seguridad.filtro_dato_usuario AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_flujo_autenticacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_flujo_autenticacion], t.[id_usuario], t.[id_tenant], t.[mfa_requerido], t.[mfa_validado], t.[expira_en_utc], t.[usado], t.[ip_origen], t.[agente_usuario], t.[huella_dispositivo], t.[solicitud_id], t.[creado_utc]
    FROM seguridad.flujo_autenticacion AS t
    
    WHERE t.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_politica_empresa_override_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_empresa], t.[timeout_inactividad_min_override], t.[timeout_absoluto_min_override], t.[mfa_obligatorio_override], t.[max_intentos_login_override], t.[minutos_bloqueo_override], t.[requiere_politica_ip_override], t.[requiere_mfa_aprobaciones_override], t.[creado_utc], t.[actualizado_utc], t.[version_fila]
    FROM seguridad.politica_empresa_override AS t
    INNER JOIN organizacion.empresa e ON e.id_empresa = t.id_empresa
    WHERE t.id_empresa = @ctx_id_empresa AND e.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_politica_ip_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_politica_ip], t.[id_tenant], t.[id_empresa], t.[ip_o_cidr], t.[accion], t.[prioridad], t.[activo], t.[creado_utc], t.[actualizado_utc]
    FROM seguridad.politica_ip AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_politica_tenant_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_tenant], t.[timeout_inactividad_min], t.[timeout_absoluto_min], t.[longitud_minima_clave], t.[requiere_mayuscula], t.[requiere_minuscula], t.[requiere_numero], t.[requiere_especial], t.[historial_claves], t.[max_intentos_login], t.[minutos_bloqueo], t.[mfa_obligatorio], t.[permite_login_local], t.[permite_sso], t.[requiere_mfa_aprobaciones], t.[requiere_politica_ip], t.[limite_rate_por_minuto], t.[creado_utc], t.[actualizado_utc], t.[version_fila]
    FROM seguridad.politica_tenant AS t
    
    WHERE t.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_privilegio_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_privilegio], t.[id_tenant], t.[codigo], t.[nombre], t.[descripcion], t.[es_sistema], t.[activo], t.[creado_utc], t.[actualizado_utc], t.[version_fila]
    FROM seguridad.privilegio AS t
    
    WHERE t.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_rol_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_rol], t.[id_tenant], t.[codigo], t.[nombre], t.[descripcion], t.[es_sistema], t.[activo], t.[creado_utc], t.[actualizado_utc], t.[version_fila]
    FROM seguridad.rol AS t
    
    WHERE t.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_sesion_usuario_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_sesion_usuario], t.[id_usuario], t.[id_tenant], t.[id_empresa], t.[token_hash], t.[refresh_hash], t.[origen_autenticacion], t.[mfa_validado], t.[creado_utc], t.[expira_absoluta_utc], t.[ultima_actividad_utc], t.[ip_origen], t.[agente_usuario], t.[huella_dispositivo], t.[activo], t.[revocada_utc], t.[motivo_revocacion], t.[version_fila]
    FROM seguridad.sesion_usuario AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_usuario_empresa_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_usuario_empresa], t.[id_usuario], t.[id_tenant], t.[id_empresa], t.[es_empresa_predeterminada], t.[puede_operar], t.[fecha_inicio_utc], t.[fecha_fin_utc], t.[activo], t.[creado_utc], t.[actualizado_utc]
    FROM seguridad.usuario_empresa AS t
    
    WHERE t.id_tenant = @ctx_id_tenant AND t.id_empresa = @ctx_id_empresa;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_empresa_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_usuario_scope_empresa], t.[id_usuario], t.[id_empresa]
    FROM seguridad.usuario_scope_empresa AS t
    INNER JOIN organizacion.empresa e ON e.id_empresa = t.id_empresa
    WHERE t.id_empresa = @ctx_id_empresa AND e.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_usuario_tenant_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_usuario], t.[id_tenant], t.[es_administrador_tenant], t.[es_cuenta_servicio], t.[activo], t.[creado_utc], t.[actualizado_utc]
    FROM seguridad.usuario_tenant AS t
    
    WHERE t.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE seguridad.usp_usuario_unidad_organizativa_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_usuario_unidad_organizativa], t.[id_usuario], t.[id_empresa], t.[id_unidad_organizativa], t.[rol_operativo], t.[fecha_inicio_utc], t.[fecha_fin_utc], t.[activo], t.[creado_utc], t.[actualizado_utc]
    FROM seguridad.usuario_unidad_organizativa AS t
    INNER JOIN organizacion.empresa e ON e.id_empresa = t.id_empresa
    WHERE t.id_empresa = @ctx_id_empresa AND e.id_tenant = @ctx_id_tenant;
END
GO
CREATE OR ALTER PROCEDURE tercero.usp_tercero_rol_listar
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;
    SELECT t.[id_tercero_rol], t.[id_tercero], t.[id_rol_tercero], t.[id_empresa], t.[activo]
    FROM tercero.tercero_rol AS t
    INNER JOIN organizacion.empresa e ON e.id_empresa = t.id_empresa
    WHERE t.id_empresa = @ctx_id_empresa AND e.id_tenant = @ctx_id_tenant;
END
GO
/* Limpieza de FKs duplicadas */
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_empresa_tenant' AND parent_object_id=OBJECT_ID(N'organizacion.empresa')) ALTER TABLE organizacion.empresa DROP CONSTRAINT FK_empresa_tenant;
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_usuario_tenant_tenant' AND parent_object_id=OBJECT_ID(N'seguridad.usuario_tenant')) ALTER TABLE seguridad.usuario_tenant DROP CONSTRAINT FK_usuario_tenant_tenant;
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_usuario_tenant_usuario' AND parent_object_id=OBJECT_ID(N'seguridad.usuario_tenant')) ALTER TABLE seguridad.usuario_tenant DROP CONSTRAINT FK_usuario_tenant_usuario;
/* Indices de performance para consultas scoped */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_seguridad_sesion_usuario_scope' AND object_id=OBJECT_ID(N'seguridad.sesion_usuario')) CREATE NONCLUSTERED INDEX IX_seguridad_sesion_usuario_scope ON seguridad.sesion_usuario(id_tenant,id_empresa,activo,expira_absoluta_utc);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_seguridad_usuario_empresa_scope' AND object_id=OBJECT_ID(N'seguridad.usuario_empresa')) CREATE NONCLUSTERED INDEX IX_seguridad_usuario_empresa_scope ON seguridad.usuario_empresa(id_tenant,id_empresa,id_usuario);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_organizacion_unidad_organizativa_scope' AND object_id=OBJECT_ID(N'organizacion.unidad_organizativa')) CREATE NONCLUSTERED INDEX IX_organizacion_unidad_organizativa_scope ON organizacion.unidad_organizativa(id_tenant,id_empresa);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_cumplimiento_instancia_aprobacion_scope' AND object_id=OBJECT_ID(N'cumplimiento.instancia_aprobacion')) CREATE NONCLUSTERED INDEX IX_cumplimiento_instancia_aprobacion_scope ON cumplimiento.instancia_aprobacion(id_tenant,id_empresa);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_logistica_factor_volumetrico_scope' AND object_id=OBJECT_ID(N'logistica.factor_volumetrico')) CREATE NONCLUSTERED INDEX IX_logistica_factor_volumetrico_scope ON logistica.factor_volumetrico(id_tenant,id_empresa);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_logistica_ruta_logistica_scope' AND object_id=OBJECT_ID(N'logistica.ruta_logistica')) CREATE NONCLUSTERED INDEX IX_logistica_ruta_logistica_scope ON logistica.ruta_logistica(id_tenant,id_empresa);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name=N'IX_logistica_temporada_tarifaria_scope' AND object_id=OBJECT_ID(N'logistica.temporada_tarifaria')) CREATE NONCLUSTERED INDEX IX_logistica_temporada_tarifaria_scope ON logistica.temporada_tarifaria(id_tenant,id_empresa);
PRINT N'Migracion aplicada: hardening listados scoped + higiene FK + indices scope.';

