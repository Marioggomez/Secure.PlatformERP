CREATE OR ALTER PROCEDURE tercero.usp_direccion_tercero_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_direccion_tercero], [id_tercero], [id_tipo_direccion], [direccion_linea1], [direccion_linea2], [id_pais], [id_estado], [id_ciudad], [codigo_postal], [principal]
    FROM tercero.direccion_tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_direccion_tercero_obtener
    @id_direccion_tercero bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_direccion_tercero], [id_tercero], [id_tipo_direccion], [direccion_linea1], [direccion_linea2], [id_pais], [id_estado], [id_ciudad], [codigo_postal], [principal]
    FROM tercero.direccion_tercero
    WHERE [id_direccion_tercero] = @id_direccion_tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_direccion_tercero_crear
    @id_tercero bigint,
    @id_tipo_direccion int,
    @direccion_linea1 nvarchar(400),
    @direccion_linea2 nvarchar(400),
    @id_pais int,
    @id_estado int,
    @id_ciudad int,
    @codigo_postal nvarchar(50),
    @principal bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tercero.direccion_tercero ([id_tercero], [id_tipo_direccion], [direccion_linea1], [direccion_linea2], [id_pais], [id_estado], [id_ciudad], [codigo_postal], [principal])
    VALUES (@id_tercero, @id_tipo_direccion, @direccion_linea1, @direccion_linea2, @id_pais, @id_estado, @id_ciudad, @codigo_postal, @principal);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_direccion_tercero_actualizar
    @id_direccion_tercero bigint,
    @id_tercero bigint,
    @id_tipo_direccion int,
    @direccion_linea1 nvarchar(400),
    @direccion_linea2 nvarchar(400),
    @id_pais int,
    @id_estado int,
    @id_ciudad int,
    @codigo_postal nvarchar(50),
    @principal bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tercero.direccion_tercero
    SET [id_tercero] = @id_tercero,
        [id_tipo_direccion] = @id_tipo_direccion,
        [direccion_linea1] = @direccion_linea1,
        [direccion_linea2] = @direccion_linea2,
        [id_pais] = @id_pais,
        [id_estado] = @id_estado,
        [id_ciudad] = @id_ciudad,
        [codigo_postal] = @codigo_postal,
        [principal] = @principal
    WHERE [id_direccion_tercero] = @id_direccion_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_direccion_tercero_desactivar
    @id_direccion_tercero bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tercero.direccion_tercero
    WHERE [id_direccion_tercero] = @id_direccion_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
