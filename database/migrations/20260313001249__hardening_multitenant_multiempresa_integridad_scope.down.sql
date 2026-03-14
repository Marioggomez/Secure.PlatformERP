/*
    Migracion: 20260313001249__hardening_multitenant_multiempresa_integridad_scope
    Autor: Mario Gomez
    Fecha UTC: 2026-03-13 00:12:49
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

/* =========================
   Eliminar indices
   ========================= */
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tercero_tercero_rol_id_empresa' AND object_id = OBJECT_ID(N'tercero.tercero_rol'))
    DROP INDEX IX_tercero_tercero_rol_id_empresa ON tercero.tercero_rol;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tercero_tercero_empresa_id_empresa' AND object_id = OBJECT_ID(N'tercero.tercero_empresa'))
    DROP INDEX IX_tercero_tercero_empresa_id_empresa ON tercero.tercero_empresa;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_plataforma_configuracion_empresa_id_empresa' AND object_id = OBJECT_ID(N'plataforma.configuracion_empresa'))
    DROP INDEX IX_plataforma_configuracion_empresa_id_empresa ON plataforma.configuracion_empresa;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_observabilidad_error_aplicacion_scope' AND object_id = OBJECT_ID(N'observabilidad.error_aplicacion'))
    DROP INDEX IX_observabilidad_error_aplicacion_scope ON observabilidad.error_aplicacion;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_observabilidad_auditoria_reinicio_scope' AND object_id = OBJECT_ID(N'observabilidad.auditoria_reinicio_mesa_ayuda'))
    DROP INDEX IX_observabilidad_auditoria_reinicio_scope ON observabilidad.auditoria_reinicio_mesa_ayuda;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_observabilidad_auditoria_evento_seguridad_scope' AND object_id = OBJECT_ID(N'observabilidad.auditoria_evento_seguridad'))
    DROP INDEX IX_observabilidad_auditoria_evento_seguridad_scope ON observabilidad.auditoria_evento_seguridad;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_observabilidad_auditoria_autorizacion_scope' AND object_id = OBJECT_ID(N'observabilidad.auditoria_autorizacion'))
    DROP INDEX IX_observabilidad_auditoria_autorizacion_scope ON observabilidad.auditoria_autorizacion;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_observabilidad_operacion_api_log_id_tenant' AND object_id = OBJECT_ID(N'observabilidad.operacion_api_log'))
    DROP INDEX IX_observabilidad_operacion_api_log_id_tenant ON observabilidad.operacion_api_log;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_logistica_recargo_id_tenant' AND object_id = OBJECT_ID(N'logistica.recargo'))
    DROP INDEX IX_logistica_recargo_id_tenant ON logistica.recargo;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_logistica_regla_control_carga_scope' AND object_id = OBJECT_ID(N'logistica.regla_control_carga'))
    DROP INDEX IX_logistica_regla_control_carga_scope ON logistica.regla_control_carga;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_logistica_cotizacion_scope' AND object_id = OBJECT_ID(N'logistica.cotizacion'))
    DROP INDEX IX_logistica_cotizacion_scope ON logistica.cotizacion;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_cumplimiento_regla_sod_scope' AND object_id = OBJECT_ID(N'cumplimiento.regla_sod'))
    DROP INDEX IX_cumplimiento_regla_sod_scope ON cumplimiento.regla_sod;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_cumplimiento_perfil_aprobacion_scope' AND object_id = OBJECT_ID(N'cumplimiento.perfil_aprobacion'))
    DROP INDEX IX_cumplimiento_perfil_aprobacion_scope ON cumplimiento.perfil_aprobacion;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_politica_ip_scope' AND object_id = OBJECT_ID(N'seguridad.politica_ip'))
    DROP INDEX IX_seguridad_politica_ip_scope ON seguridad.politica_ip;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_filtro_dato_usuario_scope' AND object_id = OBJECT_ID(N'seguridad.filtro_dato_usuario'))
    DROP INDEX IX_seguridad_filtro_dato_usuario_scope ON seguridad.filtro_dato_usuario;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_excepcion_permiso_usuario_scope' AND object_id = OBJECT_ID(N'seguridad.excepcion_permiso_usuario'))
    DROP INDEX IX_seguridad_excepcion_permiso_usuario_scope ON seguridad.excepcion_permiso_usuario;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_desafio_mfa_scope' AND object_id = OBJECT_ID(N'seguridad.desafio_mfa'))
    DROP INDEX IX_seguridad_desafio_mfa_scope ON seguridad.desafio_mfa;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_contador_rate_limit_scope' AND object_id = OBJECT_ID(N'seguridad.contador_rate_limit'))
    DROP INDEX IX_seguridad_contador_rate_limit_scope ON seguridad.contador_rate_limit;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_configuracion_canal_notificacion_scope' AND object_id = OBJECT_ID(N'seguridad.configuracion_canal_notificacion'))
    DROP INDEX IX_seguridad_configuracion_canal_notificacion_scope ON seguridad.configuracion_canal_notificacion;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_asignacion_rol_usuario_scope' AND object_id = OBJECT_ID(N'seguridad.asignacion_rol_usuario'))
    DROP INDEX IX_seguridad_asignacion_rol_usuario_scope ON seguridad.asignacion_rol_usuario;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_plataforma_configuracion_empresa_id_empresa_id_parametro_configuracion' AND object_id = OBJECT_ID(N'plataforma.configuracion_empresa'))
    DROP INDEX UX_plataforma_configuracion_empresa_id_empresa_id_parametro_configuracion ON plataforma.configuracion_empresa;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_plataforma_tenant_feature_id_tenant_id_feature' AND object_id = OBJECT_ID(N'plataforma.tenant_feature'))
    DROP INDEX UX_plataforma_tenant_feature_id_tenant_id_feature ON plataforma.tenant_feature;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_seguridad_usuario_scope_unidad_id_usuario_id_unidad_organizativa' AND object_id = OBJECT_ID(N'seguridad.usuario_scope_unidad'))
    DROP INDEX UX_seguridad_usuario_scope_unidad_id_usuario_id_unidad_organizativa ON seguridad.usuario_scope_unidad;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_seguridad_usuario_scope_empresa_id_usuario_id_empresa' AND object_id = OBJECT_ID(N'seguridad.usuario_scope_empresa'))
    DROP INDEX UX_seguridad_usuario_scope_empresa_id_usuario_id_empresa ON seguridad.usuario_scope_empresa;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_tercero_tercero_rol_id_tercero_id_empresa_id_rol_tercero' AND object_id = OBJECT_ID(N'tercero.tercero_rol'))
    DROP INDEX UX_tercero_tercero_rol_id_tercero_id_empresa_id_rol_tercero ON tercero.tercero_rol;

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_tercero_tercero_empresa_id_tercero_id_empresa' AND object_id = OBJECT_ID(N'tercero.tercero_empresa'))
    DROP INDEX UX_tercero_tercero_empresa_id_tercero_id_empresa ON tercero.tercero_empresa;

