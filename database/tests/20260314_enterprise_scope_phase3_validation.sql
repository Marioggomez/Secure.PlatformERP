/* Validacion enterprise phase3: tenant + empresa + user-scope + integridad */
SET NOCOUNT ON;
SET XACT_ABORT ON;
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;

PRINT 'TEST 1: usuario de tenant A no ve tenant B';
DECLARE @uA BIGINT = (
    SELECT TOP(1) ut.id_usuario
    FROM seguridad.usuario_tenant ut
    WHERE ut.activo = 1
    ORDER BY ut.id_usuario
);
DECLARE @tA BIGINT = (
    SELECT TOP(1) ut.id_tenant
    FROM seguridad.usuario_tenant ut
    WHERE ut.id_usuario = @uA
      AND ut.activo = 1
    ORDER BY ut.id_tenant
);
DECLARE @tB BIGINT = (
    SELECT TOP(1) t.id_tenant
    FROM plataforma.tenant t
    WHERE t.id_tenant <> @tA
    ORDER BY t.id_tenant
);
IF @tB IS NOT NULL
AND EXISTS (SELECT 1 FROM seguridad.fn_usuario_empresas_efectivas(@uA, @tB))
    THROW 51400, N'Fallo TEST 1: fuga tenant A -> tenant B.', 1;

PRINT 'TEST 2: usuario con una empresa no ve otras';
DECLARE @uSingle BIGINT = (
    SELECT TOP(1) ue.id_usuario
    FROM seguridad.usuario_empresa ue
    WHERE ue.activo = 1
      AND ue.puede_operar = 1
    GROUP BY ue.id_usuario, ue.id_tenant
    HAVING COUNT(DISTINCT ue.id_empresa) = 1
);
IF @uSingle IS NOT NULL
BEGIN
    DECLARE @tSingle BIGINT = (
        SELECT TOP(1) ue.id_tenant
        FROM seguridad.usuario_empresa ue
        WHERE ue.id_usuario = @uSingle
        ORDER BY ue.id_tenant
    );
    DECLARE @eSingle BIGINT = (
        SELECT TOP(1) ue.id_empresa
        FROM seguridad.usuario_empresa ue
        WHERE ue.id_usuario = @uSingle
          AND ue.id_tenant = @tSingle
          AND ue.activo = 1
          AND ue.puede_operar = 1
        ORDER BY ue.id_empresa
    );

    IF EXISTS (
        SELECT 1
        FROM seguridad.fn_usuario_empresas_efectivas(@uSingle, @tSingle) ef
        WHERE ef.id_empresa <> @eSingle
    )
        THROW 51401, N'Fallo TEST 2: usuario de empresa unica visualiza empresa no autorizada.', 1;
END;

PRINT 'TEST 3: usuario con varias empresas ve solo autorizadas';
DECLARE @uMulti BIGINT = (
    SELECT TOP(1) ue.id_usuario
    FROM seguridad.usuario_empresa ue
    WHERE ue.activo = 1
      AND ue.puede_operar = 1
    GROUP BY ue.id_usuario, ue.id_tenant
    HAVING COUNT(DISTINCT ue.id_empresa) > 1
);
IF @uMulti IS NOT NULL
BEGIN
    DECLARE @tMulti BIGINT = (
        SELECT TOP(1) ue.id_tenant
        FROM seguridad.usuario_empresa ue
        WHERE ue.id_usuario = @uMulti
        ORDER BY ue.id_tenant
    );

    IF EXISTS (
        SELECT 1
        FROM seguridad.fn_usuario_empresas_efectivas(@uMulti, @tMulti) ef
        WHERE NOT EXISTS (
            SELECT 1
            FROM seguridad.usuario_empresa ue
            WHERE ue.id_usuario = @uMulti
              AND ue.id_tenant = @tMulti
              AND ue.id_empresa = ef.id_empresa
              AND ue.activo = 1
              AND ue.puede_operar = 1
        )
    )
        THROW 51402, N'Fallo TEST 3: usuario multiempresa visualiza empresa fuera de asignacion.', 1;
END;

