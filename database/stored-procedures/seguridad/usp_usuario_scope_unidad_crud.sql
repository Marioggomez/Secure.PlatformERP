CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_unidad_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario_scope_unidad], [id_usuario], [id_unidad_organizativa]
    FROM seguridad.usuario_scope_unidad;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_unidad_obtener
    @id_usuario_scope_unidad bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario_scope_unidad], [id_usuario], [id_unidad_organizativa]
    FROM seguridad.usuario_scope_unidad
    WHERE [id_usuario_scope_unidad] = @id_usuario_scope_unidad;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_unidad_crear
    @id_usuario bigint,
    @id_unidad_organizativa bigint
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.usuario_scope_unidad ([id_usuario], [id_unidad_organizativa])
    VALUES (@id_usuario, @id_unidad_organizativa);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_unidad_actualizar
    @id_usuario_scope_unidad bigint,
    @id_usuario bigint,
    @id_unidad_organizativa bigint
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.usuario_scope_unidad
    SET [id_usuario] = @id_usuario,
        [id_unidad_organizativa] = @id_unidad_organizativa
    WHERE [id_usuario_scope_unidad] = @id_usuario_scope_unidad;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_scope_unidad_desactivar
    @id_usuario_scope_unidad bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM seguridad.usuario_scope_unidad
    WHERE [id_usuario_scope_unidad] = @id_usuario_scope_unidad;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