/* =========================
   Eliminar FKs
   ========================= */
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_observabilidad_operacion_api_log_tenant_scope' AND parent_object_id = OBJECT_ID(N'observabilidad.operacion_api_log'))
    ALTER TABLE observabilidad.operacion_api_log DROP CONSTRAINT FK_observabilidad_operacion_api_log_tenant_scope;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_logistica_recargo_tenant_scope' AND parent_object_id = OBJECT_ID(N'logistica.recargo'))
    ALTER TABLE logistica.recargo DROP CONSTRAINT FK_logistica_recargo_tenant_scope;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_logistica_cotizacion_empresa_scope' AND parent_object_id = OBJECT_ID(N'logistica.cotizacion'))
    ALTER TABLE logistica.cotizacion DROP CONSTRAINT FK_logistica_cotizacion_empresa_scope;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_logistica_regla_control_carga_empresa_scope' AND parent_object_id = OBJECT_ID(N'logistica.regla_control_carga'))
    ALTER TABLE logistica.regla_control_carga DROP CONSTRAINT FK_logistica_regla_control_carga_empresa_scope;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_observabilidad_error_aplicacion_empresa_scope' AND parent_object_id = OBJECT_ID(N'observabilidad.error_aplicacion'))
    ALTER TABLE observabilidad.error_aplicacion DROP CONSTRAINT FK_observabilidad_error_aplicacion_empresa_scope;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_observabilidad_auditoria_reinicio_mesa_ayuda_empresa_scope' AND parent_object_id = OBJECT_ID(N'observabilidad.auditoria_reinicio_mesa_ayuda'))
    ALTER TABLE observabilidad.auditoria_reinicio_mesa_ayuda DROP CONSTRAINT FK_observabilidad_auditoria_reinicio_mesa_ayuda_empresa_scope;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_observabilidad_auditoria_evento_seguridad_empresa_scope' AND parent_object_id = OBJECT_ID(N'observabilidad.auditoria_evento_seguridad'))
    ALTER TABLE observabilidad.auditoria_evento_seguridad DROP CONSTRAINT FK_observabilidad_auditoria_evento_seguridad_empresa_scope;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_observabilidad_auditoria_autorizacion_empresa_scope' AND parent_object_id = OBJECT_ID(N'observabilidad.auditoria_autorizacion'))
    ALTER TABLE observabilidad.auditoria_autorizacion DROP CONSTRAINT FK_observabilidad_auditoria_autorizacion_empresa_scope;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_cumplimiento_regla_sod_empresa_scope' AND parent_object_id = OBJECT_ID(N'cumplimiento.regla_sod'))
    ALTER TABLE cumplimiento.regla_sod DROP CONSTRAINT FK_cumplimiento_regla_sod_empresa_scope;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_cumplimiento_perfil_aprobacion_empresa_scope' AND parent_object_id = OBJECT_ID(N'cumplimiento.perfil_aprobacion'))
    ALTER TABLE cumplimiento.perfil_aprobacion DROP CONSTRAINT FK_cumplimiento_perfil_aprobacion_empresa_scope;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_politica_ip_empresa_scope' AND parent_object_id = OBJECT_ID(N'seguridad.politica_ip'))
    ALTER TABLE seguridad.politica_ip DROP CONSTRAINT FK_seguridad_politica_ip_empresa_scope;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_filtro_dato_usuario_empresa_scope' AND parent_object_id = OBJECT_ID(N'seguridad.filtro_dato_usuario'))
    ALTER TABLE seguridad.filtro_dato_usuario DROP CONSTRAINT FK_seguridad_filtro_dato_usuario_empresa_scope;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_excepcion_permiso_usuario_empresa_scope' AND parent_object_id = OBJECT_ID(N'seguridad.excepcion_permiso_usuario'))
    ALTER TABLE seguridad.excepcion_permiso_usuario DROP CONSTRAINT FK_seguridad_excepcion_permiso_usuario_empresa_scope;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_desafio_mfa_empresa_scope' AND parent_object_id = OBJECT_ID(N'seguridad.desafio_mfa'))
    ALTER TABLE seguridad.desafio_mfa DROP CONSTRAINT FK_seguridad_desafio_mfa_empresa_scope;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_contador_rate_limit_empresa_scope' AND parent_object_id = OBJECT_ID(N'seguridad.contador_rate_limit'))
    ALTER TABLE seguridad.contador_rate_limit DROP CONSTRAINT FK_seguridad_contador_rate_limit_empresa_scope;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_configuracion_canal_notificacion_empresa_scope' AND parent_object_id = OBJECT_ID(N'seguridad.configuracion_canal_notificacion'))
    ALTER TABLE seguridad.configuracion_canal_notificacion DROP CONSTRAINT FK_seguridad_configuracion_canal_notificacion_empresa_scope;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_asignacion_rol_usuario_empresa_scope' AND parent_object_id = OBJECT_ID(N'seguridad.asignacion_rol_usuario'))
    ALTER TABLE seguridad.asignacion_rol_usuario DROP CONSTRAINT FK_seguridad_asignacion_rol_usuario_empresa_scope;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_plataforma_tenant_feature_feature' AND parent_object_id = OBJECT_ID(N'plataforma.tenant_feature'))
    ALTER TABLE plataforma.tenant_feature DROP CONSTRAINT FK_plataforma_tenant_feature_feature;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_plataforma_tenant_feature_tenant' AND parent_object_id = OBJECT_ID(N'plataforma.tenant_feature'))
    ALTER TABLE plataforma.tenant_feature DROP CONSTRAINT FK_plataforma_tenant_feature_tenant;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_plataforma_configuracion_empresa_parametro' AND parent_object_id = OBJECT_ID(N'plataforma.configuracion_empresa'))
    ALTER TABLE plataforma.configuracion_empresa DROP CONSTRAINT FK_plataforma_configuracion_empresa_parametro;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_plataforma_configuracion_empresa_empresa' AND parent_object_id = OBJECT_ID(N'plataforma.configuracion_empresa'))
    ALTER TABLE plataforma.configuracion_empresa DROP CONSTRAINT FK_plataforma_configuracion_empresa_empresa;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_usuario_scope_unidad_unidad_organizativa' AND parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_unidad'))
    ALTER TABLE seguridad.usuario_scope_unidad DROP CONSTRAINT FK_seguridad_usuario_scope_unidad_unidad_organizativa;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_usuario_scope_unidad_usuario' AND parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_unidad'))
    ALTER TABLE seguridad.usuario_scope_unidad DROP CONSTRAINT FK_seguridad_usuario_scope_unidad_usuario;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_usuario_scope_empresa_empresa' AND parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_empresa'))
    ALTER TABLE seguridad.usuario_scope_empresa DROP CONSTRAINT FK_seguridad_usuario_scope_empresa_empresa;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_usuario_scope_empresa_usuario' AND parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_empresa'))
    ALTER TABLE seguridad.usuario_scope_empresa DROP CONSTRAINT FK_seguridad_usuario_scope_empresa_usuario;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_tercero_rol_empresa' AND parent_object_id = OBJECT_ID(N'tercero.tercero_rol'))
    ALTER TABLE tercero.tercero_rol DROP CONSTRAINT FK_tercero_tercero_rol_empresa;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_tercero_rol_rol' AND parent_object_id = OBJECT_ID(N'tercero.tercero_rol'))
    ALTER TABLE tercero.tercero_rol DROP CONSTRAINT FK_tercero_tercero_rol_rol;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_tercero_rol_tercero' AND parent_object_id = OBJECT_ID(N'tercero.tercero_rol'))
    ALTER TABLE tercero.tercero_rol DROP CONSTRAINT FK_tercero_tercero_rol_tercero;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_tercero_empresa_empresa' AND parent_object_id = OBJECT_ID(N'tercero.tercero_empresa'))
    ALTER TABLE tercero.tercero_empresa DROP CONSTRAINT FK_tercero_tercero_empresa_empresa;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_tercero_empresa_tercero' AND parent_object_id = OBJECT_ID(N'tercero.tercero_empresa'))
    ALTER TABLE tercero.tercero_empresa DROP CONSTRAINT FK_tercero_tercero_empresa_tercero;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_coincidencia_tercero_posible' AND parent_object_id = OBJECT_ID(N'tercero.coincidencia_tercero'))
    ALTER TABLE tercero.coincidencia_tercero DROP CONSTRAINT FK_tercero_coincidencia_tercero_posible;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_coincidencia_tercero_origen' AND parent_object_id = OBJECT_ID(N'tercero.coincidencia_tercero'))
    ALTER TABLE tercero.coincidencia_tercero DROP CONSTRAINT FK_tercero_coincidencia_tercero_origen;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_merge_tercero_duplicado' AND parent_object_id = OBJECT_ID(N'tercero.merge_tercero'))
    ALTER TABLE tercero.merge_tercero DROP CONSTRAINT FK_tercero_merge_tercero_duplicado;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_merge_tercero_principal' AND parent_object_id = OBJECT_ID(N'tercero.merge_tercero'))
    ALTER TABLE tercero.merge_tercero DROP CONSTRAINT FK_tercero_merge_tercero_principal;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_relacion_tercero_destino' AND parent_object_id = OBJECT_ID(N'tercero.relacion_tercero'))
    ALTER TABLE tercero.relacion_tercero DROP CONSTRAINT FK_tercero_relacion_tercero_destino;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_relacion_tercero_origen' AND parent_object_id = OBJECT_ID(N'tercero.relacion_tercero'))
    ALTER TABLE tercero.relacion_tercero DROP CONSTRAINT FK_tercero_relacion_tercero_origen;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_indice_busqueda_tercero_id_tercero' AND parent_object_id = OBJECT_ID(N'tercero.indice_busqueda_tercero'))
    ALTER TABLE tercero.indice_busqueda_tercero DROP CONSTRAINT FK_tercero_indice_busqueda_tercero_id_tercero;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_cuenta_bancaria_tercero_id_tercero' AND parent_object_id = OBJECT_ID(N'tercero.cuenta_bancaria_tercero'))
    ALTER TABLE tercero.cuenta_bancaria_tercero DROP CONSTRAINT FK_tercero_cuenta_bancaria_tercero_id_tercero;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_identificacion_tercero_id_tercero' AND parent_object_id = OBJECT_ID(N'tercero.identificacion_tercero'))
    ALTER TABLE tercero.identificacion_tercero DROP CONSTRAINT FK_tercero_identificacion_tercero_id_tercero;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_direccion_tercero_id_tercero' AND parent_object_id = OBJECT_ID(N'tercero.direccion_tercero'))
    ALTER TABLE tercero.direccion_tercero DROP CONSTRAINT FK_tercero_direccion_tercero_id_tercero;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_contacto_tercero_id_tercero' AND parent_object_id = OBJECT_ID(N'tercero.contacto_tercero'))
    ALTER TABLE tercero.contacto_tercero DROP CONSTRAINT FK_tercero_contacto_tercero_id_tercero;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_tercero_tipo_persona' AND parent_object_id = OBJECT_ID(N'tercero.tercero'))
    ALTER TABLE tercero.tercero DROP CONSTRAINT FK_tercero_tercero_tipo_persona;

PRINT N'Rollback aplicado: hardening multitenant/multiempresa e integridad referencial.';
