CREATE OR ALTER PROCEDURE tercero.usp_cuenta_bancaria_tercero_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_cuenta_bancaria_tercero], [id_tercero], [id_banco], [numero_cuenta], [id_moneda], [principal]
    FROM tercero.cuenta_bancaria_tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_cuenta_bancaria_tercero_obtener
    @id_cuenta_bancaria_tercero bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_cuenta_bancaria_tercero], [id_tercero], [id_banco], [numero_cuenta], [id_moneda], [principal]
    FROM tercero.cuenta_bancaria_tercero
    WHERE [id_cuenta_bancaria_tercero] = @id_cuenta_bancaria_tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_cuenta_bancaria_tercero_crear
    @id_tercero bigint,
    @id_banco int,
    @numero_cuenta nvarchar(100),
    @id_moneda int,
    @principal bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tercero.cuenta_bancaria_tercero ([id_tercero], [id_banco], [numero_cuenta], [id_moneda], [principal])
    VALUES (@id_tercero, @id_banco, @numero_cuenta, @id_moneda, @principal);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_cuenta_bancaria_tercero_actualizar
    @id_cuenta_bancaria_tercero bigint,
    @id_tercero bigint,
    @id_banco int,
    @numero_cuenta nvarchar(100),
    @id_moneda int,
    @principal bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tercero.cuenta_bancaria_tercero
    SET [id_tercero] = @id_tercero,
        [id_banco] = @id_banco,
        [numero_cuenta] = @numero_cuenta,
        [id_moneda] = @id_moneda,
        [principal] = @principal
    WHERE [id_cuenta_bancaria_tercero] = @id_cuenta_bancaria_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_cuenta_bancaria_tercero_desactivar
    @id_cuenta_bancaria_tercero bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tercero.cuenta_bancaria_tercero
    WHERE [id_cuenta_bancaria_tercero] = @id_cuenta_bancaria_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
