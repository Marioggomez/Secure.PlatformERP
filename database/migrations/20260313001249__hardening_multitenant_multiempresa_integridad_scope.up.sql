/*
    Migracion: 20260313001249__hardening_multitenant_multiempresa_integridad_scope
    Autor: Mario Gomez
    Fecha UTC: 2026-03-13 00:12:49
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;

/*
    Objetivo:
    - Endurecer integridad referencial para esquema tercero.
    - Reforzar scope tenant+empresa con FK compuesta donde aplique.
    - Completar FKs faltantes en tablas de alcance IAM.
    - Agregar indices clave para filtros por scope.
*/

/* =========================
   Validaciones previas
   ========================= */
IF EXISTS
(
    SELECT 1
    FROM tercero.contacto_tercero c
    LEFT JOIN tercero.tercero t ON t.id_tercero = c.id_tercero
    WHERE t.id_tercero IS NULL
)
    THROW 51000, N'No se puede aplicar migracion: tercero.contacto_tercero contiene id_tercero huerfanos.', 1;

IF EXISTS
(
    SELECT 1
    FROM tercero.direccion_tercero c
    LEFT JOIN tercero.tercero t ON t.id_tercero = c.id_tercero
    WHERE t.id_tercero IS NULL
)
    THROW 51000, N'No se puede aplicar migracion: tercero.direccion_tercero contiene id_tercero huerfanos.', 1;

IF EXISTS
(
    SELECT 1
    FROM tercero.identificacion_tercero c
    LEFT JOIN tercero.tercero t ON t.id_tercero = c.id_tercero
    WHERE t.id_tercero IS NULL
)
    THROW 51000, N'No se puede aplicar migracion: tercero.identificacion_tercero contiene id_tercero huerfanos.', 1;

IF EXISTS
(
    SELECT 1
    FROM tercero.cuenta_bancaria_tercero c
    LEFT JOIN tercero.tercero t ON t.id_tercero = c.id_tercero
    WHERE t.id_tercero IS NULL
)
    THROW 51000, N'No se puede aplicar migracion: tercero.cuenta_bancaria_tercero contiene id_tercero huerfanos.', 1;

IF EXISTS
(
    SELECT 1
    FROM tercero.indice_busqueda_tercero c
    LEFT JOIN tercero.tercero t ON t.id_tercero = c.id_tercero
    WHERE t.id_tercero IS NULL
)
    THROW 51000, N'No se puede aplicar migracion: tercero.indice_busqueda_tercero contiene id_tercero huerfanos.', 1;

IF EXISTS
(
    SELECT 1
    FROM tercero.relacion_tercero r
    LEFT JOIN tercero.tercero torigen ON torigen.id_tercero = r.id_tercero_origen
    LEFT JOIN tercero.tercero tdestino ON tdestino.id_tercero = r.id_tercero_destino
    WHERE torigen.id_tercero IS NULL OR tdestino.id_tercero IS NULL
)
    THROW 51000, N'No se puede aplicar migracion: tercero.relacion_tercero contiene referencias huerfanas.', 1;

IF EXISTS
(
    SELECT 1
    FROM tercero.merge_tercero m
    LEFT JOIN tercero.tercero tprincipal ON tprincipal.id_tercero = m.id_tercero_principal
    LEFT JOIN tercero.tercero tduplicado ON tduplicado.id_tercero = m.id_tercero_duplicado
    WHERE tprincipal.id_tercero IS NULL OR tduplicado.id_tercero IS NULL
)
    THROW 51000, N'No se puede aplicar migracion: tercero.merge_tercero contiene referencias huerfanas.', 1;

IF EXISTS
(
    SELECT 1
    FROM tercero.coincidencia_tercero c
    LEFT JOIN tercero.tercero torigen ON torigen.id_tercero = c.id_tercero_origen
    LEFT JOIN tercero.tercero tposible ON tposible.id_tercero = c.id_tercero_posible
    WHERE torigen.id_tercero IS NULL OR tposible.id_tercero IS NULL
)
    THROW 51000, N'No se puede aplicar migracion: tercero.coincidencia_tercero contiene referencias huerfanas.', 1;

