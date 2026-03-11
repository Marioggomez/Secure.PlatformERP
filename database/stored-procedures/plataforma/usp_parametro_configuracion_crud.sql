CREATE OR ALTER PROCEDURE plataforma.usp_parametro_configuracion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_parametro_configuracion], [id_categoria_configuracion], [codigo], [descripcion], [tipo_valor], [valor_defecto]
    FROM plataforma.parametro_configuracion;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_parametro_configuracion_obtener
    @id_parametro_configuracion int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_parametro_configuracion], [id_categoria_configuracion], [codigo], [descripcion], [tipo_valor], [valor_defecto]
    FROM plataforma.parametro_configuracion
    WHERE [id_parametro_configuracion] = @id_parametro_configuracion;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_parametro_configuracion_crear
    @id_categoria_configuracion int,
    @codigo varchar(100),
    @descripcion varchar(500),
    @tipo_valor varchar(50),
    @valor_defecto varchar(500)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO plataforma.parametro_configuracion ([id_categoria_configuracion], [codigo], [descripcion], [tipo_valor], [valor_defecto])
    VALUES (@id_categoria_configuracion, @codigo, @descripcion, @tipo_valor, @valor_defecto);
    SELECT CAST(SCOPE_IDENTITY() AS int) AS id;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_parametro_configuracion_actualizar
    @id_parametro_configuracion int,
    @id_categoria_configuracion int,
    @codigo varchar(100),
    @descripcion varchar(500),
    @tipo_valor varchar(50),
    @valor_defecto varchar(500)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.parametro_configuracion
    SET [id_categoria_configuracion] = @id_categoria_configuracion,
        [codigo] = @codigo,
        [descripcion] = @descripcion,
        [tipo_valor] = @tipo_valor,
        [valor_defecto] = @valor_defecto
    WHERE [id_parametro_configuracion] = @id_parametro_configuracion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_parametro_configuracion_desactivar
    @id_parametro_configuracion int,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM plataforma.parametro_configuracion
    WHERE [id_parametro_configuracion] = @id_parametro_configuracion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
