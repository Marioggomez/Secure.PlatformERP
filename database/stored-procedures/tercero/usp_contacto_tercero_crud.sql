CREATE OR ALTER PROCEDURE tercero.usp_contacto_tercero_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_contacto_tercero], [id_tercero], [id_tipo_contacto], [valor], [principal]
    FROM tercero.contacto_tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_contacto_tercero_obtener
    @id_contacto_tercero bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_contacto_tercero], [id_tercero], [id_tipo_contacto], [valor], [principal]
    FROM tercero.contacto_tercero
    WHERE [id_contacto_tercero] = @id_contacto_tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_contacto_tercero_crear
    @id_tercero bigint,
    @id_tipo_contacto int,
    @valor nvarchar(300),
    @principal bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tercero.contacto_tercero ([id_tercero], [id_tipo_contacto], [valor], [principal])
    VALUES (@id_tercero, @id_tipo_contacto, @valor, @principal);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_contacto_tercero_actualizar
    @id_contacto_tercero bigint,
    @id_tercero bigint,
    @id_tipo_contacto int,
    @valor nvarchar(300),
    @principal bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tercero.contacto_tercero
    SET [id_tercero] = @id_tercero,
        [id_tipo_contacto] = @id_tipo_contacto,
        [valor] = @valor,
        [principal] = @principal
    WHERE [id_contacto_tercero] = @id_contacto_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_contacto_tercero_desactivar
    @id_contacto_tercero bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tercero.contacto_tercero
    WHERE [id_contacto_tercero] = @id_contacto_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