IF EXISTS
(
    SELECT 1
    FROM tercero.tercero t
    LEFT JOIN tercero.tipo_persona tp ON tp.id_tipo_persona = t.id_tipo_persona
    WHERE tp.id_tipo_persona IS NULL
)
    THROW 51000, N'No se puede aplicar migracion: tercero.tercero contiene id_tipo_persona huerfanos.', 1;

IF EXISTS
(
    SELECT 1
    FROM tercero.tercero_empresa te
    LEFT JOIN tercero.tercero t ON t.id_tercero = te.id_tercero
    LEFT JOIN organizacion.empresa e ON e.id_empresa = te.id_empresa
    WHERE t.id_tercero IS NULL OR e.id_empresa IS NULL
)
    THROW 51000, N'No se puede aplicar migracion: tercero.tercero_empresa contiene referencias huerfanas.', 1;

IF EXISTS
(
    SELECT 1
    FROM tercero.tercero_rol tr
    LEFT JOIN tercero.tercero t ON t.id_tercero = tr.id_tercero
    LEFT JOIN tercero.rol_tercero rt ON rt.id_rol_tercero = tr.id_rol_tercero
    LEFT JOIN organizacion.empresa e ON e.id_empresa = tr.id_empresa
    WHERE t.id_tercero IS NULL OR rt.id_rol_tercero IS NULL OR (tr.id_empresa IS NOT NULL AND e.id_empresa IS NULL)
)
    THROW 51000, N'No se puede aplicar migracion: tercero.tercero_rol contiene referencias huerfanas.', 1;

IF EXISTS
(
    SELECT 1
    FROM tercero.tercero_empresa
    GROUP BY id_tercero, id_empresa
    HAVING COUNT(1) > 1
)
    THROW 51000, N'No se puede aplicar migracion: tercero.tercero_empresa tiene duplicados por (id_tercero,id_empresa).', 1;

IF EXISTS
(
    SELECT 1
    FROM tercero.tercero_rol
    WHERE id_empresa IS NOT NULL
    GROUP BY id_tercero, id_empresa, id_rol_tercero
    HAVING COUNT(1) > 1
)
    THROW 51000, N'No se puede aplicar migracion: tercero.tercero_rol tiene duplicados por (id_tercero,id_empresa,id_rol_tercero).', 1;

IF EXISTS
(
    SELECT 1
    FROM seguridad.usuario_scope_empresa
    GROUP BY id_usuario, id_empresa
    HAVING COUNT(1) > 1
)
    THROW 51000, N'No se puede aplicar migracion: seguridad.usuario_scope_empresa tiene duplicados por (id_usuario,id_empresa).', 1;

IF EXISTS
(
    SELECT 1
    FROM seguridad.usuario_scope_unidad
    GROUP BY id_usuario, id_unidad_organizativa
    HAVING COUNT(1) > 1
)
    THROW 51000, N'No se puede aplicar migracion: seguridad.usuario_scope_unidad tiene duplicados por (id_usuario,id_unidad_organizativa).', 1;

IF EXISTS
(
    SELECT 1
    FROM plataforma.tenant_feature
    GROUP BY id_tenant, id_feature
    HAVING COUNT(1) > 1
)
    THROW 51000, N'No se puede aplicar migracion: plataforma.tenant_feature tiene duplicados por (id_tenant,id_feature).', 1;

IF EXISTS
(
    SELECT 1
    FROM plataforma.configuracion_empresa
    GROUP BY id_empresa, id_parametro_configuracion
    HAVING COUNT(1) > 1
)
    THROW 51000, N'No se puede aplicar migracion: plataforma.configuracion_empresa tiene duplicados por (id_empresa,id_parametro_configuracion).', 1;

