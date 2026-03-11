CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_empresa_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario_scope_empresa], [id_usuario], [id_empresa]
    FROM seguridad.usuario_scope_empresa;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_empresa_obtener
    @id_usuario_scope_empresa bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario_scope_empresa], [id_usuario], [id_empresa]
    FROM seguridad.usuario_scope_empresa
    WHERE [id_usuario_scope_empresa] = @id_usuario_scope_empresa;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_empresa_crear
    @id_usuario bigint,
    @id_empresa bigint
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.usuario_scope_empresa ([id_usuario], [id_empresa])
    VALUES (@id_usuario, @id_empresa);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_empresa_actualizar
    @id_usuario_scope_empresa bigint,
    @id_usuario bigint,
    @id_empresa bigint
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.usuario_scope_empresa
    SET [id_usuario] = @id_usuario,
        [id_empresa] = @id_empresa
    WHERE [id_usuario_scope_empresa] = @id_usuario_scope_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_empresa_desactivar
    @id_usuario_scope_empresa bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM seguridad.usuario_scope_empresa
    WHERE [id_usuario_scope_empresa] = @id_usuario_scope_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
