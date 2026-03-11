CREATE OR ALTER PROCEDURE organizacion.usp_unidad_organizativa_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_unidad_organizativa], [id_tenant], [id_empresa], [id_tipo_unidad_organizativa], [id_unidad_padre], [codigo], [nombre], [nivel_jerarquia], [ruta_jerarquia], [es_hoja], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM organizacion.unidad_organizativa;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_unidad_organizativa_obtener
    @id_unidad_organizativa bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_unidad_organizativa], [id_tenant], [id_empresa], [id_tipo_unidad_organizativa], [id_unidad_padre], [codigo], [nombre], [nivel_jerarquia], [ruta_jerarquia], [es_hoja], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM organizacion.unidad_organizativa
    WHERE [id_unidad_organizativa] = @id_unidad_organizativa;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_unidad_organizativa_crear
    @id_tenant bigint,
    @id_empresa bigint,
    @id_tipo_unidad_organizativa smallint,
    @id_unidad_padre bigint,
    @codigo nvarchar(60),
    @nombre nvarchar(200),
    @nivel_jerarquia smallint,
    @ruta_jerarquia nvarchar(500),
    @es_hoja bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO organizacion.unidad_organizativa ([id_tenant], [id_empresa], [id_tipo_unidad_organizativa], [id_unidad_padre], [codigo], [nombre], [nivel_jerarquia], [ruta_jerarquia], [es_hoja], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @id_empresa, @id_tipo_unidad_organizativa, @id_unidad_padre, @codigo, @nombre, @nivel_jerarquia, @ruta_jerarquia, @es_hoja, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_unidad_organizativa_actualizar
    @id_unidad_organizativa bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @id_tipo_unidad_organizativa smallint,
    @id_unidad_padre bigint,
    @codigo nvarchar(60),
    @nombre nvarchar(200),
    @nivel_jerarquia smallint,
    @ruta_jerarquia nvarchar(500),
    @es_hoja bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE organizacion.unidad_organizativa
    SET [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [id_tipo_unidad_organizativa] = @id_tipo_unidad_organizativa,
        [id_unidad_padre] = @id_unidad_padre,
        [codigo] = @codigo,
        [nombre] = @nombre,
        [nivel_jerarquia] = @nivel_jerarquia,
        [ruta_jerarquia] = @ruta_jerarquia,
        [es_hoja] = @es_hoja,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_unidad_organizativa] = @id_unidad_organizativa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_unidad_organizativa_desactivar
    @id_unidad_organizativa bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE organizacion.unidad_organizativa
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_unidad_organizativa] = @id_unidad_organizativa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
