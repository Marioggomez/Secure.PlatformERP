CREATE OR ALTER PROCEDURE seguridad.usp_politica_empresa_override_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_empresa], [timeout_inactividad_min_override], [timeout_absoluto_min_override], [mfa_obligatorio_override], [max_intentos_login_override], [minutos_bloqueo_override], [requiere_politica_ip_override], [requiere_mfa_aprobaciones_override], [creado_utc], [actualizado_utc], [version_fila]
    FROM seguridad.politica_empresa_override;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_empresa_override_obtener
    @id_empresa bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_empresa], [timeout_inactividad_min_override], [timeout_absoluto_min_override], [mfa_obligatorio_override], [max_intentos_login_override], [minutos_bloqueo_override], [requiere_politica_ip_override], [requiere_mfa_aprobaciones_override], [creado_utc], [actualizado_utc], [version_fila]
    FROM seguridad.politica_empresa_override
    WHERE [id_empresa] = @id_empresa;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_empresa_override_crear
    @timeout_inactividad_min_override int,
    @timeout_absoluto_min_override int,
    @mfa_obligatorio_override bit,
    @max_intentos_login_override tinyint,
    @minutos_bloqueo_override int,
    @requiere_politica_ip_override bit,
    @requiere_mfa_aprobaciones_override bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.politica_empresa_override ([timeout_inactividad_min_override], [timeout_absoluto_min_override], [mfa_obligatorio_override], [max_intentos_login_override], [minutos_bloqueo_override], [requiere_politica_ip_override], [requiere_mfa_aprobaciones_override], [creado_utc], [actualizado_utc])
    VALUES (@timeout_inactividad_min_override, @timeout_absoluto_min_override, @mfa_obligatorio_override, @max_intentos_login_override, @minutos_bloqueo_override, @requiere_politica_ip_override, @requiere_mfa_aprobaciones_override, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_empresa_override_actualizar
    @id_empresa bigint,
    @timeout_inactividad_min_override int,
    @timeout_absoluto_min_override int,
    @mfa_obligatorio_override bit,
    @max_intentos_login_override tinyint,
    @minutos_bloqueo_override int,
    @requiere_politica_ip_override bit,
    @requiere_mfa_aprobaciones_override bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.politica_empresa_override
    SET [timeout_inactividad_min_override] = @timeout_inactividad_min_override,
        [timeout_absoluto_min_override] = @timeout_absoluto_min_override,
        [mfa_obligatorio_override] = @mfa_obligatorio_override,
        [max_intentos_login_override] = @max_intentos_login_override,
        [minutos_bloqueo_override] = @minutos_bloqueo_override,
        [requiere_politica_ip_override] = @requiere_politica_ip_override,
        [requiere_mfa_aprobaciones_override] = @requiere_mfa_aprobaciones_override,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_empresa] = @id_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_empresa_override_desactivar
    @id_empresa bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM seguridad.politica_empresa_override
    WHERE [id_empresa] = @id_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
