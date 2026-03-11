CREATE OR ALTER PROCEDURE seguridad.usp_politica_tenant_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tenant], [timeout_inactividad_min], [timeout_absoluto_min], [longitud_minima_clave], [requiere_mayuscula], [requiere_minuscula], [requiere_numero], [requiere_especial], [historial_claves], [max_intentos_login], [minutos_bloqueo], [mfa_obligatorio], [permite_login_local], [permite_sso], [requiere_mfa_aprobaciones], [requiere_politica_ip], [limite_rate_por_minuto], [creado_utc], [actualizado_utc], [version_fila]
    FROM seguridad.politica_tenant;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_tenant_obtener
    @id_tenant bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tenant], [timeout_inactividad_min], [timeout_absoluto_min], [longitud_minima_clave], [requiere_mayuscula], [requiere_minuscula], [requiere_numero], [requiere_especial], [historial_claves], [max_intentos_login], [minutos_bloqueo], [mfa_obligatorio], [permite_login_local], [permite_sso], [requiere_mfa_aprobaciones], [requiere_politica_ip], [limite_rate_por_minuto], [creado_utc], [actualizado_utc], [version_fila]
    FROM seguridad.politica_tenant
    WHERE [id_tenant] = @id_tenant;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_tenant_crear
    @timeout_inactividad_min int,
    @timeout_absoluto_min int,
    @longitud_minima_clave tinyint,
    @requiere_mayuscula bit,
    @requiere_minuscula bit,
    @requiere_numero bit,
    @requiere_especial bit,
    @historial_claves tinyint,
    @max_intentos_login tinyint,
    @minutos_bloqueo int,
    @mfa_obligatorio bit,
    @permite_login_local bit,
    @permite_sso bit,
    @requiere_mfa_aprobaciones bit,
    @requiere_politica_ip bit,
    @limite_rate_por_minuto int,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.politica_tenant ([timeout_inactividad_min], [timeout_absoluto_min], [longitud_minima_clave], [requiere_mayuscula], [requiere_minuscula], [requiere_numero], [requiere_especial], [historial_claves], [max_intentos_login], [minutos_bloqueo], [mfa_obligatorio], [permite_login_local], [permite_sso], [requiere_mfa_aprobaciones], [requiere_politica_ip], [limite_rate_por_minuto], [creado_utc], [actualizado_utc])
    VALUES (@timeout_inactividad_min, @timeout_absoluto_min, @longitud_minima_clave, @requiere_mayuscula, @requiere_minuscula, @requiere_numero, @requiere_especial, @historial_claves, @max_intentos_login, @minutos_bloqueo, @mfa_obligatorio, @permite_login_local, @permite_sso, @requiere_mfa_aprobaciones, @requiere_politica_ip, @limite_rate_por_minuto, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_tenant_actualizar
    @id_tenant bigint,
    @timeout_inactividad_min int,
    @timeout_absoluto_min int,
    @longitud_minima_clave tinyint,
    @requiere_mayuscula bit,
    @requiere_minuscula bit,
    @requiere_numero bit,
    @requiere_especial bit,
    @historial_claves tinyint,
    @max_intentos_login tinyint,
    @minutos_bloqueo int,
    @mfa_obligatorio bit,
    @permite_login_local bit,
    @permite_sso bit,
    @requiere_mfa_aprobaciones bit,
    @requiere_politica_ip bit,
    @limite_rate_por_minuto int,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.politica_tenant
    SET [timeout_inactividad_min] = @timeout_inactividad_min,
        [timeout_absoluto_min] = @timeout_absoluto_min,
        [longitud_minima_clave] = @longitud_minima_clave,
        [requiere_mayuscula] = @requiere_mayuscula,
        [requiere_minuscula] = @requiere_minuscula,
        [requiere_numero] = @requiere_numero,
        [requiere_especial] = @requiere_especial,
        [historial_claves] = @historial_claves,
        [max_intentos_login] = @max_intentos_login,
        [minutos_bloqueo] = @minutos_bloqueo,
        [mfa_obligatorio] = @mfa_obligatorio,
        [permite_login_local] = @permite_login_local,
        [permite_sso] = @permite_sso,
        [requiere_mfa_aprobaciones] = @requiere_mfa_aprobaciones,
        [requiere_politica_ip] = @requiere_politica_ip,
        [limite_rate_por_minuto] = @limite_rate_por_minuto,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_tenant] = @id_tenant;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_tenant_desactivar
    @id_tenant bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM seguridad.politica_tenant
    WHERE [id_tenant] = @id_tenant;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
