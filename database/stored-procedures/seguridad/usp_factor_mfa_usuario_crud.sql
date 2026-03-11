CREATE OR ALTER PROCEDURE seguridad.usp_factor_mfa_usuario_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_factor_mfa_usuario], [id_usuario], [id_tipo_factor_mfa], [etiqueta], [destino_enmascarado], [referencia_secreto], [secreto_cifrado], [configuracion_json], [verificado], [es_predeterminado], [ultimo_uso_utc], [fecha_enrolamiento_utc], [activo], [version_fila]
    FROM seguridad.factor_mfa_usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_factor_mfa_usuario_obtener
    @id_factor_mfa_usuario bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_factor_mfa_usuario], [id_usuario], [id_tipo_factor_mfa], [etiqueta], [destino_enmascarado], [referencia_secreto], [secreto_cifrado], [configuracion_json], [verificado], [es_predeterminado], [ultimo_uso_utc], [fecha_enrolamiento_utc], [activo], [version_fila]
    FROM seguridad.factor_mfa_usuario
    WHERE [id_factor_mfa_usuario] = @id_factor_mfa_usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_factor_mfa_usuario_crear
    @id_usuario bigint,
    @id_tipo_factor_mfa smallint,
    @etiqueta nvarchar(100),
    @destino_enmascarado nvarchar(120),
    @referencia_secreto nvarchar(300),
    @secreto_cifrado varbinary(512),
    @configuracion_json nvarchar(max),
    @verificado bit,
    @es_predeterminado bit,
    @ultimo_uso_utc datetime2,
    @fecha_enrolamiento_utc datetime2,
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.factor_mfa_usuario ([id_usuario], [id_tipo_factor_mfa], [etiqueta], [destino_enmascarado], [referencia_secreto], [secreto_cifrado], [configuracion_json], [verificado], [es_predeterminado], [ultimo_uso_utc], [fecha_enrolamiento_utc], [activo])
    VALUES (@id_usuario, @id_tipo_factor_mfa, @etiqueta, @destino_enmascarado, @referencia_secreto, @secreto_cifrado, @configuracion_json, @verificado, @es_predeterminado, @ultimo_uso_utc, @fecha_enrolamiento_utc, @activo);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_factor_mfa_usuario_actualizar
    @id_factor_mfa_usuario bigint,
    @id_usuario bigint,
    @id_tipo_factor_mfa smallint,
    @etiqueta nvarchar(100),
    @destino_enmascarado nvarchar(120),
    @referencia_secreto nvarchar(300),
    @secreto_cifrado varbinary(512),
    @configuracion_json nvarchar(max),
    @verificado bit,
    @es_predeterminado bit,
    @ultimo_uso_utc datetime2,
    @fecha_enrolamiento_utc datetime2,
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.factor_mfa_usuario
    SET [id_usuario] = @id_usuario,
        [id_tipo_factor_mfa] = @id_tipo_factor_mfa,
        [etiqueta] = @etiqueta,
        [destino_enmascarado] = @destino_enmascarado,
        [referencia_secreto] = @referencia_secreto,
        [secreto_cifrado] = @secreto_cifrado,
        [configuracion_json] = @configuracion_json,
        [verificado] = @verificado,
        [es_predeterminado] = @es_predeterminado,
        [ultimo_uso_utc] = @ultimo_uso_utc,
        [fecha_enrolamiento_utc] = @fecha_enrolamiento_utc,
        [activo] = @activo
    WHERE [id_factor_mfa_usuario] = @id_factor_mfa_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_factor_mfa_usuario_desactivar
    @id_factor_mfa_usuario bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.factor_mfa_usuario
    SET [activo] = 0
    WHERE [id_factor_mfa_usuario] = @id_factor_mfa_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