/* =========================
   Endurecimiento tercero
   ========================= */
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_tercero_tipo_persona' AND parent_object_id = OBJECT_ID(N'tercero.tercero'))
BEGIN
    ALTER TABLE tercero.tercero WITH CHECK
        ADD CONSTRAINT FK_tercero_tercero_tipo_persona
            FOREIGN KEY (id_tipo_persona) REFERENCES tercero.tipo_persona(id_tipo_persona);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_contacto_tercero_id_tercero' AND parent_object_id = OBJECT_ID(N'tercero.contacto_tercero'))
BEGIN
    ALTER TABLE tercero.contacto_tercero WITH CHECK
        ADD CONSTRAINT FK_tercero_contacto_tercero_id_tercero
            FOREIGN KEY (id_tercero) REFERENCES tercero.tercero(id_tercero);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_direccion_tercero_id_tercero' AND parent_object_id = OBJECT_ID(N'tercero.direccion_tercero'))
BEGIN
    ALTER TABLE tercero.direccion_tercero WITH CHECK
        ADD CONSTRAINT FK_tercero_direccion_tercero_id_tercero
            FOREIGN KEY (id_tercero) REFERENCES tercero.tercero(id_tercero);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_identificacion_tercero_id_tercero' AND parent_object_id = OBJECT_ID(N'tercero.identificacion_tercero'))
BEGIN
    ALTER TABLE tercero.identificacion_tercero WITH CHECK
        ADD CONSTRAINT FK_tercero_identificacion_tercero_id_tercero
            FOREIGN KEY (id_tercero) REFERENCES tercero.tercero(id_tercero);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_cuenta_bancaria_tercero_id_tercero' AND parent_object_id = OBJECT_ID(N'tercero.cuenta_bancaria_tercero'))
BEGIN
    ALTER TABLE tercero.cuenta_bancaria_tercero WITH CHECK
        ADD CONSTRAINT FK_tercero_cuenta_bancaria_tercero_id_tercero
            FOREIGN KEY (id_tercero) REFERENCES tercero.tercero(id_tercero);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_indice_busqueda_tercero_id_tercero' AND parent_object_id = OBJECT_ID(N'tercero.indice_busqueda_tercero'))
BEGIN
    ALTER TABLE tercero.indice_busqueda_tercero WITH CHECK
        ADD CONSTRAINT FK_tercero_indice_busqueda_tercero_id_tercero
            FOREIGN KEY (id_tercero) REFERENCES tercero.tercero(id_tercero);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_relacion_tercero_origen' AND parent_object_id = OBJECT_ID(N'tercero.relacion_tercero'))
BEGIN
    ALTER TABLE tercero.relacion_tercero WITH CHECK
        ADD CONSTRAINT FK_tercero_relacion_tercero_origen
            FOREIGN KEY (id_tercero_origen) REFERENCES tercero.tercero(id_tercero);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_relacion_tercero_destino' AND parent_object_id = OBJECT_ID(N'tercero.relacion_tercero'))
BEGIN
    ALTER TABLE tercero.relacion_tercero WITH CHECK
        ADD CONSTRAINT FK_tercero_relacion_tercero_destino
            FOREIGN KEY (id_tercero_destino) REFERENCES tercero.tercero(id_tercero);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_merge_tercero_principal' AND parent_object_id = OBJECT_ID(N'tercero.merge_tercero'))
BEGIN
    ALTER TABLE tercero.merge_tercero WITH CHECK
        ADD CONSTRAINT FK_tercero_merge_tercero_principal
            FOREIGN KEY (id_tercero_principal) REFERENCES tercero.tercero(id_tercero);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_merge_tercero_duplicado' AND parent_object_id = OBJECT_ID(N'tercero.merge_tercero'))
BEGIN
    ALTER TABLE tercero.merge_tercero WITH CHECK
        ADD CONSTRAINT FK_tercero_merge_tercero_duplicado
            FOREIGN KEY (id_tercero_duplicado) REFERENCES tercero.tercero(id_tercero);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_coincidencia_tercero_origen' AND parent_object_id = OBJECT_ID(N'tercero.coincidencia_tercero'))
