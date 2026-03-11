CREATE OR ALTER PROCEDURE catalogo.usp_proposito_desafio_mfa_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_proposito_desafio_mfa], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.proposito_desafio_mfa;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_proposito_desafio_mfa_obtener
    @id_proposito_desafio_mfa smallint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_proposito_desafio_mfa], [codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM catalogo.proposito_desafio_mfa
    WHERE [id_proposito_desafio_mfa] = @id_proposito_desafio_mfa;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_proposito_desafio_mfa_crear
    @codigo varchar(30),
    @nombre nvarchar(120),
    @descripcion nvarchar(300),
    @orden_visual smallint,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO catalogo.proposito_desafio_mfa ([codigo], [nombre], [descripcion], [orden_visual], [activo], [creado_utc], [actualizado_utc])
    VALUES (@codigo, @nombre, @descripcion, @orden_visual, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS smallint) AS id;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_proposito_desafio_mfa_actualizar
    @id_proposito_desafio_mfa smallint,
    @codigo varchar(30),
    @nombre nvarchar(120),
    @descripcion nvarchar(300),
    @orden_visual smallint,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE catalogo.proposito_desafio_mfa
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [orden_visual] = @orden_visual,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_proposito_desafio_mfa] = @id_proposito_desafio_mfa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE catalogo.usp_proposito_desafio_mfa_desactivar
    @id_proposito_desafio_mfa smallint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE catalogo.proposito_desafio_mfa
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_proposito_desafio_mfa] = @id_proposito_desafio_mfa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
