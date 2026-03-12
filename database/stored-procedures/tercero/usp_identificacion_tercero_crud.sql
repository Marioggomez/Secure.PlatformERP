CREATE OR ALTER PROCEDURE tercero.usp_identificacion_tercero_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_identificacion_tercero], [id_tercero], [id_tipo_identificacion], [numero_identificacion], [fecha_emision], [fecha_vencimiento], [principal]
    FROM tercero.identificacion_tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_identificacion_tercero_obtener
    @id_identificacion_tercero bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_identificacion_tercero], [id_tercero], [id_tipo_identificacion], [numero_identificacion], [fecha_emision], [fecha_vencimiento], [principal]
    FROM tercero.identificacion_tercero
    WHERE [id_identificacion_tercero] = @id_identificacion_tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_identificacion_tercero_crear
    @id_tercero bigint,
    @id_tipo_identificacion int,
    @numero_identificacion nvarchar(100),
    @fecha_emision date,
    @fecha_vencimiento date,
    @principal bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tercero.identificacion_tercero ([id_tercero], [id_tipo_identificacion], [numero_identificacion], [fecha_emision], [fecha_vencimiento], [principal])
    VALUES (@id_tercero, @id_tipo_identificacion, @numero_identificacion, @fecha_emision, @fecha_vencimiento, @principal);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_identificacion_tercero_actualizar
    @id_identificacion_tercero bigint,
    @id_tercero bigint,
    @id_tipo_identificacion int,
    @numero_identificacion nvarchar(100),
    @fecha_emision date,
    @fecha_vencimiento date,
    @principal bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tercero.identificacion_tercero
    SET [id_tercero] = @id_tercero,
        [id_tipo_identificacion] = @id_tipo_identificacion,
        [numero_identificacion] = @numero_identificacion,
        [fecha_emision] = @fecha_emision,
        [fecha_vencimiento] = @fecha_vencimiento,
        [principal] = @principal
    WHERE [id_identificacion_tercero] = @id_identificacion_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_identificacion_tercero_desactivar
    @id_identificacion_tercero bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tercero.identificacion_tercero
    WHERE [id_identificacion_tercero] = @id_identificacion_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
