/*
  Validacion: enterprise scope hardening
  Ejecutar en entorno de QA con datos seed.
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;

PRINT 'TEST 1: usuario de tenant A no ve tenant B';
DECLARE @uA BIGINT = (SELECT TOP(1) ut.id_usuario FROM seguridad.usuario_tenant ut ORDER BY ut.id_usuario);
DECLARE @tA BIGINT = (SELECT TOP(1) ut.id_tenant FROM seguridad.usuario_tenant ut WHERE ut.id_usuario = @uA ORDER BY ut.id_tenant);
DECLARE @tB BIGINT = (SELECT TOP(1) t.id_tenant FROM plataforma.tenant t WHERE t.id_tenant <> @tA ORDER BY t.id_tenant);
IF @tB IS NOT NULL
BEGIN
    IF EXISTS (SELECT 1 FROM seguridad.fn_usuario_empresas_efectivas(@uA, @tB))
        THROW 51200, N'Fallo TEST 1: hay fuga tenant->tenant.', 1;
END;

PRINT 'TEST 2: usuario con empresa unica no ve otras';
DECLARE @uSingle BIGINT = (
    SELECT TOP(1) ue.id_usuario
    FROM seguridad.usuario_empresa ue
    WHERE ue.activo = 1 AND ue.puede_operar = 1
    GROUP BY ue.id_usuario, ue.id_tenant
    HAVING COUNT(DISTINCT ue.id_empresa) = 1
    ORDER BY ue.id_usuario
);
IF @uSingle IS NOT NULL
BEGIN
    DECLARE @tenantSingle BIGINT = (SELECT TOP(1) id_tenant FROM seguridad.usuario_empresa WHERE id_usuario = @uSingle ORDER BY id_tenant);
    DECLARE @empresaSingle BIGINT = (SELECT TOP(1) id_empresa FROM seguridad.usuario_empresa WHERE id_usuario = @uSingle AND id_tenant = @tenantSingle AND activo = 1 AND puede_operar = 1 ORDER BY id_empresa);

    IF EXISTS (
        SELECT 1
        FROM seguridad.fn_usuario_empresas_efectivas(@uSingle, @tenantSingle) x
        WHERE x.id_empresa <> @empresaSingle
    )
        THROW 51201, N'Fallo TEST 2: usuario con empresa unica ve empresas no asignadas.', 1;
END;

PRINT 'TEST 3: usuario con multiples empresas ve solo asignadas';
DECLARE @uMulti BIGINT = (
    SELECT TOP(1) ue.id_usuario
    FROM seguridad.usuario_empresa ue
    WHERE ue.activo = 1 AND ue.puede_operar = 1
    GROUP BY ue.id_usuario, ue.id_tenant
    HAVING COUNT(DISTINCT ue.id_empresa) > 1
    ORDER BY ue.id_usuario
);
IF @uMulti IS NOT NULL
BEGIN
    DECLARE @tenantMulti BIGINT = (SELECT TOP(1) id_tenant FROM seguridad.usuario_empresa WHERE id_usuario = @uMulti ORDER BY id_tenant);

    IF EXISTS (
        SELECT 1
        FROM seguridad.fn_usuario_empresas_efectivas(@uMulti, @tenantMulti) x
        WHERE NOT EXISTS (
            SELECT 1
            FROM seguridad.usuario_empresa ue
            WHERE ue.id_usuario = @uMulti
              AND ue.id_tenant = @tenantMulti
              AND ue.id_empresa = x.id_empresa
              AND ue.activo = 1
              AND ue.puede_operar = 1
        )
    )
        THROW 51202, N'Fallo TEST 3: aparecen empresas fuera de asignacion operable.', 1;
END;

PRINT 'TEST 4: scope por unidad';
DECLARE @uUnidad BIGINT = (SELECT TOP(1) id_usuario FROM seguridad.usuario_scope_unidad ORDER BY id_usuario);
IF @uUnidad IS NOT NULL
BEGIN
    DECLARE @tenantU BIGINT = (SELECT TOP(1) id_tenant FROM seguridad.usuario_scope_unidad WHERE id_usuario = @uUnidad ORDER BY id_tenant);
    DECLARE @empresaU BIGINT = (SELECT TOP(1) id_empresa FROM seguridad.usuario_scope_unidad WHERE id_usuario = @uUnidad ORDER BY id_empresa);

    IF EXISTS (
        SELECT 1
        FROM seguridad.fn_usuario_unidades_efectivas(@uUnidad, @tenantU, @empresaU) x
        WHERE NOT EXISTS (
            SELECT 1
            FROM seguridad.usuario_scope_unidad su
            WHERE su.id_usuario = @uUnidad
              AND su.id_tenant = @tenantU
              AND su.id_empresa = @empresaU
              AND su.id_unidad_organizativa = x.id_unidad_organizativa
        )
    )
        THROW 51203, N'Fallo TEST 4: unidades fuera de alcance explicito.', 1;
END;

PRINT 'TEST 5: usuario sin scope efectivo no ve nada';
DECLARE @uNoScope BIGINT = (
    SELECT TOP(1) u.id_usuario
    FROM seguridad.usuario u
    WHERE NOT EXISTS (SELECT 1 FROM seguridad.usuario_empresa ue WHERE ue.id_usuario = u.id_usuario AND ue.activo = 1 AND ue.puede_operar = 1)
      AND EXISTS (SELECT 1 FROM seguridad.usuario_tenant ut WHERE ut.id_usuario = u.id_usuario AND ut.activo = 1)
    ORDER BY u.id_usuario
);
IF @uNoScope IS NOT NULL
BEGIN
    DECLARE @tNoScope BIGINT = (SELECT TOP(1) id_tenant FROM seguridad.usuario_tenant WHERE id_usuario = @uNoScope AND activo = 1 ORDER BY id_tenant);
    IF EXISTS (SELECT 1 FROM seguridad.fn_usuario_empresas_efectivas(@uNoScope, @tNoScope))
        THROW 51204, N'Fallo TEST 5: usuario sin scope efectivo puede ver empresas.', 1;
END;

PRINT 'TEST 6: idempotencia de script';
PRINT 'Ejecutar migracion UP dos veces y verificar que no falle por objetos existentes.';

PRINT 'TEST 7: verificacion de fallback inseguro eliminado en pre-auth';
IF EXISTS (
    SELECT 1
    FROM sys.sql_modules m
    INNER JOIN sys.objects o ON o.object_id = m.object_id
    INNER JOIN sys.schemas s ON s.schema_id = o.schema_id
    WHERE s.name = N'seguridad'
      AND o.name = N'usp_auth_obtener_usuario_para_autenticacion'
      AND m.definition LIKE '%COALESCE(ue.id_empresa, emp.id_empresa)%'
)
    THROW 51205, N'Fallo TEST 7: aun existe fallback de empresa por tenant.', 1;

PRINT 'TEST 8: SESSION_CONTEXT valido para scope de sesion';
-- Requiere establecer contexto previamente desde aplicacion o sesion de prueba.
-- EXEC sys.sp_set_session_context @key = N'id_usuario', @value = 1;
-- EXEC sys.sp_set_session_context @key = N'id_tenant', @value = 1;
-- EXEC sys.sp_set_session_context @key = N'id_empresa', @value = 1;
-- EXEC seguridad.usp_scope_validar_sesion;

PRINT 'TEST 9: integridad FK compuesta';
IF EXISTS (
    SELECT 1
    FROM sys.foreign_keys
    WHERE name IN
    (
        N'FK_seguridad_usuario_scope_empresa_empresa_scope',
        N'FK_seguridad_usuario_scope_unidad_empresa_scope',
        N'FK_tercero_tercero_empresa_empresa_scope',
        N'FK_cumplimiento_accion_instancia_aprobacion_scope'
    )
      AND is_disabled = 1
)
    THROW 51206, N'Fallo TEST 9: FK compuesta deshabilitada.', 1;

PRINT 'TEST 10: smoke de capa canonica';
SELECT TOP (20) * FROM seguridad.vw_usuario_scope_efectivo ORDER BY id_usuario, id_tenant, id_empresa;

PRINT 'OK - Suite de validacion ejecutada.';
