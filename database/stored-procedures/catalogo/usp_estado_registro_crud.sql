CREATE OR ALTER PROCEDURE catalogo.usp_estado_registro_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_estado], [codigo], [nombre], [descripcion], [activo]
    FROM catalogo.estado_registro;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_estado_registro_obtener
    @id_estado int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_estado], [codigo], [nombre], [descripcion], [activo]
    FROM catalogo.estado_registro
    WHERE [id_estado] = @id_estado;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_estado_registro_crear
    @codigo varchar(50),
    @nombre varchar(100),
    @descripcion varchar(200),
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO catalogo.estado_registro ([codigo], [nombre], [descripcion], [activo])
    VALUES (@codigo, @nombre, @descripcion, @activo);
    SELECT CAST(SCOPE_IDENTITY() AS int) AS id;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_estado_registro_actualizar
    @id_estado int,
    @codigo varchar(50),
    @nombre varchar(100),
    @descripcion varchar(200),
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE catalogo.estado_registro
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [activo] = @activo
    WHERE [id_estado] = @id_estado;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_estado_registro_desactivar
    @id_estado int,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE catalogo.estado_registro
    SET [activo] = 0
    WHERE [id_estado] = @id_estado;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
