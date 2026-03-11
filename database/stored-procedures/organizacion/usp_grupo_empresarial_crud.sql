CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_grupo_empresarial], [id_tenant], [codigo], [nombre], [descripcion], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM organizacion.grupo_empresarial;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_obtener
    @id_grupo_empresarial bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_grupo_empresarial], [id_tenant], [codigo], [nombre], [descripcion], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM organizacion.grupo_empresarial
    WHERE [id_grupo_empresarial] = @id_grupo_empresarial;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_crear
    @id_tenant bigint,
    @codigo nvarchar(50),
    @nombre nvarchar(200),
    @descripcion nvarchar(300),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO organizacion.grupo_empresarial ([id_tenant], [codigo], [nombre], [descripcion], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @codigo, @nombre, @descripcion, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_actualizar
    @id_grupo_empresarial bigint,
    @id_tenant bigint,
    @codigo nvarchar(50),
    @nombre nvarchar(200),
    @descripcion nvarchar(300),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE organizacion.grupo_empresarial
    SET [id_tenant] = @id_tenant,
        [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_grupo_empresarial] = @id_grupo_empresarial;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_grupo_empresarial_desactivar
    @id_grupo_empresarial bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE organizacion.grupo_empresarial
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_grupo_empresarial] = @id_grupo_empresarial;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