BEGIN
    ALTER TABLE tercero.coincidencia_tercero WITH CHECK
        ADD CONSTRAINT FK_tercero_coincidencia_tercero_origen
            FOREIGN KEY (id_tercero_origen) REFERENCES tercero.tercero(id_tercero);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_coincidencia_tercero_posible' AND parent_object_id = OBJECT_ID(N'tercero.coincidencia_tercero'))
BEGIN
    ALTER TABLE tercero.coincidencia_tercero WITH CHECK
        ADD CONSTRAINT FK_tercero_coincidencia_tercero_posible
            FOREIGN KEY (id_tercero_posible) REFERENCES tercero.tercero(id_tercero);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_tercero_empresa_tercero' AND parent_object_id = OBJECT_ID(N'tercero.tercero_empresa'))
BEGIN
    ALTER TABLE tercero.tercero_empresa WITH CHECK
        ADD CONSTRAINT FK_tercero_tercero_empresa_tercero
            FOREIGN KEY (id_tercero) REFERENCES tercero.tercero(id_tercero);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_tercero_empresa_empresa' AND parent_object_id = OBJECT_ID(N'tercero.tercero_empresa'))
BEGIN
    ALTER TABLE tercero.tercero_empresa WITH CHECK
        ADD CONSTRAINT FK_tercero_tercero_empresa_empresa
            FOREIGN KEY (id_empresa) REFERENCES organizacion.empresa(id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_tercero_rol_tercero' AND parent_object_id = OBJECT_ID(N'tercero.tercero_rol'))
BEGIN
    ALTER TABLE tercero.tercero_rol WITH CHECK
        ADD CONSTRAINT FK_tercero_tercero_rol_tercero
            FOREIGN KEY (id_tercero) REFERENCES tercero.tercero(id_tercero);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_tercero_rol_rol' AND parent_object_id = OBJECT_ID(N'tercero.tercero_rol'))
BEGIN
    ALTER TABLE tercero.tercero_rol WITH CHECK
        ADD CONSTRAINT FK_tercero_tercero_rol_rol
            FOREIGN KEY (id_rol_tercero) REFERENCES tercero.rol_tercero(id_rol_tercero);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_tercero_tercero_rol_empresa' AND parent_object_id = OBJECT_ID(N'tercero.tercero_rol'))
BEGIN
    ALTER TABLE tercero.tercero_rol WITH CHECK
        ADD CONSTRAINT FK_tercero_tercero_rol_empresa
            FOREIGN KEY (id_empresa) REFERENCES organizacion.empresa(id_empresa);
END;

/* =========================
   FKs de scope IAM faltantes
   ========================= */
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_usuario_scope_empresa_usuario' AND parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_empresa'))
BEGIN
    ALTER TABLE seguridad.usuario_scope_empresa WITH CHECK
        ADD CONSTRAINT FK_seguridad_usuario_scope_empresa_usuario
            FOREIGN KEY (id_usuario) REFERENCES seguridad.usuario(id_usuario);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_usuario_scope_empresa_empresa' AND parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_empresa'))
BEGIN
    ALTER TABLE seguridad.usuario_scope_empresa WITH CHECK
        ADD CONSTRAINT FK_seguridad_usuario_scope_empresa_empresa
            FOREIGN KEY (id_empresa) REFERENCES organizacion.empresa(id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_usuario_scope_unidad_usuario' AND parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_unidad'))
BEGIN
    ALTER TABLE seguridad.usuario_scope_unidad WITH CHECK
        ADD CONSTRAINT FK_seguridad_usuario_scope_unidad_usuario
            FOREIGN KEY (id_usuario) REFERENCES seguridad.usuario(id_usuario);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_usuario_scope_unidad_unidad_organizativa' AND parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_unidad'))
