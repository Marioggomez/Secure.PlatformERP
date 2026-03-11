CREATE OR ALTER PROCEDURE seguridad.usp_usuario_identificador_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario_identificador], [id_usuario], [id_tipo_identificador_usuario], [valor], [valor_normalizado], [es_principal], [verificado], [fecha_verificacion_utc], [activo], [creado_utc]
    FROM seguridad.usuario_identificador;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_identificador_obtener
    @id_usuario_identificador bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario_identificador], [id_usuario], [id_tipo_identificador_usuario], [valor], [valor_normalizado], [es_principal], [verificado], [fecha_verificacion_utc], [activo], [creado_utc]
    FROM seguridad.usuario_identificador
    WHERE [id_usuario_identificador] = @id_usuario_identificador;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_identificador_crear
    @id_usuario bigint,
    @id_tipo_identificador_usuario smallint,
    @valor nvarchar(250),
    @valor_normalizado nvarchar(250),
    @es_principal bit,
    @verificado bit,
    @fecha_verificacion_utc datetime2,
    @activo bit,
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.usuario_identificador ([id_usuario], [id_tipo_identificador_usuario], [valor], [valor_normalizado], [es_principal], [verificado], [fecha_verificacion_utc], [activo], [creado_utc])
    VALUES (@id_usuario, @id_tipo_identificador_usuario, @valor, @valor_normalizado, @es_principal, @verificado, @fecha_verificacion_utc, @activo, @creado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_identificador_actualizar
    @id_usuario_identificador bigint,
    @id_usuario bigint,
    @id_tipo_identificador_usuario smallint,
    @valor nvarchar(250),
    @valor_normalizado nvarchar(250),
    @es_principal bit,
    @verificado bit,
    @fecha_verificacion_utc datetime2,
    @activo bit,
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.usuario_identificador
    SET [id_usuario] = @id_usuario,
        [id_tipo_identificador_usuario] = @id_tipo_identificador_usuario,
        [valor] = @valor,
        [valor_normalizado] = @valor_normalizado,
        [es_principal] = @es_principal,
        [verificado] = @verificado,
        [fecha_verificacion_utc] = @fecha_verificacion_utc,
        [activo] = @activo,
        [creado_utc] = @creado_utc
    WHERE [id_usuario_identificador] = @id_usuario_identificador;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_identificador_desactivar
    @id_usuario_identificador bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.usuario_identificador
    SET [activo] = 0
    WHERE [id_usuario_identificador] = @id_usuario_identificador;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
