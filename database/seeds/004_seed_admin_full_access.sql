/*
    Seed para otorgar acceso total a admin.seed (tenant/empresa/roles/menu).
    Autor: Mario Gomez
*/
SET NOCOUNT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @now DATETIME2(3) = SYSUTCDATETIME();
    DECLARE @idUsuarioAdmin BIGINT = (
        SELECT TOP (1) u.id_usuario
        FROM seguridad.usuario u
        WHERE u.login_normalizado = N'ADMIN.SEED'
           OR UPPER(u.login_principal) = N'ADMIN.SEED'
        ORDER BY u.id_usuario
    );

    IF @idUsuarioAdmin IS NULL
    BEGIN
        THROW 50011, 'No existe el usuario admin.seed para aplicar full-access.', 1;
    END;

    ------------------------------------------------------------
    -- 1) Usuario admin activo y administrador de tenant
    ------------------------------------------------------------
    UPDATE seguridad.usuario
    SET
        activo = 1,
        mfa_habilitado = 1,
        actualizado_utc = @now
    WHERE id_usuario = @idUsuarioAdmin;

    INSERT INTO seguridad.usuario_tenant
    (
        id_usuario,
        id_tenant,
        es_administrador_tenant,
        es_cuenta_servicio,
        activo,
        creado_utc
    )
    SELECT
        @idUsuarioAdmin,
        t.id_tenant,
        1,
        0,
        1,
        @now
    FROM plataforma.tenant t
    WHERE t.activo = 1
      AND NOT EXISTS
      (
          SELECT 1
          FROM seguridad.usuario_tenant ut
          WHERE ut.id_usuario = @idUsuarioAdmin
            AND ut.id_tenant = t.id_tenant
      );

    UPDATE ut
    SET
        es_administrador_tenant = 1,
        es_cuenta_servicio = 0,
        activo = 1,
        actualizado_utc = @now
    FROM seguridad.usuario_tenant ut
    WHERE ut.id_usuario = @idUsuarioAdmin;

    ------------------------------------------------------------
    -- 2) Acceso a todas las empresas activas del tenant
    ------------------------------------------------------------
    ;WITH empresas_target AS
    (
        SELECT
            ut.id_tenant,
            e.id_empresa,
            ROW_NUMBER() OVER (
                PARTITION BY ut.id_tenant
                ORDER BY e.id_empresa
            ) AS rn
        FROM seguridad.usuario_tenant ut
        INNER JOIN organizacion.empresa e
            ON e.id_tenant = ut.id_tenant
           AND e.activo = 1
        WHERE ut.id_usuario = @idUsuarioAdmin
          AND ut.activo = 1
    )
    MERGE seguridad.usuario_empresa AS target
    USING empresas_target AS source
    ON target.id_usuario = @idUsuarioAdmin
       AND target.id_empresa = source.id_empresa
    WHEN MATCHED THEN
        UPDATE SET
            id_tenant = source.id_tenant,
            es_empresa_predeterminada = CASE WHEN source.rn = 1 THEN 1 ELSE target.es_empresa_predeterminada END,
            puede_operar = 1,
            fecha_inicio_utc = CASE
                WHEN target.fecha_inicio_utc IS NULL THEN @now
                ELSE target.fecha_inicio_utc
            END,
            fecha_fin_utc = NULL,
            activo = 1,
            actualizado_utc = @now
    WHEN NOT MATCHED THEN
        INSERT
        (
            id_usuario,
            id_tenant,
            id_empresa,
            es_empresa_predeterminada,
            puede_operar,
            fecha_inicio_utc,
            fecha_fin_utc,
            activo,
            creado_utc
        )
        VALUES
        (
            @idUsuarioAdmin,
            source.id_tenant,
            source.id_empresa,
            CASE WHEN source.rn = 1 THEN 1 ELSE 0 END,
            1,
            @now,
            NULL,
            1,
            @now
        );

    ;WITH defaults_cte AS
    (
        SELECT
            ue.id_usuario_empresa,
            ROW_NUMBER() OVER
            (
                PARTITION BY ue.id_tenant
                ORDER BY
                    CASE WHEN ue.es_empresa_predeterminada = 1 THEN 0 ELSE 1 END,
                    ue.id_usuario_empresa
            ) AS rn
        FROM seguridad.usuario_empresa ue
        WHERE ue.id_usuario = @idUsuarioAdmin
          AND ue.activo = 1
          AND ue.puede_operar = 1
          AND ue.fecha_inicio_utc <= @now
          AND (ue.fecha_fin_utc IS NULL OR ue.fecha_fin_utc >= @now)
    )
    UPDATE ue
    SET
        es_empresa_predeterminada = CASE WHEN d.rn = 1 THEN 1 ELSE 0 END,
        actualizado_utc = @now
    FROM seguridad.usuario_empresa ue
    INNER JOIN defaults_cte d
        ON d.id_usuario_empresa = ue.id_usuario_empresa;

    ------------------------------------------------------------
    -- 3) Scope sin restricciones para admin
    ------------------------------------------------------------
    DELETE FROM seguridad.usuario_scope_empresa
    WHERE id_usuario = @idUsuarioAdmin;

    DELETE FROM seguridad.usuario_scope_unidad
    WHERE id_usuario = @idUsuarioAdmin;

    ------------------------------------------------------------
    -- 4) Asignar todos los roles activos por tenant
    ------------------------------------------------------------
    DECLARE @idAlcanceEmpresa SMALLINT = (
        SELECT TOP (1) id_alcance_asignacion
        FROM catalogo.alcance_asignacion
        WHERE codigo = 'EMPRESA'
        ORDER BY id_alcance_asignacion
    );

    IF @idAlcanceEmpresa IS NULL
    BEGIN
        SET @idAlcanceEmpresa = (
            SELECT TOP (1) id_alcance_asignacion
            FROM catalogo.alcance_asignacion
            ORDER BY id_alcance_asignacion
        );
    END;

    IF @idAlcanceEmpresa IS NULL
    BEGIN
        THROW 50012, 'No existe catalogo.alcance_asignacion para roles admin.', 1;
    END;

    UPDATE seguridad.asignacion_rol_usuario
    SET
        activo = 1,
        fecha_fin_utc = NULL,
        actualizado_utc = @now
    WHERE id_usuario = @idUsuarioAdmin;

    ;WITH empresa_default_tenant AS
    (
        SELECT
            ue.id_tenant,
            ue.id_empresa,
            ROW_NUMBER() OVER
            (
                PARTITION BY ue.id_tenant
                ORDER BY
                    CASE WHEN ue.es_empresa_predeterminada = 1 THEN 0 ELSE 1 END,
                    ue.id_usuario_empresa
            ) AS rn
        FROM seguridad.usuario_empresa ue
        WHERE ue.id_usuario = @idUsuarioAdmin
          AND ue.activo = 1
          AND ue.puede_operar = 1
    ),
    target_roles AS
    (
        SELECT
            ut.id_tenant,
            r.id_rol,
            edt.id_empresa
        FROM seguridad.usuario_tenant ut
        INNER JOIN seguridad.rol r
            ON r.id_tenant = ut.id_tenant
           AND r.activo = 1
        INNER JOIN empresa_default_tenant edt
            ON edt.id_tenant = ut.id_tenant
           AND edt.rn = 1
        WHERE ut.id_usuario = @idUsuarioAdmin
          AND ut.activo = 1
    )
    INSERT INTO seguridad.asignacion_rol_usuario
    (
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
        activo,
        creado_utc,
        actualizado_utc
    )
    SELECT
        @idUsuarioAdmin,
        tr.id_tenant,
        tr.id_rol,
        @idAlcanceEmpresa,
        NULL,
        tr.id_empresa,
        NULL,
        @now,
        NULL,
        @idUsuarioAdmin,
        1,
        @now,
        @now
    FROM target_roles tr
    WHERE NOT EXISTS
    (
        SELECT 1
        FROM seguridad.asignacion_rol_usuario aru
        WHERE aru.id_usuario = @idUsuarioAdmin
          AND aru.id_tenant = tr.id_tenant
          AND aru.id_rol = tr.id_rol
          AND ISNULL(aru.id_empresa, 0) = ISNULL(tr.id_empresa, 0)
          AND aru.id_grupo_empresarial IS NULL
          AND aru.id_unidad_organizativa IS NULL
    );

    ------------------------------------------------------------
    -- 5) Asegurar deberes para todos los permisos por tenant
    ------------------------------------------------------------
    ;WITH tenants_admin AS
    (
        SELECT DISTINCT ut.id_tenant
        FROM seguridad.usuario_tenant ut
        WHERE ut.id_usuario = @idUsuarioAdmin
          AND ut.activo = 1
    ),
    deberes_target AS
    (
        SELECT
            ta.id_tenant,
            p.codigo,
            p.nombre,
            p.descripcion
        FROM tenants_admin ta
        CROSS JOIN seguridad.permiso p
        WHERE p.activo = 1
    )
    MERGE seguridad.deber AS target
    USING deberes_target AS source
    ON target.id_tenant = source.id_tenant
       AND target.codigo = source.codigo
    WHEN MATCHED THEN
        UPDATE SET
            nombre = COALESCE(source.nombre, source.codigo),
            descripcion = COALESCE(source.descripcion, CONCAT(N'Deber para permiso ', source.codigo)),
            activo = 1,
            actualizado_utc = @now
    WHEN NOT MATCHED THEN
        INSERT
        (
            id_tenant,
            codigo,
            nombre,
            descripcion,
            es_sistema,
            activo,
            creado_utc,
            actualizado_utc
        )
        VALUES
        (
            source.id_tenant,
            source.codigo,
            COALESCE(source.nombre, source.codigo),
            COALESCE(source.descripcion, CONCAT(N'Deber para permiso ', source.codigo)),
            1,
            1,
            @now,
            @now
        );

    ------------------------------------------------------------
    -- 6) Conceder todos los deberes a todos los roles de admin
    ------------------------------------------------------------
    ;WITH roles_admin AS
    (
        SELECT DISTINCT
            aru.id_tenant,
            aru.id_rol
        FROM seguridad.asignacion_rol_usuario aru
        WHERE aru.id_usuario = @idUsuarioAdmin
          AND aru.activo = 1
          AND aru.fecha_inicio_utc <= @now
          AND (aru.fecha_fin_utc IS NULL OR aru.fecha_fin_utc >= @now)
    ),
    deberes_admin AS
    (
        SELECT
            ra.id_rol,
            d.id_deber
        FROM roles_admin ra
        INNER JOIN seguridad.deber d
            ON d.id_tenant = ra.id_tenant
           AND d.activo = 1
    )
    MERGE seguridad.rol_deber AS target
    USING deberes_admin AS source
    ON target.id_rol = source.id_rol
       AND target.id_deber = source.id_deber
    WHEN MATCHED THEN
        UPDATE SET
            activo = 1,
            actualizado_utc = @now
    WHEN NOT MATCHED THEN
        INSERT
        (
            id_rol,
            id_deber,
            activo,
            creado_utc,
            actualizado_utc
        )
        VALUES
        (
            source.id_rol,
            source.id_deber,
            1,
            @now,
            @now
        );

    ------------------------------------------------------------
    -- 7) Habilitar mapeos menu-permiso existentes
    ------------------------------------------------------------
    UPDATE seguridad.recurso_ui_permiso
    SET
        activo = 1
    WHERE activo = 0;

    ------------------------------------------------------------
    -- 8) Recurso UI y permiso explicito para Centro IAM
    ------------------------------------------------------------
    MERGE seguridad.permiso AS target
    USING
    (
        SELECT
            N'SEG.IAM.ACCESO' AS codigo,
            N'SEGURIDAD' AS modulo,
            N'ACCESO' AS accion,
            N'Acceso Centro IAM' AS nombre,
            N'Permite abrir y operar FrmIamAdminCenter.' AS descripcion,
            CAST(1 AS bit) AS es_sensible,
            CAST(1 AS bit) AS activo
    ) AS source
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
        INSERT (codigo, modulo, accion, nombre, descripcion, es_sensible, activo, creado_utc, actualizado_utc)
        VALUES (source.codigo, source.modulo, source.accion, source.nombre, source.descripcion, source.es_sensible, source.activo, @now, @now);

    DECLARE @idPermisoIam INT = (
        SELECT TOP (1) id_permiso
        FROM seguridad.permiso
        WHERE codigo = N'SEG.IAM.ACCESO'
    );

    ;WITH tenants_admin AS
    (
        SELECT DISTINCT id_tenant
        FROM seguridad.usuario_tenant
        WHERE id_usuario = @idUsuarioAdmin
          AND activo = 1
    )
    MERGE seguridad.deber AS target
    USING
    (
        SELECT
            ta.id_tenant,
            N'SEG.IAM.ACCESO' AS codigo,
            N'Acceso Centro IAM' AS nombre,
            N'Deber para abrir y administrar el Centro IAM.' AS descripcion
        FROM tenants_admin ta
    ) AS source
    ON target.id_tenant = source.id_tenant
       AND target.codigo = source.codigo
    WHEN MATCHED THEN
        UPDATE SET
            nombre = source.nombre,
            descripcion = source.descripcion,
            activo = 1,
            actualizado_utc = @now
    WHEN NOT MATCHED THEN
        INSERT (id_tenant, codigo, nombre, descripcion, es_sistema, activo, creado_utc, actualizado_utc)
        VALUES (source.id_tenant, source.codigo, source.nombre, source.descripcion, 1, 1, @now, @now);

    ;WITH tenants_admin AS
    (
        SELECT DISTINCT id_tenant
        FROM seguridad.usuario_tenant
        WHERE id_usuario = @idUsuarioAdmin
          AND activo = 1
    )
    MERGE seguridad.privilegio AS target
    USING
    (
        SELECT
            ta.id_tenant,
            N'SEG.IAM.ACCESO' AS codigo,
            N'Privilegio Centro IAM' AS nombre,
            N'Privilegio enlazado al permiso de Centro IAM.' AS descripcion
        FROM tenants_admin ta
    ) AS source
    ON target.id_tenant = source.id_tenant
       AND target.codigo = source.codigo
    WHEN MATCHED THEN
        UPDATE SET
            nombre = source.nombre,
            descripcion = source.descripcion,
            activo = 1,
            actualizado_utc = @now
    WHEN NOT MATCHED THEN
        INSERT (id_tenant, codigo, nombre, descripcion, es_sistema, activo, creado_utc, actualizado_utc)
        VALUES (source.id_tenant, source.codigo, source.nombre, source.descripcion, 1, 1, @now, @now);

    ;WITH map_cte AS
    (
        SELECT
            d.id_deber,
            p.id_privilegio
        FROM seguridad.deber d
        INNER JOIN seguridad.privilegio p
            ON p.id_tenant = d.id_tenant
           AND p.codigo = d.codigo
        WHERE d.codigo = N'SEG.IAM.ACCESO'
          AND p.codigo = N'SEG.IAM.ACCESO'
    )
    MERGE seguridad.deber_privilegio AS target
    USING map_cte AS source
    ON target.id_deber = source.id_deber
       AND target.id_privilegio = source.id_privilegio
    WHEN MATCHED THEN
        UPDATE SET
            activo = 1,
            actualizado_utc = @now
    WHEN NOT MATCHED THEN
        INSERT (id_deber, id_privilegio, activo, creado_utc, actualizado_utc)
        VALUES (source.id_deber, source.id_privilegio, 1, @now, @now);

    ;WITH map_cte AS
    (
        SELECT DISTINCT
            p.id_privilegio,
            @idPermisoIam AS id_permiso
        FROM seguridad.privilegio p
        WHERE p.codigo = N'SEG.IAM.ACCESO'
          AND p.activo = 1
    )
    MERGE seguridad.privilegio_permiso AS target
    USING map_cte AS source
    ON target.id_privilegio = source.id_privilegio
       AND target.id_permiso = source.id_permiso
    WHEN MATCHED THEN
        UPDATE SET
            activo = 1,
            actualizado_utc = @now
    WHEN NOT MATCHED THEN
        INSERT (id_privilegio, id_permiso, activo, creado_utc, actualizado_utc)
        VALUES (source.id_privilegio, source.id_permiso, 1, @now, @now);

    DECLARE @idTipoMenu SMALLINT = (
        SELECT TOP (1) id_tipo_recurso_ui
        FROM catalogo.tipo_recurso_ui
        WHERE codigo = 'MENU'
    );
    DECLARE @idTipoForm SMALLINT = (
        SELECT TOP (1) id_tipo_recurso_ui
        FROM catalogo.tipo_recurso_ui
        WHERE codigo = 'FORM'
    );

    IF @idTipoMenu IS NULL OR @idTipoForm IS NULL
    BEGIN
        THROW 50013, 'No existe catalogo.tipo_recurso_ui MENU/FORM para sembrar Centro IAM.', 1;
    END;

    MERGE seguridad.recurso_ui AS target
    USING
    (
        SELECT
            N'NAV.SEGURIDAD' AS codigo,
            N'Seguridad' AS nombre,
            @idTipoMenu AS id_tipo_recurso_ui,
            N'/seguridad' AS ruta,
            N'FrmIamAdminCenter' AS componente,
            N'BusinessObjects.BOUser' AS icono,
            CAST(NULL AS BIGINT) AS id_recurso_ui_padre,
            CAST(10 AS INT) AS orden_visual,
            CAST(1 AS bit) AS es_visible,
            CAST(1 AS bit) AS activo
    ) AS source
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
        INSERT (codigo, nombre, id_tipo_recurso_ui, ruta, componente, icono, id_recurso_ui_padre, orden_visual, es_visible, activo, creado_utc, actualizado_utc)
        VALUES (source.codigo, source.nombre, source.id_tipo_recurso_ui, source.ruta, source.componente, source.icono, source.id_recurso_ui_padre, source.orden_visual, source.es_visible, source.activo, @now, @now);

    DECLARE @idRecursoSeguridad BIGINT = (
        SELECT TOP (1) id_recurso_ui
        FROM seguridad.recurso_ui
        WHERE codigo = N'NAV.SEGURIDAD'
    );

    MERGE seguridad.recurso_ui AS target
    USING
    (
        SELECT
            N'NAV.SEGURIDAD.IAM' AS codigo,
            N'Centro IAM' AS nombre,
            @idTipoForm AS id_tipo_recurso_ui,
            N'/seguridad/iam' AS ruta,
            N'FrmIamAdminCenter' AS componente,
            N'BusinessObjects.BOUser' AS icono,
            @idRecursoSeguridad AS id_recurso_ui_padre,
            CAST(11 AS INT) AS orden_visual,
            CAST(1 AS bit) AS es_visible,
            CAST(1 AS bit) AS activo
    ) AS source
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
        INSERT (codigo, nombre, id_tipo_recurso_ui, ruta, componente, icono, id_recurso_ui_padre, orden_visual, es_visible, activo, creado_utc, actualizado_utc)
        VALUES (source.codigo, source.nombre, source.id_tipo_recurso_ui, source.ruta, source.componente, source.icono, source.id_recurso_ui_padre, source.orden_visual, source.es_visible, source.activo, @now, @now);

    DECLARE @idRecursoIam BIGINT = (
        SELECT TOP (1) id_recurso_ui
        FROM seguridad.recurso_ui
        WHERE codigo = N'NAV.SEGURIDAD.IAM'
    );

    MERGE seguridad.recurso_ui_permiso AS target
    USING
    (
        SELECT @idRecursoIam AS id_recurso_ui, @idPermisoIam AS id_permiso
    ) AS source
    ON target.id_recurso_ui = source.id_recurso_ui
       AND target.id_permiso = source.id_permiso
    WHEN MATCHED THEN
        UPDATE SET activo = 1
    WHEN NOT MATCHED THEN
        INSERT (id_recurso_ui, id_permiso, activo, creado_utc)
        VALUES (source.id_recurso_ui, source.id_permiso, 1, @now);

    ;WITH roles_admin AS
    (
        SELECT DISTINCT aru.id_rol
        FROM seguridad.asignacion_rol_usuario aru
        WHERE aru.id_usuario = @idUsuarioAdmin
          AND aru.activo = 1
          AND aru.fecha_inicio_utc <= @now
          AND (aru.fecha_fin_utc IS NULL OR aru.fecha_fin_utc >= @now)
    ),
    deber_iam AS
    (
        SELECT id_deber
        FROM seguridad.deber
        WHERE codigo = N'SEG.IAM.ACCESO'
          AND activo = 1
    )
    MERGE seguridad.rol_deber AS target
    USING
    (
        SELECT ra.id_rol, di.id_deber
        FROM roles_admin ra
        CROSS JOIN deber_iam di
    ) AS source
    ON target.id_rol = source.id_rol
       AND target.id_deber = source.id_deber
    WHEN MATCHED THEN
        UPDATE SET
            activo = 1,
            actualizado_utc = @now
    WHEN NOT MATCHED THEN
        INSERT (id_rol, id_deber, activo, creado_utc, actualizado_utc)
        VALUES (source.id_rol, source.id_deber, 1, @now, @now);

    COMMIT TRANSACTION;
    PRINT 'Seed full-access aplicado a admin.seed.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