BEGIN
    ALTER TABLE seguridad.usuario_scope_unidad WITH CHECK
        ADD CONSTRAINT FK_seguridad_usuario_scope_unidad_unidad_organizativa
            FOREIGN KEY (id_unidad_organizativa) REFERENCES organizacion.unidad_organizativa(id_unidad_organizativa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_plataforma_configuracion_empresa_empresa' AND parent_object_id = OBJECT_ID(N'plataforma.configuracion_empresa'))
BEGIN
    ALTER TABLE plataforma.configuracion_empresa WITH CHECK
        ADD CONSTRAINT FK_plataforma_configuracion_empresa_empresa
            FOREIGN KEY (id_empresa) REFERENCES organizacion.empresa(id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_plataforma_configuracion_empresa_parametro' AND parent_object_id = OBJECT_ID(N'plataforma.configuracion_empresa'))
BEGIN
    ALTER TABLE plataforma.configuracion_empresa WITH CHECK
        ADD CONSTRAINT FK_plataforma_configuracion_empresa_parametro
            FOREIGN KEY (id_parametro_configuracion) REFERENCES plataforma.parametro_configuracion(id_parametro_configuracion);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_plataforma_tenant_feature_tenant' AND parent_object_id = OBJECT_ID(N'plataforma.tenant_feature'))
BEGIN
    ALTER TABLE plataforma.tenant_feature WITH CHECK
        ADD CONSTRAINT FK_plataforma_tenant_feature_tenant
            FOREIGN KEY (id_tenant) REFERENCES plataforma.tenant(id_tenant);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_plataforma_tenant_feature_feature' AND parent_object_id = OBJECT_ID(N'plataforma.tenant_feature'))
BEGIN
    ALTER TABLE plataforma.tenant_feature WITH CHECK
        ADD CONSTRAINT FK_plataforma_tenant_feature_feature
            FOREIGN KEY (id_feature) REFERENCES plataforma.feature_flag(id_feature);
END;

/* =========================
   FK compuesta tenant+empresa
   ========================= */
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_asignacion_rol_usuario_empresa_scope' AND parent_object_id = OBJECT_ID(N'seguridad.asignacion_rol_usuario'))
BEGIN
    ALTER TABLE seguridad.asignacion_rol_usuario WITH CHECK
        ADD CONSTRAINT FK_seguridad_asignacion_rol_usuario_empresa_scope
            FOREIGN KEY (id_tenant, id_empresa) REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_configuracion_canal_notificacion_empresa_scope' AND parent_object_id = OBJECT_ID(N'seguridad.configuracion_canal_notificacion'))
BEGIN
    ALTER TABLE seguridad.configuracion_canal_notificacion WITH CHECK
        ADD CONSTRAINT FK_seguridad_configuracion_canal_notificacion_empresa_scope
            FOREIGN KEY (id_tenant, id_empresa) REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_contador_rate_limit_empresa_scope' AND parent_object_id = OBJECT_ID(N'seguridad.contador_rate_limit'))
BEGIN
    ALTER TABLE seguridad.contador_rate_limit WITH CHECK
        ADD CONSTRAINT FK_seguridad_contador_rate_limit_empresa_scope
            FOREIGN KEY (id_tenant, id_empresa) REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_desafio_mfa_empresa_scope' AND parent_object_id = OBJECT_ID(N'seguridad.desafio_mfa'))
BEGIN
    ALTER TABLE seguridad.desafio_mfa WITH CHECK
        ADD CONSTRAINT FK_seguridad_desafio_mfa_empresa_scope
            FOREIGN KEY (id_tenant, id_empresa) REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_excepcion_permiso_usuario_empresa_scope' AND parent_object_id = OBJECT_ID(N'seguridad.excepcion_permiso_usuario'))
BEGIN
    ALTER TABLE seguridad.excepcion_permiso_usuario WITH CHECK
        ADD CONSTRAINT FK_seguridad_excepcion_permiso_usuario_empresa_scope
            FOREIGN KEY (id_tenant, id_empresa) REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_filtro_dato_usuario_empresa_scope' AND parent_object_id = OBJECT_ID(N'seguridad.filtro_dato_usuario'))
BEGIN
    ALTER TABLE seguridad.filtro_dato_usuario WITH CHECK
        ADD CONSTRAINT FK_seguridad_filtro_dato_usuario_empresa_scope
            FOREIGN KEY (id_tenant, id_empresa) REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_seguridad_politica_ip_empresa_scope' AND parent_object_id = OBJECT_ID(N'seguridad.politica_ip'))
BEGIN
    ALTER TABLE seguridad.politica_ip WITH CHECK
        ADD CONSTRAINT FK_seguridad_politica_ip_empresa_scope
            FOREIGN KEY (id_tenant, id_empresa) REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_cumplimiento_perfil_aprobacion_empresa_scope' AND parent_object_id = OBJECT_ID(N'cumplimiento.perfil_aprobacion'))
BEGIN
    ALTER TABLE cumplimiento.perfil_aprobacion WITH CHECK
        ADD CONSTRAINT FK_cumplimiento_perfil_aprobacion_empresa_scope
            FOREIGN KEY (id_tenant, id_empresa) REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_cumplimiento_regla_sod_empresa_scope' AND parent_object_id = OBJECT_ID(N'cumplimiento.regla_sod'))
BEGIN
    ALTER TABLE cumplimiento.regla_sod WITH CHECK
        ADD CONSTRAINT FK_cumplimiento_regla_sod_empresa_scope
            FOREIGN KEY (id_tenant, id_empresa) REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_observabilidad_auditoria_autorizacion_empresa_scope' AND parent_object_id = OBJECT_ID(N'observabilidad.auditoria_autorizacion'))
BEGIN
    ALTER TABLE observabilidad.auditoria_autorizacion WITH CHECK
        ADD CONSTRAINT FK_observabilidad_auditoria_autorizacion_empresa_scope
            FOREIGN KEY (id_tenant, id_empresa) REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_observabilidad_auditoria_evento_seguridad_empresa_scope' AND parent_object_id = OBJECT_ID(N'observabilidad.auditoria_evento_seguridad'))
BEGIN
    ALTER TABLE observabilidad.auditoria_evento_seguridad WITH CHECK
        ADD CONSTRAINT FK_observabilidad_auditoria_evento_seguridad_empresa_scope
            FOREIGN KEY (id_tenant, id_empresa) REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_observabilidad_auditoria_reinicio_mesa_ayuda_empresa_scope' AND parent_object_id = OBJECT_ID(N'observabilidad.auditoria_reinicio_mesa_ayuda'))
BEGIN
    ALTER TABLE observabilidad.auditoria_reinicio_mesa_ayuda WITH CHECK
        ADD CONSTRAINT FK_observabilidad_auditoria_reinicio_mesa_ayuda_empresa_scope
            FOREIGN KEY (id_tenant, id_empresa) REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_observabilidad_error_aplicacion_empresa_scope' AND parent_object_id = OBJECT_ID(N'observabilidad.error_aplicacion'))
BEGIN
    ALTER TABLE observabilidad.error_aplicacion WITH CHECK
        ADD CONSTRAINT FK_observabilidad_error_aplicacion_empresa_scope
            FOREIGN KEY (id_tenant, id_empresa) REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_logistica_regla_control_carga_empresa_scope' AND parent_object_id = OBJECT_ID(N'logistica.regla_control_carga'))
BEGIN
    ALTER TABLE logistica.regla_control_carga WITH CHECK
        ADD CONSTRAINT FK_logistica_regla_control_carga_empresa_scope
            FOREIGN KEY (id_tenant, id_empresa) REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_logistica_cotizacion_empresa_scope' AND parent_object_id = OBJECT_ID(N'logistica.cotizacion'))
BEGIN
    ALTER TABLE logistica.cotizacion WITH CHECK
        ADD CONSTRAINT FK_logistica_cotizacion_empresa_scope
            FOREIGN KEY (id_tenant, id_empresa) REFERENCES organizacion.empresa(id_tenant, id_empresa);
END;

/* =========================
   FK tenant-only
   ========================= */
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_logistica_recargo_tenant_scope' AND parent_object_id = OBJECT_ID(N'logistica.recargo'))
BEGIN
    ALTER TABLE logistica.recargo WITH CHECK
        ADD CONSTRAINT FK_logistica_recargo_tenant_scope
            FOREIGN KEY (id_tenant) REFERENCES plataforma.tenant(id_tenant);
END;

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_observabilidad_operacion_api_log_tenant_scope' AND parent_object_id = OBJECT_ID(N'observabilidad.operacion_api_log'))
BEGIN
    ALTER TABLE observabilidad.operacion_api_log WITH CHECK
        ADD CONSTRAINT FK_observabilidad_operacion_api_log_tenant_scope
            FOREIGN KEY (id_tenant) REFERENCES plataforma.tenant(id_tenant);
END;

/* =========================
   Indices de scope / unicidad
   ========================= */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_tercero_tercero_empresa_id_tercero_id_empresa' AND object_id = OBJECT_ID(N'tercero.tercero_empresa'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_tercero_tercero_empresa_id_tercero_id_empresa
        ON tercero.tercero_empresa(id_tercero, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_tercero_tercero_rol_id_tercero_id_empresa_id_rol_tercero' AND object_id = OBJECT_ID(N'tercero.tercero_rol'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_tercero_tercero_rol_id_tercero_id_empresa_id_rol_tercero
        ON tercero.tercero_rol(id_tercero, id_empresa, id_rol_tercero)
        WHERE id_empresa IS NOT NULL;
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_seguridad_usuario_scope_empresa_id_usuario_id_empresa' AND object_id = OBJECT_ID(N'seguridad.usuario_scope_empresa'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_seguridad_usuario_scope_empresa_id_usuario_id_empresa
        ON seguridad.usuario_scope_empresa(id_usuario, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_seguridad_usuario_scope_unidad_id_usuario_id_unidad_organizativa' AND object_id = OBJECT_ID(N'seguridad.usuario_scope_unidad'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_seguridad_usuario_scope_unidad_id_usuario_id_unidad_organizativa
        ON seguridad.usuario_scope_unidad(id_usuario, id_unidad_organizativa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_plataforma_tenant_feature_id_tenant_id_feature' AND object_id = OBJECT_ID(N'plataforma.tenant_feature'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_plataforma_tenant_feature_id_tenant_id_feature
        ON plataforma.tenant_feature(id_tenant, id_feature);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'UX_plataforma_configuracion_empresa_id_empresa_id_parametro_configuracion' AND object_id = OBJECT_ID(N'plataforma.configuracion_empresa'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX UX_plataforma_configuracion_empresa_id_empresa_id_parametro_configuracion
        ON plataforma.configuracion_empresa(id_empresa, id_parametro_configuracion);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_asignacion_rol_usuario_scope' AND object_id = OBJECT_ID(N'seguridad.asignacion_rol_usuario'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_seguridad_asignacion_rol_usuario_scope
        ON seguridad.asignacion_rol_usuario(id_tenant, id_empresa, id_usuario);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_configuracion_canal_notificacion_scope' AND object_id = OBJECT_ID(N'seguridad.configuracion_canal_notificacion'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_seguridad_configuracion_canal_notificacion_scope
        ON seguridad.configuracion_canal_notificacion(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_contador_rate_limit_scope' AND object_id = OBJECT_ID(N'seguridad.contador_rate_limit'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_seguridad_contador_rate_limit_scope
        ON seguridad.contador_rate_limit(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_desafio_mfa_scope' AND object_id = OBJECT_ID(N'seguridad.desafio_mfa'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_seguridad_desafio_mfa_scope
        ON seguridad.desafio_mfa(id_tenant, id_empresa, id_usuario);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_excepcion_permiso_usuario_scope' AND object_id = OBJECT_ID(N'seguridad.excepcion_permiso_usuario'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_seguridad_excepcion_permiso_usuario_scope
        ON seguridad.excepcion_permiso_usuario(id_tenant, id_empresa, id_usuario);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_filtro_dato_usuario_scope' AND object_id = OBJECT_ID(N'seguridad.filtro_dato_usuario'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_seguridad_filtro_dato_usuario_scope
        ON seguridad.filtro_dato_usuario(id_tenant, id_empresa, id_usuario);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_seguridad_politica_ip_scope' AND object_id = OBJECT_ID(N'seguridad.politica_ip'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_seguridad_politica_ip_scope
        ON seguridad.politica_ip(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_cumplimiento_perfil_aprobacion_scope' AND object_id = OBJECT_ID(N'cumplimiento.perfil_aprobacion'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_cumplimiento_perfil_aprobacion_scope
        ON cumplimiento.perfil_aprobacion(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_cumplimiento_regla_sod_scope' AND object_id = OBJECT_ID(N'cumplimiento.regla_sod'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_cumplimiento_regla_sod_scope
        ON cumplimiento.regla_sod(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_logistica_cotizacion_scope' AND object_id = OBJECT_ID(N'logistica.cotizacion'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_logistica_cotizacion_scope
        ON logistica.cotizacion(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_logistica_regla_control_carga_scope' AND object_id = OBJECT_ID(N'logistica.regla_control_carga'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_logistica_regla_control_carga_scope
        ON logistica.regla_control_carga(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_logistica_recargo_id_tenant' AND object_id = OBJECT_ID(N'logistica.recargo'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_logistica_recargo_id_tenant
        ON logistica.recargo(id_tenant);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_observabilidad_operacion_api_log_id_tenant' AND object_id = OBJECT_ID(N'observabilidad.operacion_api_log'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_observabilidad_operacion_api_log_id_tenant
        ON observabilidad.operacion_api_log(id_tenant, fecha);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_observabilidad_auditoria_autorizacion_scope' AND object_id = OBJECT_ID(N'observabilidad.auditoria_autorizacion'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_observabilidad_auditoria_autorizacion_scope
        ON observabilidad.auditoria_autorizacion(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_observabilidad_auditoria_evento_seguridad_scope' AND object_id = OBJECT_ID(N'observabilidad.auditoria_evento_seguridad'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_observabilidad_auditoria_evento_seguridad_scope
        ON observabilidad.auditoria_evento_seguridad(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_observabilidad_auditoria_reinicio_scope' AND object_id = OBJECT_ID(N'observabilidad.auditoria_reinicio_mesa_ayuda'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_observabilidad_auditoria_reinicio_scope
        ON observabilidad.auditoria_reinicio_mesa_ayuda(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_observabilidad_error_aplicacion_scope' AND object_id = OBJECT_ID(N'observabilidad.error_aplicacion'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_observabilidad_error_aplicacion_scope
        ON observabilidad.error_aplicacion(id_tenant, id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_plataforma_configuracion_empresa_id_empresa' AND object_id = OBJECT_ID(N'plataforma.configuracion_empresa'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_plataforma_configuracion_empresa_id_empresa
        ON plataforma.configuracion_empresa(id_empresa);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tercero_tercero_empresa_id_empresa' AND object_id = OBJECT_ID(N'tercero.tercero_empresa'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tercero_tercero_empresa_id_empresa
        ON tercero.tercero_empresa(id_empresa, id_tercero);
END;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_tercero_tercero_rol_id_empresa' AND object_id = OBJECT_ID(N'tercero.tercero_rol'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_tercero_tercero_rol_id_empresa
        ON tercero.tercero_rol(id_empresa, id_tercero);
END;

PRINT N'Migracion aplicada: hardening multitenant/multiempresa e integridad referencial.';
