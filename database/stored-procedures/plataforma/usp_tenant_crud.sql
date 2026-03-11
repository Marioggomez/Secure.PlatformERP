CREATE OR ALTER PROCEDURE plataforma.usp_tenant_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tenant], [codigo], [nombre], [descripcion], [dominio_principal], [activo], [creado_utc], [actualizado_utc], [version_fila], [es_entrenamiento]
    FROM plataforma.tenant;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_tenant_obtener
    @id_tenant bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tenant], [codigo], [nombre], [descripcion], [dominio_principal], [activo], [creado_utc], [actualizado_utc], [version_fila], [es_entrenamiento]
    FROM plataforma.tenant
    WHERE [id_tenant] = @id_tenant;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_tenant_crear
    @codigo nvarchar(50),
    @nombre nvarchar(200),
    @descripcion nvarchar(500),
    @dominio_principal nvarchar(200),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2,
    @es_entrenamiento bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO plataforma.tenant ([codigo], [nombre], [descripcion], [dominio_principal], [activo], [creado_utc], [actualizado_utc], [es_entrenamiento])
    VALUES (@codigo, @nombre, @descripcion, @dominio_principal, @activo, @creado_utc, @actualizado_utc, @es_entrenamiento);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_tenant_actualizar
    @id_tenant bigint,
    @codigo nvarchar(50),
    @nombre nvarchar(200),
    @descripcion nvarchar(500),
    @dominio_principal nvarchar(200),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2,
    @es_entrenamiento bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.tenant
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [dominio_principal] = @dominio_principal,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc,
        [es_entrenamiento] = @es_entrenamiento
    WHERE [id_tenant] = @id_tenant;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_tenant_desactivar
    @id_tenant bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.tenant
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_tenant] = @id_tenant;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
