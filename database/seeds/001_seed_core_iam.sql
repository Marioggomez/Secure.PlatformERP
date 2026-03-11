/*
    Seed inicial core para pruebas de endpoints.
    Autor: Mario Gomez
*/
SET NOCOUNT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    ------------------------------------------------------------
    -- 1) Catalogos base
    ------------------------------------------------------------
    MERGE catalogo.tipo_empresa AS target
    USING (VALUES
        ('MATRIZ', N'Empresa Matriz', N'Tipo de empresa para pruebas base')
    ) AS source (codigo, nombre, descripcion)
    ON target.codigo = source.codigo
    WHEN MATCHED THEN
        UPDATE SET
            nombre = source.nombre,
            descripcion = source.descripcion,
            orden_visual = 1,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
    WHEN NOT MATCHED THEN
        INSERT (codigo, nombre, descripcion, orden_visual, activo)
        VALUES (source.codigo, source.nombre, source.descripcion, 1, 1);

    MERGE catalogo.estado_empresa AS target
    USING (VALUES
        ('ACTIVA', N'Activa', N'Empresa habilitada para operar')
    ) AS source (codigo, nombre, descripcion)
    ON target.codigo = source.codigo
    WHEN MATCHED THEN
        UPDATE SET
            nombre = source.nombre,
            descripcion = source.descripcion,
            orden_visual = 1,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
    WHEN NOT MATCHED THEN
        INSERT (codigo, nombre, descripcion, orden_visual, activo)
        VALUES (source.codigo, source.nombre, source.descripcion, 1, 1);

    MERGE catalogo.estado_usuario AS target
    USING (VALUES
        ('ACTIVO', N'Activo', N'Usuario habilitado para autenticarse')
    ) AS source (codigo, nombre, descripcion)
    ON target.codigo = source.codigo
    WHEN MATCHED THEN
        UPDATE SET
            nombre = source.nombre,
            descripcion = source.descripcion,
            orden_visual = 1,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
    WHEN NOT MATCHED THEN
        INSERT (codigo, nombre, descripcion, orden_visual, activo)
        VALUES (source.codigo, source.nombre, source.descripcion, 1, 1);

    MERGE catalogo.tipo_recurso_ui AS target
    USING (VALUES
        ('MENU', N'Menu', N'Recurso visual de navegacion', 1),
        ('FORM', N'Formulario', N'Recurso visual tipo formulario', 2)
    ) AS source (codigo, nombre, descripcion, orden_visual)
    ON target.codigo = source.codigo
    WHEN MATCHED THEN
        UPDATE SET
            nombre = source.nombre,
            descripcion = source.descripcion,
            orden_visual = source.orden_visual,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
    WHEN NOT MATCHED THEN
        INSERT (codigo, nombre, descripcion, orden_visual, activo)
        VALUES (source.codigo, source.nombre, source.descripcion, source.orden_visual, 1);

    MERGE catalogo.efecto_permiso AS target
    USING (VALUES
        ('ALLOW', N'Permitir', N'Permiso concedido', 1),
        ('DENY', N'Denegar', N'Permiso denegado', 2)
    ) AS source (codigo, nombre, descripcion, orden_visual)
    ON target.codigo = source.codigo
    WHEN MATCHED THEN
        UPDATE SET
            nombre = source.nombre,
            descripcion = source.descripcion,
            orden_visual = source.orden_visual,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
    WHEN NOT MATCHED THEN
        INSERT (codigo, nombre, descripcion, orden_visual, activo)
        VALUES (source.codigo, source.nombre, source.descripcion, source.orden_visual, 1);

    MERGE catalogo.alcance_asignacion AS target
    USING (VALUES
        ('EMPRESA', N'Alcance Empresa', N'Alcance por empresa para asignacion de rol', 1)
    ) AS source (codigo, nombre, descripcion, orden_visual)
    ON target.codigo = source.codigo
    WHEN MATCHED THEN
        UPDATE SET
            nombre = source.nombre,
            descripcion = source.descripcion,
            orden_visual = source.orden_visual,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
    WHEN NOT MATCHED THEN
        INSERT (codigo, nombre, descripcion, orden_visual, activo)
        VALUES (source.codigo, source.nombre, source.descripcion, source.orden_visual, 1);

    ------------------------------------------------------------
    -- 2) Plataforma y organizacion
    ------------------------------------------------------------
    MERGE plataforma.tenant AS target
    USING (VALUES
        ('SEED', N'Tenant Seed', N'Tenant inicial para pruebas', N'seed.local', 1, 0)
    ) AS source (codigo, nombre, descripcion, dominio_principal, activo, es_entrenamiento)
    ON target.codigo = source.codigo
    WHEN MATCHED THEN
        UPDATE SET
            nombre = source.nombre,
            descripcion = source.descripcion,
            dominio_principal = source.dominio_principal,
            activo = source.activo,
            es_entrenamiento = source.es_entrenamiento,
            actualizado_utc = SYSUTCDATETIME()
    WHEN NOT MATCHED THEN
        INSERT (codigo, nombre, descripcion, dominio_principal, activo, es_entrenamiento)
        VALUES (source.codigo, source.nombre, source.descripcion, source.dominio_principal, source.activo, source.es_entrenamiento);

    DECLARE @idTenant BIGINT = (
        SELECT id_tenant
        FROM plataforma.tenant
        WHERE codigo = 'SEED'
    );

    DECLARE @idTipoEmpresa SMALLINT = (
        SELECT id_tipo_empresa
        FROM catalogo.tipo_empresa
        WHERE codigo = 'MATRIZ'
    );

    DECLARE @idEstadoEmpresa SMALLINT = (
        SELECT id_estado_empresa
        FROM catalogo.estado_empresa
        WHERE codigo = 'ACTIVA'
    );

    MERGE organizacion.empresa AS target
    USING (VALUES
        (@idTenant, N'SEED-ERP', N'Empresa Seed ERP', N'Empresa Seed ERP S.A.', @idTipoEmpresa, @idEstadoEmpresa, N'GT-SEED-001', 'GTQ', N'America/Guatemala', 1)
    ) AS source (id_tenant, codigo, nombre, nombre_legal, id_tipo_empresa, id_estado_empresa, identificacion_fiscal, moneda_base, zona_horaria, activo)
    ON target.id_tenant = source.id_tenant
       AND target.codigo = source.codigo
    WHEN MATCHED THEN
        UPDATE SET
            nombre = source.nombre,
            nombre_legal = source.nombre_legal,
            id_tipo_empresa = source.id_tipo_empresa,
            id_estado_empresa = source.id_estado_empresa,
            identificacion_fiscal = source.identificacion_fiscal,
            moneda_base = source.moneda_base,
            zona_horaria = source.zona_horaria,
            activo = source.activo,
            actualizado_utc = SYSUTCDATETIME()
    WHEN NOT MATCHED THEN
        INSERT (
            id_tenant, codigo, nombre, nombre_legal,
            id_tipo_empresa, id_estado_empresa,
            identificacion_fiscal, moneda_base, zona_horaria, activo
        )
        VALUES (
            source.id_tenant, source.codigo, source.nombre, source.nombre_legal,
            source.id_tipo_empresa, source.id_estado_empresa,
            source.identificacion_fiscal, source.moneda_base, source.zona_horaria, source.activo
        );

    DECLARE @idEmpresa BIGINT = (
        SELECT id_empresa
        FROM organizacion.empresa
        WHERE id_tenant = @idTenant
          AND codigo = N'SEED-ERP'
    );

    ------------------------------------------------------------
    -- 3) Usuario, credencial y relaciones de contexto
    ------------------------------------------------------------
    DECLARE @idEstadoUsuario SMALLINT = (
        SELECT id_estado_usuario
        FROM catalogo.estado_usuario
        WHERE codigo = 'ACTIVO'
    );

    IF EXISTS (
        SELECT 1
        FROM seguridad.usuario
        WHERE login_normalizado = N'ADMIN.SEED'
    )
    BEGIN
        UPDATE seguridad.usuario
        SET
            codigo = N'SEED-ADMIN',
            login_principal = N'admin.seed',
            login_normalizado = N'ADMIN.SEED',
            nombre = N'Admin',
            apellido = N'Seed',
            nombre_mostrar = N'Admin Seed',
            correo_electronico = N'admin.seed@seed.local',
            correo_normalizado = N'ADMIN.SEED@SEED.LOCAL',
            telefono_movil = N'+50255550000',
            idioma = N'es-GT',
            zona_horaria = N'America/Guatemala',
            id_estado_usuario = @idEstadoUsuario,
            mfa_habilitado = 0,
            requiere_cambio_clave = 0,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE login_normalizado = N'ADMIN.SEED';
    END
    ELSE
    BEGIN
        INSERT INTO seguridad.usuario (
            codigo,
            login_principal,
            login_normalizado,
            nombre,
            apellido,
            nombre_mostrar,
            correo_electronico,
            correo_normalizado,
            telefono_movil,
            idioma,
            zona_horaria,
            id_estado_usuario,
            mfa_habilitado,
            requiere_cambio_clave,
            activo
        )
        VALUES (
            N'SEED-ADMIN',
            N'admin.seed',
            N'ADMIN.SEED',
            N'Admin',
            N'Seed',
            N'Admin Seed',
            N'admin.seed@seed.local',
            N'ADMIN.SEED@SEED.LOCAL',
            N'+50255550000',
            N'es-GT',
            N'America/Guatemala',
            @idEstadoUsuario,
            0,
            0,
            1
        );
    END;

    DECLARE @idUsuario BIGINT = (
        SELECT id_usuario
        FROM seguridad.usuario
        WHERE login_normalizado = N'ADMIN.SEED'
    );

    IF EXISTS (
        SELECT 1
        FROM seguridad.usuario_tenant
        WHERE id_usuario = @idUsuario
          AND id_tenant = @idTenant
    )
    BEGIN
        UPDATE seguridad.usuario_tenant
        SET
            es_administrador_tenant = 1,
            es_cuenta_servicio = 0,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE id_usuario = @idUsuario
          AND id_tenant = @idTenant;
    END
    ELSE
    BEGIN
        INSERT INTO seguridad.usuario_tenant (
            id_usuario,
            id_tenant,
            es_administrador_tenant,
            es_cuenta_servicio,
            activo
        )
        VALUES (
            @idUsuario,
            @idTenant,
            1,
            0,
            1
        );
    END;

    IF EXISTS (
        SELECT 1
        FROM seguridad.usuario_empresa
        WHERE id_usuario = @idUsuario
          AND id_empresa = @idEmpresa
    )
    BEGIN
        UPDATE seguridad.usuario_empresa
        SET
            id_tenant = @idTenant,
            es_empresa_predeterminada = 1,
            puede_operar = 1,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE id_usuario = @idUsuario
          AND id_empresa = @idEmpresa;
    END
    ELSE
    BEGIN
        INSERT INTO seguridad.usuario_empresa (
            id_usuario,
            id_tenant,
            id_empresa,
            es_empresa_predeterminada,
            puede_operar,
            activo
        )
        VALUES (
            @idUsuario,
            @idTenant,
            @idEmpresa,
            1,
            1,
            1
        );
    END;

    DECLARE @saltClave VARBINARY(32) = HASHBYTES('SHA2_256', 'SEED-SALT-ADMIN');
    DECLARE @hashClave VARBINARY(128) = HASHBYTES('SHA2_512', CONVERT(VARBINARY(32), @saltClave) + CONVERT(VARBINARY(4000), 'Admin123!'));

    IF EXISTS (
        SELECT 1
        FROM seguridad.credencial_local_usuario
        WHERE id_usuario = @idUsuario
    )
    BEGIN
        UPDATE seguridad.credencial_local_usuario
        SET
            hash_clave = @hashClave,
            salt_clave = @saltClave,
            algoritmo_clave = 'SHA2_512',
            iteraciones_clave = 100000,
            cambio_clave_utc = SYSUTCDATETIME(),
            debe_cambiar_clave = 0,
            activo = 1
        WHERE id_usuario = @idUsuario;
    END
    ELSE
    BEGIN
        INSERT INTO seguridad.credencial_local_usuario (
            id_usuario,
            hash_clave,
            salt_clave,
            algoritmo_clave,
            iteraciones_clave,
            cambio_clave_utc,
            debe_cambiar_clave,
            activo
        )
        VALUES (
            @idUsuario,
            @hashClave,
            @saltClave,
            'SHA2_512',
            100000,
            SYSUTCDATETIME(),
            0,
            1
        );
    END;

    ------------------------------------------------------------
    -- 4) RBAC basico (rol y permisos)
    ------------------------------------------------------------
    IF EXISTS (
        SELECT 1
        FROM seguridad.rol
        WHERE id_tenant = @idTenant
          AND codigo = N'ADMIN'
    )
    BEGIN
        UPDATE seguridad.rol
        SET
            nombre = N'Administrador Seed',
            descripcion = N'Rol administrador para pruebas de endpoints',
            es_sistema = 0,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE id_tenant = @idTenant
          AND codigo = N'ADMIN';
    END
    ELSE
    BEGIN
        INSERT INTO seguridad.rol (
            id_tenant,
            codigo,
            nombre,
            descripcion,
            es_sistema,
            activo
        )
        VALUES (
            @idTenant,
            N'ADMIN',
            N'Administrador Seed',
            N'Rol administrador para pruebas de endpoints',
            0,
            1
        );
    END;

    DECLARE @idRol BIGINT = (
        SELECT id_rol
        FROM seguridad.rol
        WHERE id_tenant = @idTenant
          AND codigo = N'ADMIN'
    );

    DECLARE @idAlcanceEmpresa SMALLINT = (
        SELECT id_alcance_asignacion
        FROM catalogo.alcance_asignacion
        WHERE codigo = 'EMPRESA'
    );

    IF EXISTS (
        SELECT 1
        FROM seguridad.asignacion_rol_usuario
        WHERE id_usuario = @idUsuario
          AND id_tenant = @idTenant
          AND id_rol = @idRol
          AND id_alcance_asignacion = @idAlcanceEmpresa
          AND id_empresa = @idEmpresa
          AND id_unidad_organizativa IS NULL
          AND id_grupo_empresarial IS NULL
    )
    BEGIN
        UPDATE seguridad.asignacion_rol_usuario
        SET
            fecha_inicio_utc = SYSUTCDATETIME(),
            fecha_fin_utc = NULL,
            concedido_por = @idUsuario,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE id_usuario = @idUsuario
          AND id_tenant = @idTenant
          AND id_rol = @idRol
          AND id_alcance_asignacion = @idAlcanceEmpresa
          AND id_empresa = @idEmpresa
          AND id_unidad_organizativa IS NULL
          AND id_grupo_empresarial IS NULL;
    END
    ELSE
    BEGIN
        INSERT INTO seguridad.asignacion_rol_usuario (
            id_usuario,
            id_tenant,
            id_rol,
            id_alcance_asignacion,
            id_grupo_empresarial,
            id_empresa,
            id_unidad_organizativa,
            fecha_inicio_utc,
            fecha_fin_utc,
            concedido_por,
            activo
        )
        VALUES (
            @idUsuario,
            @idTenant,
            @idRol,
            @idAlcanceEmpresa,
            NULL,
            @idEmpresa,
            NULL,
            SYSUTCDATETIME(),
            NULL,
            @idUsuario,
            1
        );
    END;

    MERGE seguridad.permiso AS target
    USING (VALUES
        (N'SEG.USUARIO.LISTAR', N'SEGURIDAD', N'LISTAR', N'Listar usuarios', N'Permiso para consultar usuarios', 0, 1),
        (N'SEG.USUARIO.CREAR', N'SEGURIDAD', N'CREAR', N'Crear usuarios', N'Permiso para crear usuarios', 1, 1),
        (N'ORG.EMPRESA.LISTAR', N'ORGANIZACION', N'LISTAR', N'Listar empresas', N'Permiso para consultar empresas', 0, 1)
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
            actualizado_utc = SYSUTCDATETIME()
    WHEN NOT MATCHED THEN
        INSERT (codigo, modulo, accion, nombre, descripcion, es_sensible, activo)
        VALUES (source.codigo, source.modulo, source.accion, source.nombre, source.descripcion, source.es_sensible, source.activo);

    ------------------------------------------------------------
    -- 5) Recursos UI dinamicos + mapeo recurso/permisos
    ------------------------------------------------------------
    DECLARE @idTipoMenu SMALLINT = (
        SELECT id_tipo_recurso_ui
        FROM catalogo.tipo_recurso_ui
        WHERE codigo = 'MENU'
    );

    DECLARE @idTipoForm SMALLINT = (
        SELECT id_tipo_recurso_ui
        FROM catalogo.tipo_recurso_ui
        WHERE codigo = 'FORM'
    );

    IF EXISTS (SELECT 1 FROM seguridad.recurso_ui WHERE codigo = N'NAV.HOME')
    BEGIN
        UPDATE seguridad.recurso_ui
        SET
            nombre = N'Inicio',
            id_tipo_recurso_ui = @idTipoMenu,
            ruta = N'/',
            componente = N'Home',
            icono = N'Navigation.Home',
            id_recurso_ui_padre = NULL,
            orden_visual = 1,
            es_visible = 1,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE codigo = N'NAV.HOME';
    END
    ELSE
    BEGIN
        INSERT INTO seguridad.recurso_ui (
            codigo, nombre, id_tipo_recurso_ui, ruta, componente, icono, id_recurso_ui_padre, orden_visual, es_visible, activo
        )
        VALUES (
            N'NAV.HOME', N'Inicio', @idTipoMenu, N'/', N'Home', N'Navigation.Home', NULL, 1, 1, 1
        );
    END;

    DECLARE @idRecursoHome BIGINT = (
        SELECT id_recurso_ui
        FROM seguridad.recurso_ui
        WHERE codigo = N'NAV.HOME'
    );

    IF EXISTS (SELECT 1 FROM seguridad.recurso_ui WHERE codigo = N'NAV.USUARIOS')
    BEGIN
        UPDATE seguridad.recurso_ui
        SET
            nombre = N'Usuarios',
            id_tipo_recurso_ui = @idTipoForm,
            ruta = N'/seguridad/usuarios',
            componente = N'FrmUsuariosBuscar',
            icono = N'BusinessObjects.BOUser',
            id_recurso_ui_padre = @idRecursoHome,
            orden_visual = 10,
            es_visible = 1,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE codigo = N'NAV.USUARIOS';
    END
    ELSE
    BEGIN
        INSERT INTO seguridad.recurso_ui (
            codigo, nombre, id_tipo_recurso_ui, ruta, componente, icono, id_recurso_ui_padre, orden_visual, es_visible, activo
        )
        VALUES (
            N'NAV.USUARIOS', N'Usuarios', @idTipoForm, N'/seguridad/usuarios', N'FrmUsuariosBuscar', N'BusinessObjects.BOUser', @idRecursoHome, 10, 1, 1
        );
    END;

    IF EXISTS (SELECT 1 FROM seguridad.recurso_ui WHERE codigo = N'NAV.EMPRESAS')
    BEGIN
        UPDATE seguridad.recurso_ui
        SET
            nombre = N'Empresas',
            id_tipo_recurso_ui = @idTipoForm,
            ruta = N'/organizacion/empresas',
            componente = N'FrmEmpresasBuscar',
            icono = N'Edit.Edit',
            id_recurso_ui_padre = @idRecursoHome,
            orden_visual = 20,
            es_visible = 1,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE codigo = N'NAV.EMPRESAS';
    END
    ELSE
    BEGIN
        INSERT INTO seguridad.recurso_ui (
            codigo, nombre, id_tipo_recurso_ui, ruta, componente, icono, id_recurso_ui_padre, orden_visual, es_visible, activo
        )
        VALUES (
            N'NAV.EMPRESAS', N'Empresas', @idTipoForm, N'/organizacion/empresas', N'FrmEmpresasBuscar', N'Edit.Edit', @idRecursoHome, 20, 1, 1
        );
    END;

    DECLARE @idPermisoUsuarios INT = (
        SELECT id_permiso
        FROM seguridad.permiso
        WHERE codigo = N'SEG.USUARIO.LISTAR'
    );

    DECLARE @idPermisoEmpresas INT = (
        SELECT id_permiso
        FROM seguridad.permiso
        WHERE codigo = N'ORG.EMPRESA.LISTAR'
    );

    DECLARE @idRecursoUsuarios BIGINT = (
        SELECT id_recurso_ui
        FROM seguridad.recurso_ui
        WHERE codigo = N'NAV.USUARIOS'
    );

    DECLARE @idRecursoEmpresas BIGINT = (
        SELECT id_recurso_ui
        FROM seguridad.recurso_ui
        WHERE codigo = N'NAV.EMPRESAS'
    );

    IF EXISTS (
        SELECT 1
        FROM seguridad.recurso_ui_permiso
        WHERE id_recurso_ui = @idRecursoUsuarios
          AND id_permiso = @idPermisoUsuarios
    )
    BEGIN
        UPDATE seguridad.recurso_ui_permiso
        SET
            activo = 1
        WHERE id_recurso_ui = @idRecursoUsuarios
          AND id_permiso = @idPermisoUsuarios;
    END
    ELSE
    BEGIN
        INSERT INTO seguridad.recurso_ui_permiso (id_recurso_ui, id_permiso, activo)
        VALUES (@idRecursoUsuarios, @idPermisoUsuarios, 1);
    END;

    IF EXISTS (
        SELECT 1
        FROM seguridad.recurso_ui_permiso
        WHERE id_recurso_ui = @idRecursoEmpresas
          AND id_permiso = @idPermisoEmpresas
    )
    BEGIN
        UPDATE seguridad.recurso_ui_permiso
        SET
            activo = 1
        WHERE id_recurso_ui = @idRecursoEmpresas
          AND id_permiso = @idPermisoEmpresas;
    END
    ELSE
    BEGIN
        INSERT INTO seguridad.recurso_ui_permiso (id_recurso_ui, id_permiso, activo)
        VALUES (@idRecursoEmpresas, @idPermisoEmpresas, 1);
    END;

    ------------------------------------------------------------
    -- 6) Sesion de prueba (token hash opaco)
    ------------------------------------------------------------
    DECLARE @tokenHash BINARY(32) = CONVERT(BINARY(32), HASHBYTES('SHA2_256', CONCAT('SEED-TOKEN-', CONVERT(VARCHAR(20), @idUsuario))));
    DECLARE @refreshHash BINARY(32) = CONVERT(BINARY(32), HASHBYTES('SHA2_256', CONCAT('SEED-REFRESH-', CONVERT(VARCHAR(20), @idUsuario))));

    IF EXISTS (
        SELECT 1
        FROM seguridad.sesion_usuario
        WHERE token_hash = @tokenHash
    )
    BEGIN
        UPDATE seguridad.sesion_usuario
        SET
            id_usuario = @idUsuario,
            id_tenant = @idTenant,
            id_empresa = @idEmpresa,
            refresh_hash = @refreshHash,
            origen_autenticacion = 'LOCAL',
            mfa_validado = 1,
            ultima_actividad_utc = SYSUTCDATETIME(),
            expira_absoluta_utc = DATEADD(DAY, 30, SYSUTCDATETIME()),
            ip_origen = N'127.0.0.1',
            agente_usuario = N'SEED',
            huella_dispositivo = N'SEED-DEVICE',
            activo = 1,
            revocada_utc = NULL,
            motivo_revocacion = NULL
        WHERE token_hash = @tokenHash;
    END
    ELSE
    BEGIN
        INSERT INTO seguridad.sesion_usuario (
            id_sesion_usuario,
            id_usuario,
            id_tenant,
            id_empresa,
            token_hash,
            refresh_hash,
            origen_autenticacion,
            mfa_validado,
            creado_utc,
            expira_absoluta_utc,
            ultima_actividad_utc,
            ip_origen,
            agente_usuario,
            huella_dispositivo,
            activo,
            revocada_utc,
            motivo_revocacion
        )
        VALUES (
            NEWID(),
            @idUsuario,
            @idTenant,
            @idEmpresa,
            @tokenHash,
            @refreshHash,
            'LOCAL',
            1,
            SYSUTCDATETIME(),
            DATEADD(DAY, 30, SYSUTCDATETIME()),
            SYSUTCDATETIME(),
            N'127.0.0.1',
            N'SEED',
            N'SEED-DEVICE',
            1,
            NULL,
            NULL
        );
    END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
