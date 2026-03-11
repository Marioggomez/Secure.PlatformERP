/*
    Stored procedures IAM para flujo real de autenticacion.
    Autor: Mario Gomez.
*/

CREATE OR ALTER PROCEDURE seguridad.usp_auth_obtener_usuario_para_autenticacion
    @tenant_codigo NVARCHAR(50),
    @identificador NVARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @identificador_normalizado NVARCHAR(250) = UPPER(LTRIM(RTRIM(@identificador)));

    SELECT TOP (1)
        u.id_usuario,
        t.id_tenant,
        COALESCE(ue.id_empresa, emp.id_empresa) AS id_empresa,
        t.codigo AS tenant_codigo,
        u.login_principal,
        u.nombre_mostrar,
        u.correo_electronico,
        u.mfa_habilitado,
        u.requiere_cambio_clave,
        u.id_estado_usuario,
        u.activo AS activo_usuario,
        c.hash_clave,
        c.salt_clave,
        c.algoritmo_clave,
        c.iteraciones_clave,
        c.activo AS activo_credencial
    FROM plataforma.tenant t
    INNER JOIN seguridad.usuario_tenant ut
        ON ut.id_tenant = t.id_tenant
       AND ut.activo = 1
    INNER JOIN seguridad.usuario u
        ON u.id_usuario = ut.id_usuario
    INNER JOIN seguridad.credencial_local_usuario c
        ON c.id_usuario = u.id_usuario
    OUTER APPLY (
        SELECT TOP (1)
            ue1.id_empresa
        FROM seguridad.usuario_empresa ue1
        WHERE ue1.id_usuario = u.id_usuario
          AND ue1.id_tenant = t.id_tenant
          AND ue1.activo = 1
          AND ue1.puede_operar = 1
        ORDER BY
            CASE WHEN ue1.es_empresa_predeterminada = 1 THEN 0 ELSE 1 END,
            ue1.id_usuario_empresa
    ) ue
    OUTER APPLY (
        SELECT TOP (1)
            e.id_empresa
        FROM organizacion.empresa e
        WHERE e.id_tenant = t.id_tenant
          AND e.activo = 1
        ORDER BY e.id_empresa
    ) emp
    WHERE t.codigo = @tenant_codigo
      AND t.activo = 1
      AND (
            u.login_normalizado = @identificador_normalizado
         OR u.correo_normalizado = @identificador_normalizado
      );
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_auth_crear_flujo_autenticacion
    @id_flujo_autenticacion UNIQUEIDENTIFIER,
    @id_usuario BIGINT,
    @id_tenant BIGINT,
    @mfa_requerido BIT,
    @mfa_validado BIT,
    @expira_en_utc DATETIME2,
    @ip_origen NVARCHAR(45) = NULL,
    @agente_usuario NVARCHAR(300) = NULL,
    @huella_dispositivo NVARCHAR(200) = NULL,
    @solicitud_id NVARCHAR(64) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO seguridad.flujo_autenticacion
    (
        id_flujo_autenticacion,
        id_usuario,
        id_tenant,
        mfa_requerido,
        mfa_validado,
        expira_en_utc,
        usado,
        ip_origen,
        agente_usuario,
        huella_dispositivo,
        solicitud_id,
        creado_utc
    )
    VALUES
    (
        @id_flujo_autenticacion,
        @id_usuario,
        @id_tenant,
        @mfa_requerido,
        @mfa_validado,
        @expira_en_utc,
        0,
        @ip_origen,
        @agente_usuario,
        @huella_dispositivo,
        @solicitud_id,
        SYSUTCDATETIME()
    );

    SELECT @id_flujo_autenticacion AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_auth_marcar_flujo_autenticacion_usado
    @id_flujo_autenticacion UNIQUEIDENTIFIER,
    @mfa_validado BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE seguridad.flujo_autenticacion
    SET
        usado = 1,
        mfa_validado = @mfa_validado
    WHERE id_flujo_autenticacion = @id_flujo_autenticacion
      AND usado = 0;

    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_auth_crear_desafio_mfa
    @id_desafio_mfa UNIQUEIDENTIFIER,
    @id_usuario BIGINT,
    @id_tenant BIGINT,
    @id_empresa BIGINT = NULL,
    @id_flujo_autenticacion UNIQUEIDENTIFIER,
    @id_proposito_desafio_mfa SMALLINT,
    @id_canal_notificacion SMALLINT,
    @codigo_accion NVARCHAR(100) = NULL,
    @otp_hash BINARY(32),
    @otp_salt VARBINARY(16),
    @expira_en_utc DATETIME2,
    @max_intentos SMALLINT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO seguridad.desafio_mfa
    (
        id_desafio_mfa,
        id_usuario,
        id_tenant,
        id_empresa,
        id_sesion_usuario,
        id_flujo_autenticacion,
        id_proposito_desafio_mfa,
        id_canal_notificacion,
        codigo_accion,
        otp_hash,
        otp_salt,
        expira_en_utc,
        usado,
        intentos,
        max_intentos,
        creado_utc,
        validado_utc
    )
    VALUES
    (
        @id_desafio_mfa,
        @id_usuario,
        @id_tenant,
        @id_empresa,
        NULL,
        @id_flujo_autenticacion,
        @id_proposito_desafio_mfa,
        @id_canal_notificacion,
        @codigo_accion,
        @otp_hash,
        @otp_salt,
        @expira_en_utc,
        0,
        0,
        @max_intentos,
        SYSUTCDATETIME(),
        NULL
    );

    SELECT @id_desafio_mfa AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_auth_obtener_desafio_mfa
    @id_desafio_mfa UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_desafio_mfa,
        id_flujo_autenticacion,
        id_usuario,
        id_tenant,
        id_empresa,
        otp_hash,
        otp_salt,
        expira_en_utc,
        usado,
        intentos,
        max_intentos
    FROM seguridad.desafio_mfa
    WHERE id_desafio_mfa = @id_desafio_mfa;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_auth_incrementar_intento_desafio_mfa
    @id_desafio_mfa UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE seguridad.desafio_mfa
    SET
        intentos = intentos + 1
    WHERE id_desafio_mfa = @id_desafio_mfa
      AND usado = 0;

    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_auth_marcar_desafio_mfa_validado
    @id_desafio_mfa UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE seguridad.desafio_mfa
    SET
        usado = 1,
        validado_utc = SYSUTCDATETIME()
    WHERE id_desafio_mfa = @id_desafio_mfa
      AND usado = 0;

    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_auth_crear_sesion_usuario
    @id_sesion_usuario UNIQUEIDENTIFIER,
    @id_usuario BIGINT,
    @id_tenant BIGINT,
    @id_empresa BIGINT,
    @token_hash BINARY(32),
    @refresh_hash BINARY(32) = NULL,
    @origen_autenticacion VARCHAR(20),
    @mfa_validado BIT,
    @creado_utc DATETIME2,
    @expira_absoluta_utc DATETIME2,
    @ultima_actividad_utc DATETIME2,
    @ip_origen NVARCHAR(45) = NULL,
    @agente_usuario NVARCHAR(300) = NULL,
    @huella_dispositivo NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO seguridad.sesion_usuario
    (
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
    VALUES
    (
        @id_sesion_usuario,
        @id_usuario,
        @id_tenant,
        @id_empresa,
        @token_hash,
        @refresh_hash,
        @origen_autenticacion,
        @mfa_validado,
        @creado_utc,
        @expira_absoluta_utc,
        @ultima_actividad_utc,
        @ip_origen,
        @agente_usuario,
        @huella_dispositivo,
        1,
        NULL,
        NULL
    );

    SELECT @id_sesion_usuario AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_auth_obtener_permisos_usuario
    @id_usuario BIGINT,
    @id_tenant BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH permisos_rol AS
    (
        SELECT DISTINCT pp.id_permiso
        FROM seguridad.asignacion_rol_usuario aru
        INNER JOIN seguridad.rol_deber rd
            ON rd.id_rol = aru.id_rol
           AND rd.activo = 1
        INNER JOIN seguridad.deber_privilegio dp
            ON dp.id_deber = rd.id_deber
           AND dp.activo = 1
        INNER JOIN seguridad.privilegio_permiso pp
            ON pp.id_privilegio = dp.id_privilegio
           AND pp.activo = 1
        WHERE aru.id_usuario = @id_usuario
          AND aru.id_tenant = @id_tenant
          AND aru.activo = 1
          AND aru.fecha_inicio_utc <= SYSUTCDATETIME()
          AND (aru.fecha_fin_utc IS NULL OR aru.fecha_fin_utc >= SYSUTCDATETIME())
    ),
    permisos_base AS
    (
        SELECT p.id_permiso, p.codigo
        FROM seguridad.permiso p
        INNER JOIN permisos_rol pr
            ON pr.id_permiso = p.id_permiso
        WHERE p.activo = 1

        UNION

        SELECT p.id_permiso, p.codigo
        FROM seguridad.permiso p
        INNER JOIN seguridad.recurso_ui_permiso rup
            ON rup.id_permiso = p.id_permiso
           AND rup.activo = 1
        WHERE p.activo = 1
          AND NOT EXISTS (SELECT 1 FROM permisos_rol)
    )
    SELECT DISTINCT codigo
    FROM permisos_base
    ORDER BY codigo;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_auth_obtener_recursos_ui_usuario
    @id_usuario BIGINT,
    @id_tenant BIGINT
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH permisos_rol AS
    (
        SELECT DISTINCT pp.id_permiso
        FROM seguridad.asignacion_rol_usuario aru
        INNER JOIN seguridad.rol_deber rd
            ON rd.id_rol = aru.id_rol
           AND rd.activo = 1
        INNER JOIN seguridad.deber_privilegio dp
            ON dp.id_deber = rd.id_deber
           AND dp.activo = 1
        INNER JOIN seguridad.privilegio_permiso pp
            ON pp.id_privilegio = dp.id_privilegio
           AND pp.activo = 1
        WHERE aru.id_usuario = @id_usuario
          AND aru.id_tenant = @id_tenant
          AND aru.activo = 1
          AND aru.fecha_inicio_utc <= SYSUTCDATETIME()
          AND (aru.fecha_fin_utc IS NULL OR aru.fecha_fin_utc >= SYSUTCDATETIME())
    ),
    recursos_por_permiso AS
    (
        SELECT DISTINCT r.id_recurso_ui
        FROM seguridad.recurso_ui r
        INNER JOIN seguridad.recurso_ui_permiso rup
            ON rup.id_recurso_ui = r.id_recurso_ui
           AND rup.activo = 1
        INNER JOIN permisos_rol pr
            ON pr.id_permiso = rup.id_permiso
        WHERE r.activo = 1
          AND r.es_visible = 1
    ),
    recursos_sin_permiso AS
    (
        SELECT r.id_recurso_ui
        FROM seguridad.recurso_ui r
        WHERE r.activo = 1
          AND r.es_visible = 1
          AND NOT EXISTS
            (
                SELECT 1
                FROM seguridad.recurso_ui_permiso rup
                WHERE rup.id_recurso_ui = r.id_recurso_ui
                  AND rup.activo = 1
            )
    ),
    recursos_finales AS
    (
        SELECT id_recurso_ui FROM recursos_por_permiso
        UNION
        SELECT id_recurso_ui FROM recursos_sin_permiso
        UNION
        SELECT r.id_recurso_ui
        FROM seguridad.recurso_ui r
        WHERE r.activo = 1
          AND r.es_visible = 1
          AND NOT EXISTS (SELECT 1 FROM permisos_rol)
    )
    SELECT DISTINCT
        r.id_recurso_ui,
        r.codigo,
        r.nombre,
        r.ruta,
        r.componente,
        r.icono,
        r.orden_visual,
        r.id_recurso_ui_padre
    FROM seguridad.recurso_ui r
    INNER JOIN recursos_finales rf
        ON rf.id_recurso_ui = r.id_recurso_ui
    WHERE r.activo = 1
      AND r.es_visible = 1
    ORDER BY r.orden_visual, r.nombre;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_auth_crear_flujo_restablecimiento_clave
    @id_flujo_restablecimiento_clave UNIQUEIDENTIFIER,
    @id_usuario BIGINT,
    @id_tipo_verificacion_restablecimiento SMALLINT,
    @expira_en_utc DATETIME2,
    @ip_origen NVARCHAR(45) = NULL,
    @agente_usuario NVARCHAR(300) = NULL,
    @solicitud_id NVARCHAR(64) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO seguridad.flujo_restablecimiento_clave
    (
        id_flujo_restablecimiento_clave,
        id_usuario,
        id_tipo_verificacion_restablecimiento,
        verificacion_completada,
        expira_en_utc,
        usado,
        ip_origen,
        agente_usuario,
        solicitud_id,
        creado_utc
    )
    VALUES
    (
        @id_flujo_restablecimiento_clave,
        @id_usuario,
        @id_tipo_verificacion_restablecimiento,
        0,
        @expira_en_utc,
        0,
        @ip_origen,
        @agente_usuario,
        @solicitud_id,
        SYSUTCDATETIME()
    );

    SELECT @id_flujo_restablecimiento_clave AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_auth_crear_token_restablecimiento_clave
    @id_token_restablecimiento_clave UNIQUEIDENTIFIER,
    @id_usuario BIGINT,
    @id_flujo_restablecimiento_clave UNIQUEIDENTIFIER,
    @token_hash BINARY(32),
    @expira_en_utc DATETIME2,
    @ip_origen NVARCHAR(45) = NULL,
    @agente_usuario NVARCHAR(300) = NULL,
    @solicitud_id NVARCHAR(64) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO seguridad.token_restablecimiento_clave
    (
        id_token_restablecimiento_clave,
        id_usuario,
        id_flujo_restablecimiento_clave,
        token_hash,
        expira_en_utc,
        usado,
        fecha_uso_utc,
        ip_origen,
        agente_usuario,
        solicitud_id,
        creado_utc
    )
    VALUES
    (
        @id_token_restablecimiento_clave,
        @id_usuario,
        @id_flujo_restablecimiento_clave,
        @token_hash,
        @expira_en_utc,
        0,
        NULL,
        @ip_origen,
        @agente_usuario,
        @solicitud_id,
        SYSUTCDATETIME()
    );

    SELECT @id_token_restablecimiento_clave AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_auth_obtener_token_restablecimiento_por_hash
    @token_hash BINARY(32),
    @id_flujo_restablecimiento_clave UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        tk.id_token_restablecimiento_clave,
        tk.id_flujo_restablecimiento_clave,
        tk.id_usuario,
        tk.expira_en_utc,
        tk.usado,
        fr.usado AS flujo_usado
    FROM seguridad.token_restablecimiento_clave tk
    INNER JOIN seguridad.flujo_restablecimiento_clave fr
        ON fr.id_flujo_restablecimiento_clave = tk.id_flujo_restablecimiento_clave
    WHERE tk.token_hash = @token_hash
      AND tk.id_flujo_restablecimiento_clave = @id_flujo_restablecimiento_clave;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_auth_consumir_token_restablecimiento
    @id_token_restablecimiento_clave UNIQUEIDENTIFIER,
    @id_flujo_restablecimiento_clave UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRANSACTION;

    UPDATE seguridad.token_restablecimiento_clave
    SET
        usado = 1,
        fecha_uso_utc = SYSUTCDATETIME()
    WHERE id_token_restablecimiento_clave = @id_token_restablecimiento_clave
      AND usado = 0;

    DECLARE @filas_token INT = @@ROWCOUNT;

    UPDATE seguridad.flujo_restablecimiento_clave
    SET
        usado = 1,
        verificacion_completada = 1
    WHERE id_flujo_restablecimiento_clave = @id_flujo_restablecimiento_clave
      AND usado = 0;

    DECLARE @filas_flujo INT = @@ROWCOUNT;

    IF @filas_token > 0 AND @filas_flujo > 0
    BEGIN
        COMMIT TRANSACTION;
        SELECT 1 AS filas_afectadas;
        RETURN;
    END

    ROLLBACK TRANSACTION;
    SELECT 0 AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_auth_actualizar_clave_usuario
    @id_usuario BIGINT,
    @hash_clave VARBINARY(128),
    @salt_clave VARBINARY(32),
    @algoritmo_clave VARCHAR(30),
    @iteraciones_clave INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM seguridad.credencial_local_usuario WHERE id_usuario = @id_usuario)
    BEGIN
        UPDATE seguridad.credencial_local_usuario
        SET
            hash_clave = @hash_clave,
            salt_clave = @salt_clave,
            algoritmo_clave = @algoritmo_clave,
            iteraciones_clave = @iteraciones_clave,
            cambio_clave_utc = SYSUTCDATETIME(),
            debe_cambiar_clave = 0,
            activo = 1
        WHERE id_usuario = @id_usuario;
    END
    ELSE
    BEGIN
        INSERT INTO seguridad.credencial_local_usuario
        (
            id_usuario,
            hash_clave,
            salt_clave,
            algoritmo_clave,
            iteraciones_clave,
            cambio_clave_utc,
            debe_cambiar_clave,
            activo
        )
        VALUES
        (
            @id_usuario,
            @hash_clave,
            @salt_clave,
            @algoritmo_clave,
            @iteraciones_clave,
            SYSUTCDATETIME(),
            0,
            1
        );
    END

    UPDATE seguridad.usuario
    SET
        requiere_cambio_clave = 0,
        actualizado_utc = SYSUTCDATETIME()
    WHERE id_usuario = @id_usuario;

    SELECT 1 AS filas_afectadas;
END
GO
