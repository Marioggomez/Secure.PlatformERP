/*
    Seed inicial para modulo tercero (alcance MVP).
    Autor: Mario Gomez
*/
SET NOCOUNT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @now DATETIME2(3) = SYSUTCDATETIME();
    DECLARE @idTipoForm SMALLINT = (
        SELECT TOP (1) id_tipo_recurso_ui
        FROM catalogo.tipo_recurso_ui
        WHERE codigo = 'FORM'
    );

    IF @idTipoForm IS NULL
    BEGIN
        THROW 50001, 'No existe catalogo.tipo_recurso_ui codigo=FORM.', 1;
    END;

    DECLARE @idEmpresaSeed BIGINT = (
        SELECT TOP (1) id_empresa
        FROM organizacion.empresa
        WHERE activo = 1
        ORDER BY id_empresa
    );

    DECLARE @idRolTerceroDefault INT = (
        SELECT TOP (1) id_rol_tercero
        FROM tercero.rol_tercero
        WHERE activo = 1
        ORDER BY id_rol_tercero
    );

    ------------------------------------------------------------
    -- 1) Catalogo tercero.tipo_persona
    ------------------------------------------------------------
    MERGE tercero.tipo_persona AS target
    USING (VALUES
        (N'NATURAL', N'Persona Natural'),
        (N'JURIDICA', N'Persona Juridica')
    ) AS source (codigo, nombre)
    ON target.codigo = source.codigo
    WHEN MATCHED THEN
        UPDATE SET nombre = source.nombre
    WHEN NOT MATCHED THEN
        INSERT (codigo, nombre)
        VALUES (source.codigo, source.nombre);

    DECLARE @idTipoPersonaNatural INT = (
        SELECT TOP (1) id_tipo_persona
        FROM tercero.tipo_persona
        WHERE codigo = N'NATURAL'
    );

    DECLARE @idTipoPersonaJuridica INT = (
        SELECT TOP (1) id_tipo_persona
        FROM tercero.tipo_persona
        WHERE codigo = N'JURIDICA'
    );

    ------------------------------------------------------------
    -- 2) Terceros base
    ------------------------------------------------------------
    IF EXISTS (SELECT 1 FROM tercero.tercero WHERE codigo = N'TER-001')
    BEGIN
        UPDATE tercero.tercero
        SET
            id_tipo_persona = @idTipoPersonaNatural,
            nombre = N'Carlos',
            apellido = N'Mejia',
            nombre_comercial = N'Carlos Mejia',
            activo = 1,
            creado_por = 1
        WHERE codigo = N'TER-001';
    END
    ELSE
    BEGIN
        INSERT INTO tercero.tercero
        (
            codigo,
            id_tipo_persona,
            nombre,
            segundo_nombre,
            apellido,
            segundo_apellido,
            razon_social,
            nombre_comercial,
            fecha_nacimiento,
            fecha_constitucion,
            activo,
            creado_por,
            creado_utc
        )
        VALUES
        (
            N'TER-001',
            @idTipoPersonaNatural,
            N'Carlos',
            N'Enrique',
            N'Mejia',
            N'Lopez',
            NULL,
            N'Carlos Mejia',
            '1989-02-14',
            NULL,
            1,
            1,
            @now
        );
    END;

    IF EXISTS (SELECT 1 FROM tercero.tercero WHERE codigo = N'TER-002')
    BEGIN
        UPDATE tercero.tercero
        SET
            id_tipo_persona = @idTipoPersonaJuridica,
            razon_social = N'Comercializadora Seed, S.A.',
            nombre_comercial = N'Comercializadora Seed',
            activo = 1,
            creado_por = 1
        WHERE codigo = N'TER-002';
    END
    ELSE
    BEGIN
        INSERT INTO tercero.tercero
        (
            codigo,
            id_tipo_persona,
            nombre,
            segundo_nombre,
            apellido,
            segundo_apellido,
            razon_social,
            nombre_comercial,
            fecha_nacimiento,
            fecha_constitucion,
            activo,
            creado_por,
            creado_utc
        )
        VALUES
        (
            N'TER-002',
            @idTipoPersonaJuridica,
            NULL,
            NULL,
            NULL,
            NULL,
            N'Comercializadora Seed, S.A.',
            N'Comercializadora Seed',
            NULL,
            '2017-06-30',
            1,
            1,
            @now
        );
    END;

    DECLARE @idTercero1 BIGINT = (
        SELECT TOP (1) id_tercero
        FROM tercero.tercero
        WHERE codigo = N'TER-001'
    );

    DECLARE @idTercero2 BIGINT = (
        SELECT TOP (1) id_tercero
        FROM tercero.tercero
        WHERE codigo = N'TER-002'
    );

    ------------------------------------------------------------
    -- 3) Datos relacionados
    ------------------------------------------------------------
    IF NOT EXISTS (
        SELECT 1
        FROM tercero.identificacion_tercero
        WHERE id_tercero = @idTercero1
          AND numero_identificacion = N'1234567-8'
    )
    BEGIN
        INSERT INTO tercero.identificacion_tercero
        (
            id_tercero,
            id_tipo_identificacion,
            numero_identificacion,
            fecha_emision,
            fecha_vencimiento,
            principal
        )
        VALUES
        (
            @idTercero1,
            1,
            N'1234567-8',
            '2020-01-01',
            NULL,
            1
        );
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM tercero.direccion_tercero
        WHERE id_tercero = @idTercero1
          AND ISNULL(direccion_linea1, N'') = N'Zona 10, Ciudad de Guatemala'
    )
    BEGIN
        INSERT INTO tercero.direccion_tercero
        (
            id_tercero,
            id_tipo_direccion,
            direccion_linea1,
            direccion_linea2,
            id_pais,
            id_estado,
            id_ciudad,
            codigo_postal,
            principal
        )
        VALUES
        (
            @idTercero1,
            1,
            N'Zona 10, Ciudad de Guatemala',
            N'Edificio Empresarial',
            1,
            1,
            1,
            N'01010',
            1
        );
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM tercero.contacto_tercero
        WHERE id_tercero = @idTercero1
          AND valor = N'carlos.seed@example.com'
    )
    BEGIN
        INSERT INTO tercero.contacto_tercero
        (
            id_tercero,
            id_tipo_contacto,
            valor,
            principal
        )
        VALUES
        (
            @idTercero1,
            1,
            N'carlos.seed@example.com',
            1
        );
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM tercero.cuenta_bancaria_tercero
        WHERE id_tercero = @idTercero2
          AND numero_cuenta = N'010000123456'
    )
    BEGIN
        INSERT INTO tercero.cuenta_bancaria_tercero
        (
            id_tercero,
            id_banco,
            numero_cuenta,
            id_moneda,
            principal
        )
        VALUES
        (
            @idTercero2,
            1,
            N'010000123456',
            1,
            1
        );
    END;

    IF @idRolTerceroDefault IS NOT NULL AND NOT EXISTS (
        SELECT 1
        FROM tercero.tercero_rol
        WHERE id_tercero = @idTercero2
          AND id_rol_tercero = @idRolTerceroDefault
    )
    BEGIN
        INSERT INTO tercero.tercero_rol
        (
            id_tercero,
            id_rol_tercero,
            id_empresa,
            activo
        )
        VALUES
        (
            @idTercero2,
            @idRolTerceroDefault,
            @idEmpresaSeed,
            1
        );
    END;

    ------------------------------------------------------------
    -- 4) Recursos UI y permisos de navegacion
    ------------------------------------------------------------
    MERGE seguridad.permiso AS target
    USING (VALUES
        (N'TER.TERCERO.LISTAR', N'TERCERO', N'LISTAR', N'Listar terceros', N'Permiso para consultar terceros', 0, 1),
        (N'TER.TIPO_PERSONA.LISTAR', N'TERCERO', N'LISTAR', N'Listar tipos persona', N'Permiso para consultar tipos de persona', 0, 1),
        (N'TER.IDENTIFICACION.LISTAR', N'TERCERO', N'LISTAR', N'Listar identificaciones', N'Permiso para consultar identificaciones de tercero', 0, 1),
        (N'TER.DIRECCION.LISTAR', N'TERCERO', N'LISTAR', N'Listar direcciones', N'Permiso para consultar direcciones de tercero', 0, 1),
        (N'TER.CONTACTO.LISTAR', N'TERCERO', N'LISTAR', N'Listar contactos', N'Permiso para consultar contactos de tercero', 0, 1),
        (N'TER.CUENTA.LISTAR', N'TERCERO', N'LISTAR', N'Listar cuentas bancarias', N'Permiso para consultar cuentas de tercero', 0, 1),
        (N'TER.ROL.LISTAR', N'TERCERO', N'LISTAR', N'Listar roles de tercero', N'Permiso para consultar roles de tercero', 0, 1)
    ) AS source (codigo, modulo, accion, nombre, descripcion, es_sensible, activo)
    ON target.codigo = source.codigo
    WHEN MATCHED THEN
        UPDATE SET
            modulo = source.modulo,
            accion = source.accion,
            nombre = source.nombre,
            descripcion = source.descripcion,
            es_sensible = source.es_sensible,
            activo = source.activo,
            actualizado_utc = @now
    WHEN NOT MATCHED THEN
        INSERT (codigo, modulo, accion, nombre, descripcion, es_sensible, activo)
        VALUES (source.codigo, source.modulo, source.accion, source.nombre, source.descripcion, source.es_sensible, source.activo);

    MERGE seguridad.recurso_ui AS target
    USING (VALUES
        (N'NAV.TERCEROS', N'Terceros', @idTipoForm, N'/tercero/terceros', N'FrmTercerosBuscar', N'BusinessObjects.BOPerson', CAST(NULL AS BIGINT), 30, 1, 1),
        (N'NAV.TIPO_PERSONA', N'Tipos Persona', @idTipoForm, N'/tercero/tipo-persona', N'FrmTipoPersonaBuscar', N'BusinessObjects.BOContact', CAST(NULL AS BIGINT), 31, 1, 1),
        (N'NAV.IDENTIFICACION_TERCERO', N'Identificaciones', @idTipoForm, N'/tercero/identificaciones', N'FrmIdentificacionTerceroBuscar', N'BusinessObjects.BOValidation', CAST(NULL AS BIGINT), 32, 1, 1),
        (N'NAV.DIRECCION_TERCERO', N'Direcciones', @idTipoForm, N'/tercero/direcciones', N'FrmDireccionTerceroBuscar', N'BusinessObjects.BOAddress', CAST(NULL AS BIGINT), 33, 1, 1),
        (N'NAV.CONTACTO_TERCERO', N'Contactos', @idTipoForm, N'/tercero/contactos', N'FrmContactoTerceroBuscar', N'BusinessObjects.BOLead', CAST(NULL AS BIGINT), 34, 1, 1),
        (N'NAV.CUENTA_BANCARIA_TERCERO', N'Cuentas Bancarias', @idTipoForm, N'/tercero/cuentas', N'FrmCuentaBancariaTerceroBuscar', N'BusinessObjects.BOInvoice', CAST(NULL AS BIGINT), 35, 1, 1),
        (N'NAV.TERCERO_ROL', N'Roles de Tercero', @idTipoForm, N'/tercero/roles', N'FrmTerceroRolBuscar', N'BusinessObjects.BORole', CAST(NULL AS BIGINT), 36, 1, 1)
    ) AS source (codigo, nombre, id_tipo_recurso_ui, ruta, componente, icono, id_recurso_ui_padre, orden_visual, es_visible, activo)
    ON target.codigo = source.codigo
    WHEN MATCHED THEN
        UPDATE SET
            nombre = source.nombre,
            id_tipo_recurso_ui = source.id_tipo_recurso_ui,
            ruta = source.ruta,
            componente = source.componente,
            icono = source.icono,
            id_recurso_ui_padre = source.id_recurso_ui_padre,
            orden_visual = source.orden_visual,
            es_visible = source.es_visible,
            activo = source.activo,
            actualizado_utc = @now
    WHEN NOT MATCHED THEN
        INSERT
        (
            codigo,
            nombre,
            id_tipo_recurso_ui,
            ruta,
            componente,
            icono,
            id_recurso_ui_padre,
            orden_visual,
            es_visible,
            activo
        )
        VALUES
        (
            source.codigo,
            source.nombre,
            source.id_tipo_recurso_ui,
            source.ruta,
            source.componente,
            source.icono,
            source.id_recurso_ui_padre,
            source.orden_visual,
            source.es_visible,
            source.activo
        );

    DECLARE @idRecursoUiTerceros BIGINT = (
        SELECT TOP (1) id_recurso_ui
        FROM seguridad.recurso_ui
        WHERE codigo = N'NAV.TERCEROS'
    );

    IF @idRecursoUiTerceros IS NOT NULL
    BEGIN
        UPDATE seguridad.recurso_ui
        SET
            id_recurso_ui_padre = NULL,
            actualizado_utc = @now
        WHERE codigo = N'NAV.TERCEROS'
          AND id_recurso_ui_padre IS NOT NULL;

        UPDATE seguridad.recurso_ui
        SET
            id_recurso_ui_padre = @idRecursoUiTerceros,
            actualizado_utc = @now
        WHERE codigo IN
        (
            N'NAV.TIPO_PERSONA',
            N'NAV.IDENTIFICACION_TERCERO',
            N'NAV.DIRECCION_TERCERO',
            N'NAV.CONTACTO_TERCERO',
            N'NAV.CUENTA_BANCARIA_TERCERO',
            N'NAV.TERCERO_ROL'
        )
          AND (id_recurso_ui_padre IS NULL OR id_recurso_ui_padre <> @idRecursoUiTerceros);
    END;

    MERGE seguridad.recurso_ui_permiso AS target
    USING (
        SELECT r.id_recurso_ui, p.id_permiso
        FROM seguridad.recurso_ui r
        INNER JOIN seguridad.permiso p
            ON (
                (r.codigo = N'NAV.TERCEROS' AND p.codigo = N'TER.TERCERO.LISTAR')
                OR (r.codigo = N'NAV.TIPO_PERSONA' AND p.codigo = N'TER.TIPO_PERSONA.LISTAR')
                OR (r.codigo = N'NAV.IDENTIFICACION_TERCERO' AND p.codigo = N'TER.IDENTIFICACION.LISTAR')
                OR (r.codigo = N'NAV.DIRECCION_TERCERO' AND p.codigo = N'TER.DIRECCION.LISTAR')
                OR (r.codigo = N'NAV.CONTACTO_TERCERO' AND p.codigo = N'TER.CONTACTO.LISTAR')
                OR (r.codigo = N'NAV.CUENTA_BANCARIA_TERCERO' AND p.codigo = N'TER.CUENTA.LISTAR')
                OR (r.codigo = N'NAV.TERCERO_ROL' AND p.codigo = N'TER.ROL.LISTAR')
            )
    ) AS source (id_recurso_ui, id_permiso)
    ON target.id_recurso_ui = source.id_recurso_ui
       AND target.id_permiso = source.id_permiso
    WHEN MATCHED THEN
        UPDATE SET activo = 1
    WHEN NOT MATCHED THEN
        INSERT (id_recurso_ui, id_permiso, activo)
        VALUES (source.id_recurso_ui, source.id_permiso, 1);

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
