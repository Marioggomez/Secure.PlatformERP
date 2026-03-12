CREATE OR ALTER PROCEDURE tercero.usp_tipo_persona_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tipo_persona], [codigo], [nombre]
    FROM tercero.tipo_persona;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tipo_persona_obtener
    @id_tipo_persona int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tipo_persona], [codigo], [nombre]
    FROM tercero.tipo_persona
    WHERE [id_tipo_persona] = @id_tipo_persona;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tipo_persona_crear
    @codigo nvarchar(50),
    @nombre nvarchar(100)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tercero.tipo_persona ([codigo], [nombre])
    VALUES (@codigo, @nombre);
    SELECT CAST(SCOPE_IDENTITY() AS int) AS id;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tipo_persona_actualizar
    @id_tipo_persona int,
    @codigo nvarchar(50),
    @nombre nvarchar(100)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tercero.tipo_persona
    SET [codigo] = @codigo,
        [nombre] = @nombre
    WHERE [id_tipo_persona] = @id_tipo_persona;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tipo_persona_desactivar
    @id_tipo_persona int,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tercero.tipo_persona
    WHERE [id_tipo_persona] = @id_tipo_persona;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
