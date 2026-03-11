CREATE OR ALTER PROCEDURE plataforma.usp_feature_flag_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_feature], [codigo], [descripcion]
    FROM plataforma.feature_flag;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_feature_flag_obtener
    @id_feature bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_feature], [codigo], [descripcion]
    FROM plataforma.feature_flag
    WHERE [id_feature] = @id_feature;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_feature_flag_crear
    @codigo varchar(100),
    @descripcion varchar(300)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO plataforma.feature_flag ([codigo], [descripcion])
    VALUES (@codigo, @descripcion);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_feature_flag_actualizar
    @id_feature bigint,
    @codigo varchar(100),
    @descripcion varchar(300)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.feature_flag
    SET [codigo] = @codigo,
        [descripcion] = @descripcion
    WHERE [id_feature] = @id_feature;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_feature_flag_desactivar
    @id_feature bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM plataforma.feature_flag
    WHERE [id_feature] = @id_feature;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
