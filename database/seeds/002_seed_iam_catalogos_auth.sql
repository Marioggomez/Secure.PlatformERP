/*
    Seed de catalogos IAM para autenticacion, MFA y recuperacion.
    Autor: Mario Gomez.
*/
SET NOCOUNT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    IF EXISTS (SELECT 1 FROM catalogo.canal_notificacion WHERE id_canal_notificacion = 1)
    BEGIN
        UPDATE catalogo.canal_notificacion
        SET
            codigo = 'EMAIL',
            nombre = N'Correo',
            descripcion = N'Canal de correo corporativo',
            orden_visual = 1,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE id_canal_notificacion = 1;
    END
    ELSE IF EXISTS (SELECT 1 FROM catalogo.canal_notificacion WHERE codigo = 'EMAIL')
    BEGIN
        UPDATE catalogo.canal_notificacion
        SET
            nombre = N'Correo',
            descripcion = N'Canal de correo corporativo',
            orden_visual = 1,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE codigo = 'EMAIL';
    END
    ELSE
    BEGIN
        SET IDENTITY_INSERT catalogo.canal_notificacion ON;
        INSERT INTO catalogo.canal_notificacion
        (
            id_canal_notificacion,
            codigo,
            nombre,
            descripcion,
            orden_visual,
            activo,
            creado_utc
        )
        VALUES
        (
            1,
            'EMAIL',
            N'Correo',
            N'Canal de correo corporativo',
            1,
            1,
            SYSUTCDATETIME()
        );
        SET IDENTITY_INSERT catalogo.canal_notificacion OFF;
    END;

    IF EXISTS (SELECT 1 FROM catalogo.canal_notificacion WHERE id_canal_notificacion = 2)
    BEGIN
        UPDATE catalogo.canal_notificacion
        SET
            codigo = 'SMS',
            nombre = N'SMS',
            descripcion = N'Canal SMS corporativo',
            orden_visual = 2,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE id_canal_notificacion = 2;
    END
    ELSE IF EXISTS (SELECT 1 FROM catalogo.canal_notificacion WHERE codigo = 'SMS')
    BEGIN
        UPDATE catalogo.canal_notificacion
        SET
            nombre = N'SMS',
            descripcion = N'Canal SMS corporativo',
            orden_visual = 2,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE codigo = 'SMS';
    END
    ELSE
    BEGIN
        SET IDENTITY_INSERT catalogo.canal_notificacion ON;
        INSERT INTO catalogo.canal_notificacion
        (
            id_canal_notificacion,
            codigo,
            nombre,
            descripcion,
            orden_visual,
            activo,
            creado_utc
        )
        VALUES
        (
            2,
            'SMS',
            N'SMS',
            N'Canal SMS corporativo',
            2,
            1,
            SYSUTCDATETIME()
        );
        SET IDENTITY_INSERT catalogo.canal_notificacion OFF;
    END;

    IF EXISTS (SELECT 1 FROM catalogo.proposito_desafio_mfa WHERE id_proposito_desafio_mfa = 1)
    BEGIN
        UPDATE catalogo.proposito_desafio_mfa
        SET
            codigo = 'LOGIN',
            nombre = N'Login',
            descripcion = N'Desafio para inicio de sesion',
            orden_visual = 1,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE id_proposito_desafio_mfa = 1;
    END
    ELSE IF EXISTS (SELECT 1 FROM catalogo.proposito_desafio_mfa WHERE codigo = 'LOGIN')
    BEGIN
        UPDATE catalogo.proposito_desafio_mfa
        SET
            nombre = N'Login',
            descripcion = N'Desafio para inicio de sesion',
            orden_visual = 1,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE codigo = 'LOGIN';
    END
    ELSE
    BEGIN
        SET IDENTITY_INSERT catalogo.proposito_desafio_mfa ON;
        INSERT INTO catalogo.proposito_desafio_mfa
        (
            id_proposito_desafio_mfa,
            codigo,
            nombre,
            descripcion,
            orden_visual,
            activo,
            creado_utc
        )
        VALUES
        (
            1,
            'LOGIN',
            N'Login',
            N'Desafio para inicio de sesion',
            1,
            1,
            SYSUTCDATETIME()
        );
        SET IDENTITY_INSERT catalogo.proposito_desafio_mfa OFF;
    END;

    IF EXISTS (SELECT 1 FROM catalogo.tipo_verificacion_restablecimiento WHERE id_tipo_verificacion_restablecimiento = 1)
    BEGIN
        UPDATE catalogo.tipo_verificacion_restablecimiento
        SET
            codigo = 'EMAIL',
            nombre = N'Correo',
            descripcion = N'Restablecimiento validado por correo',
            orden_visual = 1,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE id_tipo_verificacion_restablecimiento = 1;
    END
    ELSE IF EXISTS (SELECT 1 FROM catalogo.tipo_verificacion_restablecimiento WHERE codigo = 'EMAIL')
    BEGIN
        UPDATE catalogo.tipo_verificacion_restablecimiento
        SET
            nombre = N'Correo',
            descripcion = N'Restablecimiento validado por correo',
            orden_visual = 1,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE codigo = 'EMAIL';
    END
    ELSE
    BEGIN
        SET IDENTITY_INSERT catalogo.tipo_verificacion_restablecimiento ON;
        INSERT INTO catalogo.tipo_verificacion_restablecimiento
        (
            id_tipo_verificacion_restablecimiento,
            codigo,
            nombre,
            descripcion,
            orden_visual,
            activo,
            creado_utc
        )
        VALUES
        (
            1,
            'EMAIL',
            N'Correo',
            N'Restablecimiento validado por correo',
            1,
            1,
            SYSUTCDATETIME()
        );
        SET IDENTITY_INSERT catalogo.tipo_verificacion_restablecimiento OFF;
    END;

    IF EXISTS (SELECT 1 FROM catalogo.tipo_factor_mfa WHERE id_tipo_factor_mfa = 1)
    BEGIN
        UPDATE catalogo.tipo_factor_mfa
        SET
            codigo = 'OTP',
            nombre = N'OTP',
            descripcion = N'Codigo OTP de un solo uso',
            orden_visual = 1,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE id_tipo_factor_mfa = 1;
    END
    ELSE IF EXISTS (SELECT 1 FROM catalogo.tipo_factor_mfa WHERE codigo = 'OTP')
    BEGIN
        UPDATE catalogo.tipo_factor_mfa
        SET
            nombre = N'OTP',
            descripcion = N'Codigo OTP de un solo uso',
            orden_visual = 1,
            activo = 1,
            actualizado_utc = SYSUTCDATETIME()
        WHERE codigo = 'OTP';
    END
    ELSE
    BEGIN
        SET IDENTITY_INSERT catalogo.tipo_factor_mfa ON;
        INSERT INTO catalogo.tipo_factor_mfa
        (
            id_tipo_factor_mfa,
            codigo,
            nombre,
            descripcion,
            orden_visual,
            activo,
            creado_utc
        )
        VALUES
        (
            1,
            'OTP',
            N'OTP',
            N'Codigo OTP de un solo uso',
            1,
            1,
            SYSUTCDATETIME()
        );
        SET IDENTITY_INSERT catalogo.tipo_factor_mfa OFF;
    END;

    UPDATE seguridad.usuario
    SET
        mfa_habilitado = 1,
        actualizado_utc = SYSUTCDATETIME()
    WHERE login_normalizado = N'ADMIN.SEED';

    DECLARE @idUsuario BIGINT = (
        SELECT TOP (1) id_usuario
        FROM seguridad.usuario
        WHERE login_normalizado = N'ADMIN.SEED'
    );

    IF @idUsuario IS NOT NULL
    BEGIN
        IF EXISTS (
            SELECT 1
            FROM seguridad.factor_mfa_usuario
            WHERE id_usuario = @idUsuario
              AND id_tipo_factor_mfa = 1
              AND activo = 1
        )
        BEGIN
            UPDATE seguridad.factor_mfa_usuario
            SET
                etiqueta = N'OTP Primario',
                destino_enmascarado = N'a***@seed.local',
                verificado = 1,
                es_predeterminado = 1,
                activo = 1
            WHERE id_usuario = @idUsuario
              AND id_tipo_factor_mfa = 1;
        END
        ELSE
        BEGIN
            INSERT INTO seguridad.factor_mfa_usuario
            (
                id_usuario,
                id_tipo_factor_mfa,
                etiqueta,
                destino_enmascarado,
                verificado,
                es_predeterminado,
                fecha_enrolamiento_utc,
                activo
            )
            VALUES
            (
                @idUsuario,
                1,
                N'OTP Primario',
                N'a***@seed.local',
                1,
                1,
                SYSUTCDATETIME(),
                1
            );
        END;
    END;

    COMMIT TRANSACTION;
    PRINT 'Seed IAM de catalogos aplicado correctamente.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    DECLARE @mensaje NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR('Error en seed IAM catalogos: %s', 16, 1, @mensaje);
END CATCH;