PRINT 'TEST 4: SELF_ONLY ve solo sus registros';
DECLARE @uSelf BIGINT = (
    SELECT TOP(1) ut.id_usuario
    FROM seguridad.usuario_tenant ut
    WHERE ut.activo = 1
      AND ISNULL(ut.es_administrador_tenant, 0) = 0
      AND NOT EXISTS (
          SELECT 1
          FROM seguridad.usuario_scope_usuario usu
          WHERE usu.id_tenant = ut.id_tenant
            AND usu.id_usuario_origen = ut.id_usuario
            AND usu.activo = 1
            AND usu.fecha_inicio_utc <= SYSUTCDATETIME()
            AND (usu.fecha_fin_utc IS NULL OR usu.fecha_fin_utc >= SYSUTCDATETIME())
      )
    ORDER BY ut.id_usuario
);
IF @uSelf IS NOT NULL
BEGIN
    DECLARE @tSelf BIGINT = (
        SELECT TOP(1) id_tenant
        FROM seguridad.usuario_tenant
        WHERE id_usuario = @uSelf
          AND activo = 1
        ORDER BY id_tenant
    );
    DECLARE @eSelf BIGINT = (
        SELECT TOP(1) id_empresa
        FROM seguridad.usuario_empresa
        WHERE id_usuario = @uSelf
          AND id_tenant = @tSelf
          AND activo = 1
          AND puede_operar = 1
        ORDER BY id_empresa
    );

    IF EXISTS (
        SELECT 1
        FROM seguridad.fn_usuario_usuarios_efectivos(@uSelf, @tSelf, @eSelf) f
        WHERE f.id_usuario <> @uSelf
    )
        THROW 51403, N'Fallo TEST 4: SELF_ONLY expone usuarios adicionales.', 1;
END;

PRINT 'TEST 5: EXPLICIT_USER_SCOPE ve solo usuarios autorizados';
DECLARE @uOrigin BIGINT = (
    SELECT TOP(1) ut.id_usuario
    FROM seguridad.usuario_tenant ut
    WHERE ut.activo = 1
      AND ISNULL(ut.es_administrador_tenant, 0) = 0
    ORDER BY ut.id_usuario
);
IF @uOrigin IS NOT NULL
BEGIN
    DECLARE @tOrigin BIGINT = (
        SELECT TOP(1) ut.id_tenant
        FROM seguridad.usuario_tenant ut
        WHERE ut.id_usuario = @uOrigin
          AND ut.activo = 1
        ORDER BY ut.id_tenant
    );
    DECLARE @eOrigin BIGINT = (
        SELECT TOP(1) ue.id_empresa
        FROM seguridad.usuario_empresa ue
        WHERE ue.id_usuario = @uOrigin
          AND ue.id_tenant = @tOrigin
          AND ue.activo = 1
          AND ue.puede_operar = 1
        ORDER BY ue.id_empresa
    );
    DECLARE @uDest BIGINT = (
        SELECT TOP(1) ut.id_usuario
        FROM seguridad.usuario_tenant ut
        WHERE ut.id_tenant = @tOrigin
          AND ut.id_usuario <> @uOrigin
          AND ut.activo = 1
        ORDER BY ut.id_usuario
    );

    IF @uDest IS NOT NULL AND @eOrigin IS NOT NULL
    BEGIN
        DECLARE @id_scope_insertado BIGINT = NULL;

        EXEC seguridad.usp_usuario_scope_usuario_guardar
            @id_tenant = @tOrigin,
            @id_usuario_origen = @uOrigin,
            @id_usuario_destino = @uDest,
            @id_empresa = @eOrigin,
            @activo = 1;

        SET @id_scope_insertado = (
            SELECT TOP(1) usu.id_usuario_scope_usuario
            FROM seguridad.usuario_scope_usuario usu
            WHERE usu.id_tenant = @tOrigin
              AND usu.id_empresa = @eOrigin
              AND usu.id_usuario_origen = @uOrigin
              AND usu.id_usuario_destino = @uDest
              AND usu.activo = 1
            ORDER BY usu.id_usuario_scope_usuario DESC
        );

        IF NOT EXISTS (
            SELECT 1
            FROM seguridad.fn_usuario_usuarios_efectivos(@uOrigin, @tOrigin, @eOrigin) f
            WHERE f.id_usuario = @uDest
        )
            THROW 51404, N'Fallo TEST 5: destino explicito no visible.', 1;

        DELETE FROM seguridad.usuario_scope_usuario
        WHERE id_usuario_scope_usuario = @id_scope_insertado;
    END;
END;

