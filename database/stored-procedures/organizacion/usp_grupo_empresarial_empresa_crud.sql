CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_empresa_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_grupo_empresarial], [id_empresa], [activo], [creado_utc]
    FROM organizacion.grupo_empresarial_empresa;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_empresa_obtener
    @id_grupo_empresarial bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_grupo_empresarial], [id_empresa], [activo], [creado_utc]
    FROM organizacion.grupo_empresarial_empresa
    WHERE [id_grupo_empresarial] = @id_grupo_empresarial;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_empresa_crear
    @id_empresa bigint,
    @activo bit,
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO organizacion.grupo_empresarial_empresa ([id_empresa], [activo], [creado_utc])
    VALUES (@id_empresa, @activo, @creado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_empresa_actualizar
    @id_grupo_empresarial bigint,
    @id_empresa bigint,
    @activo bit,
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE organizacion.grupo_empresarial_empresa
    SET [id_empresa] = @id_empresa,
        [activo] = @activo,
        [creado_utc] = @creado_utc
    WHERE [id_grupo_empresarial] = @id_grupo_empresarial;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_empresa_desactivar
    @id_grupo_empresarial bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE organizacion.grupo_empresarial_empresa
    SET [activo] = 0
    WHERE [id_grupo_empresarial] = @id_grupo_empresarial;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
