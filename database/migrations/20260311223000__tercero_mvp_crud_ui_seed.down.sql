/*
    Migracion: 20260311223000__tercero_mvp_crud_ui_seed
    Autor: Mario Gomez
    Fecha UTC: 2026-03-11
*/
SET NOCOUNT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID(N'tercero.usp_tercero_listar_paginado', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_tercero_listar_paginado;
    IF OBJECT_ID(N'tercero.usp_contacto_tercero_listar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_contacto_tercero_listar;
    IF OBJECT_ID(N'tercero.usp_contacto_tercero_obtener', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_contacto_tercero_obtener;
    IF OBJECT_ID(N'tercero.usp_contacto_tercero_crear', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_contacto_tercero_crear;
    IF OBJECT_ID(N'tercero.usp_contacto_tercero_actualizar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_contacto_tercero_actualizar;
    IF OBJECT_ID(N'tercero.usp_contacto_tercero_desactivar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_contacto_tercero_desactivar;

    IF OBJECT_ID(N'tercero.usp_cuenta_bancaria_tercero_listar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_cuenta_bancaria_tercero_listar;
    IF OBJECT_ID(N'tercero.usp_cuenta_bancaria_tercero_obtener', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_cuenta_bancaria_tercero_obtener;
    IF OBJECT_ID(N'tercero.usp_cuenta_bancaria_tercero_crear', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_cuenta_bancaria_tercero_crear;
    IF OBJECT_ID(N'tercero.usp_cuenta_bancaria_tercero_actualizar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_cuenta_bancaria_tercero_actualizar;
    IF OBJECT_ID(N'tercero.usp_cuenta_bancaria_tercero_desactivar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_cuenta_bancaria_tercero_desactivar;

    IF OBJECT_ID(N'tercero.usp_direccion_tercero_listar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_direccion_tercero_listar;
    IF OBJECT_ID(N'tercero.usp_direccion_tercero_obtener', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_direccion_tercero_obtener;
    IF OBJECT_ID(N'tercero.usp_direccion_tercero_crear', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_direccion_tercero_crear;
    IF OBJECT_ID(N'tercero.usp_direccion_tercero_actualizar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_direccion_tercero_actualizar;
    IF OBJECT_ID(N'tercero.usp_direccion_tercero_desactivar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_direccion_tercero_desactivar;

    IF OBJECT_ID(N'tercero.usp_identificacion_tercero_listar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_identificacion_tercero_listar;
    IF OBJECT_ID(N'tercero.usp_identificacion_tercero_obtener', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_identificacion_tercero_obtener;
    IF OBJECT_ID(N'tercero.usp_identificacion_tercero_crear', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_identificacion_tercero_crear;
    IF OBJECT_ID(N'tercero.usp_identificacion_tercero_actualizar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_identificacion_tercero_actualizar;
    IF OBJECT_ID(N'tercero.usp_identificacion_tercero_desactivar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_identificacion_tercero_desactivar;

    IF OBJECT_ID(N'tercero.usp_tercero_listar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_tercero_listar;
    IF OBJECT_ID(N'tercero.usp_tercero_obtener', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_tercero_obtener;
    IF OBJECT_ID(N'tercero.usp_tercero_crear', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_tercero_crear;
    IF OBJECT_ID(N'tercero.usp_tercero_actualizar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_tercero_actualizar;
    IF OBJECT_ID(N'tercero.usp_tercero_desactivar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_tercero_desactivar;

    IF OBJECT_ID(N'tercero.usp_tercero_rol_listar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_tercero_rol_listar;
    IF OBJECT_ID(N'tercero.usp_tercero_rol_obtener', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_tercero_rol_obtener;
    IF OBJECT_ID(N'tercero.usp_tercero_rol_crear', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_tercero_rol_crear;
    IF OBJECT_ID(N'tercero.usp_tercero_rol_actualizar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_tercero_rol_actualizar;
    IF OBJECT_ID(N'tercero.usp_tercero_rol_desactivar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_tercero_rol_desactivar;

    IF OBJECT_ID(N'tercero.usp_tipo_persona_listar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_tipo_persona_listar;
    IF OBJECT_ID(N'tercero.usp_tipo_persona_obtener', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_tipo_persona_obtener;
    IF OBJECT_ID(N'tercero.usp_tipo_persona_crear', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_tipo_persona_crear;
    IF OBJECT_ID(N'tercero.usp_tipo_persona_actualizar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_tipo_persona_actualizar;
    IF OBJECT_ID(N'tercero.usp_tipo_persona_desactivar', N'P') IS NOT NULL DROP PROCEDURE tercero.usp_tipo_persona_desactivar;

    DELETE rup
    FROM seguridad.recurso_ui_permiso rup
    INNER JOIN seguridad.recurso_ui r ON r.id_recurso_ui = rup.id_recurso_ui
    WHERE r.codigo IN (
        N'NAV.TERCEROS',
        N'NAV.TIPO_PERSONA',
        N'NAV.IDENTIFICACION_TERCERO',
        N'NAV.DIRECCION_TERCERO',
        N'NAV.CONTACTO_TERCERO',
        N'NAV.CUENTA_BANCARIA_TERCERO',
        N'NAV.TERCERO_ROL'
    );

    DELETE FROM seguridad.recurso_ui
    WHERE codigo IN (
        N'NAV.TERCEROS',
        N'NAV.TIPO_PERSONA',
        N'NAV.IDENTIFICACION_TERCERO',
        N'NAV.DIRECCION_TERCERO',
        N'NAV.CONTACTO_TERCERO',
        N'NAV.CUENTA_BANCARIA_TERCERO',
        N'NAV.TERCERO_ROL'
    );

    DELETE FROM seguridad.permiso
    WHERE codigo IN (
        N'TER.TERCERO.LISTAR',
        N'TER.TIPO_PERSONA.LISTAR',
        N'TER.IDENTIFICACION.LISTAR',
        N'TER.DIRECCION.LISTAR',
        N'TER.CONTACTO.LISTAR',
        N'TER.CUENTA.LISTAR',
        N'TER.ROL.LISTAR'
    );

    DELETE FROM tercero.tercero_rol WHERE id_tercero IN (SELECT id_tercero FROM tercero.tercero WHERE codigo IN (N'TER-001', N'TER-002'));
    DELETE FROM tercero.cuenta_bancaria_tercero WHERE id_tercero IN (SELECT id_tercero FROM tercero.tercero WHERE codigo IN (N'TER-001', N'TER-002'));
    DELETE FROM tercero.contacto_tercero WHERE id_tercero IN (SELECT id_tercero FROM tercero.tercero WHERE codigo IN (N'TER-001', N'TER-002'));
    DELETE FROM tercero.direccion_tercero WHERE id_tercero IN (SELECT id_tercero FROM tercero.tercero WHERE codigo IN (N'TER-001', N'TER-002'));
    DELETE FROM tercero.identificacion_tercero WHERE id_tercero IN (SELECT id_tercero FROM tercero.tercero WHERE codigo IN (N'TER-001', N'TER-002'));
    DELETE FROM tercero.tercero WHERE codigo IN (N'TER-001', N'TER-002');

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