PRINT 'TEST 6: supervisor por unidad ve solo su unidad';
DECLARE @uUnit BIGINT = (
    SELECT TOP(1) usu.id_usuario
    FROM seguridad.usuario_scope_unidad usu
    WHERE usu.activo = 1
    ORDER BY usu.id_usuario
);
IF @uUnit IS NOT NULL
BEGIN
    DECLARE @tUnit BIGINT = (
        SELECT TOP(1) usu.id_tenant
        FROM seguridad.usuario_scope_unidad usu
        WHERE usu.id_usuario = @uUnit
          AND usu.activo = 1
        ORDER BY usu.id_tenant
    );
    DECLARE @eUnit BIGINT = (
        SELECT TOP(1) usu.id_empresa
        FROM seguridad.usuario_scope_unidad usu
        WHERE usu.id_usuario = @uUnit
          AND usu.id_tenant = @tUnit
          AND usu.activo = 1
        ORDER BY usu.id_empresa
    );

    IF EXISTS (
        SELECT 1
        FROM seguridad.fn_usuario_unidades_efectivas(@uUnit, @tUnit, @eUnit) fu
        WHERE NOT EXISTS (
            SELECT 1
            FROM seguridad.usuario_scope_unidad su
            WHERE su.id_usuario = @uUnit
              AND su.id_tenant = @tUnit
              AND su.id_empresa = @eUnit
              AND su.id_unidad_organizativa = fu.id_unidad_organizativa
              AND su.activo = 1
              AND su.fecha_inicio_utc <= SYSUTCDATETIME()
              AND (su.fecha_fin_utc IS NULL OR su.fecha_fin_utc >= SYSUTCDATETIME())
        )
    )
        THROW 51405, N'Fallo TEST 6: scope por unidad inconsistente.', 1;
END;

PRINT 'TEST 7: admin tenant consistente';
DECLARE @uAdmin BIGINT = (
    SELECT TOP(1) ut.id_usuario
    FROM seguridad.usuario_tenant ut
    WHERE ut.activo = 1
      AND ut.es_administrador_tenant = 1
    ORDER BY ut.id_usuario
);
IF @uAdmin IS NOT NULL
BEGIN
    DECLARE @tAdmin BIGINT = (
        SELECT TOP(1) ut.id_tenant
        FROM seguridad.usuario_tenant ut
        WHERE ut.id_usuario = @uAdmin
          AND ut.activo = 1
        ORDER BY ut.id_tenant
    );
    DECLARE @eAdmin BIGINT = (
        SELECT TOP(1) ue.id_empresa
        FROM seguridad.usuario_empresa ue
        WHERE ue.id_usuario = @uAdmin
          AND ue.id_tenant = @tAdmin
          AND ue.activo = 1
          AND ue.puede_operar = 1
        ORDER BY ue.id_empresa
    );

    IF EXISTS (
        SELECT 1
        FROM seguridad.fn_usuario_usuarios_efectivos(@uAdmin, @tAdmin, @eAdmin) f
        WHERE NOT EXISTS (
            SELECT 1
            FROM seguridad.usuario_tenant ut
            WHERE ut.id_usuario = f.id_usuario
              AND ut.id_tenant = @tAdmin
              AND ut.activo = 1
        )
    )
        THROW 51406, N'Fallo TEST 7: admin ve usuarios fuera de su tenant.', 1;
END;

PRINT 'TEST 8: tablas auxiliares respetan estrategia formal';
IF EXISTS (
    SELECT 1
    FROM seguridad.entidad_alcance_dato e
    WHERE e.codigo_entidad IN
    (
        N'ACT.COMENTARIO',
        N'ACT.COMENTARIO_MENCION',
        N'DOC.DOCUMENTO',
        N'DOC.DOCUMENTO_ENTIDAD',
        N'DOC.DOCUMENTO_VERSION',
        N'DOC.DOCUMENTO_FIRMA',
        N'DOC.DOCUMENTO_WORKFLOW_PASO',
        N'ETQ.ENTIDAD'
    )
      AND (e.modo_scope <> N'POR_ENTIDAD_RAIZ' OR e.codigo_entidad_raiz IS NULL)
)
    THROW 51407, N'Fallo TEST 8: metadata auxiliar sin estrategia formal POR_ENTIDAD_RAIZ.', 1;

PRINT 'TEST 9: no regresion de integridad';
IF EXISTS (
    SELECT 1
    FROM sys.foreign_keys fk
    WHERE fk.parent_object_id = OBJECT_ID(N'seguridad.usuario_scope_usuario')
      AND (fk.is_disabled = 1 OR fk.is_not_trusted = 1)
)
    THROW 51408, N'Fallo TEST 9: FK de usuario_scope_usuario deshabilitada o no confiable.', 1;

PRINT 'TEST 10: scripts idempotentes (gate)';
PRINT 'Validar re-ejecucion de migracion phase3 en paso externo con exit code 0.';

PRINT 'OK phase3';
