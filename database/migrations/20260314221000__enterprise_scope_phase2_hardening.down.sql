/*
  Down: 20260314221000__enterprise_scope_phase2_hardening
  Reversion operativa: no revierte conversiones bigint ni opciones de DB por seguridad de datos.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF OBJECT_ID(N'seguridad.trg_filtro_dato_usuario_block_ui_layout', N'TR') IS NOT NULL
    DROP TRIGGER seguridad.trg_filtro_dato_usuario_block_ui_layout;
GO

IF OBJECT_ID(N'seguridad.usp_preferencia_usuario_ui_guardar', N'P') IS NOT NULL
    DROP PROCEDURE seguridad.usp_preferencia_usuario_ui_guardar;
GO

IF OBJECT_ID(N'seguridad.usp_preferencia_usuario_ui_obtener', N'P') IS NOT NULL
    DROP PROCEDURE seguridad.usp_preferencia_usuario_ui_obtener;
GO

/* reponer wrappers legacy vacios de emergencia */
CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_guardar_layout_ui
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint = NULL,
    @codigo_entidad nvarchar(256),
    @layout_payload nvarchar(300)
AS
BEGIN
    SET NOCOUNT ON;
    THROW 51290, N'Requiere reinstalar version previa de procedimiento legacy.', 1;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_obtener_layout_ui
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint = NULL,
    @codigo_entidad nvarchar(256)
AS
BEGIN
    SET NOCOUNT ON;
    THROW 51291, N'Requiere reinstalar version previa de procedimiento legacy.', 1;
END
GO

/* Revertir solo FKs simples eliminadas */
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_cumplimiento_accion_instancia_aprobacion_organizacion_empresa_id_empresa' AND parent_object_id=OBJECT_ID(N'cumplimiento.accion_instancia_aprobacion'))
    ALTER TABLE cumplimiento.accion_instancia_aprobacion WITH CHECK ADD CONSTRAINT FK_cumplimiento_accion_instancia_aprobacion_organizacion_empresa_id_empresa FOREIGN KEY(id_empresa) REFERENCES organizacion.empresa(id_empresa);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_cumplimiento_excepcion_sod_organizacion_empresa_id_empresa' AND parent_object_id=OBJECT_ID(N'cumplimiento.excepcion_sod'))
    ALTER TABLE cumplimiento.excepcion_sod WITH CHECK ADD CONSTRAINT FK_cumplimiento_excepcion_sod_organizacion_empresa_id_empresa FOREIGN KEY(id_empresa) REFERENCES organizacion.empresa(id_empresa);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_logistica_recargo_regla_empresa' AND parent_object_id=OBJECT_ID(N'logistica.recargo_regla'))
    ALTER TABLE logistica.recargo_regla WITH CHECK ADD CONSTRAINT FK_logistica_recargo_regla_empresa FOREIGN KEY(id_empresa) REFERENCES organizacion.empresa(id_empresa);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_organizacion_grupo_empresarial_empresa_organizacion_empresa_id_empresa' AND parent_object_id=OBJECT_ID(N'organizacion.grupo_empresarial_empresa'))
    ALTER TABLE organizacion.grupo_empresarial_empresa WITH CHECK ADD CONSTRAINT FK_organizacion_grupo_empresarial_empresa_organizacion_empresa_id_empresa FOREIGN KEY(id_empresa) REFERENCES organizacion.empresa(id_empresa);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_plataforma_configuracion_empresa_empresa' AND parent_object_id=OBJECT_ID(N'plataforma.configuracion_empresa'))
    ALTER TABLE plataforma.configuracion_empresa WITH CHECK ADD CONSTRAINT FK_plataforma_configuracion_empresa_empresa FOREIGN KEY(id_empresa) REFERENCES organizacion.empresa(id_empresa);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_seguridad_politica_empresa_override_organizacion_empresa_id_empresa' AND parent_object_id=OBJECT_ID(N'seguridad.politica_empresa_override'))
    ALTER TABLE seguridad.politica_empresa_override WITH CHECK ADD CONSTRAINT FK_seguridad_politica_empresa_override_organizacion_empresa_id_empresa FOREIGN KEY(id_empresa) REFERENCES organizacion.empresa(id_empresa);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_tercero_tercero_empresa_empresa' AND parent_object_id=OBJECT_ID(N'tercero.tercero_empresa'))
    ALTER TABLE tercero.tercero_empresa WITH CHECK ADD CONSTRAINT FK_tercero_tercero_empresa_empresa FOREIGN KEY(id_empresa) REFERENCES organizacion.empresa(id_empresa);
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name=N'FK_tercero_tercero_rol_empresa' AND parent_object_id=OBJECT_ID(N'tercero.tercero_rol'))
    ALTER TABLE tercero.tercero_rol WITH CHECK ADD CONSTRAINT FK_tercero_tercero_rol_empresa FOREIGN KEY(id_empresa) REFERENCES organizacion.empresa(id_empresa);
GO
