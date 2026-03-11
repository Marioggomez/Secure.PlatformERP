CREATE OR ALTER PROCEDURE seguridad.usp_recurso_ui_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_recurso_ui], [codigo], [nombre], [id_tipo_recurso_ui], [ruta], [componente], [icono], [id_recurso_ui_padre], [orden_visual], [es_visible], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM seguridad.recurso_ui;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_recurso_ui_obtener
    @id_recurso_ui bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_recurso_ui], [codigo], [nombre], [id_tipo_recurso_ui], [ruta], [componente], [icono], [id_recurso_ui_padre], [orden_visual], [es_visible], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM seguridad.recurso_ui
    WHERE [id_recurso_ui] = @id_recurso_ui;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_recurso_ui_crear
    @codigo nvarchar(120),
    @nombre nvarchar(200),
    @id_tipo_recurso_ui smallint,
    @ruta nvarchar(300),
    @componente nvarchar(200),
    @icono nvarchar(100),
    @id_recurso_ui_padre bigint,
    @orden_visual int,
    @es_visible bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.recurso_ui ([codigo], [nombre], [id_tipo_recurso_ui], [ruta], [componente], [icono], [id_recurso_ui_padre], [orden_visual], [es_visible], [activo], [creado_utc], [actualizado_utc])
    VALUES (@codigo, @nombre, @id_tipo_recurso_ui, @ruta, @componente, @icono, @id_recurso_ui_padre, @orden_visual, @es_visible, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_recurso_ui_actualizar
    @id_recurso_ui bigint,
    @codigo nvarchar(120),
    @nombre nvarchar(200),
    @id_tipo_recurso_ui smallint,
    @ruta nvarchar(300),
    @componente nvarchar(200),
    @icono nvarchar(100),
    @id_recurso_ui_padre bigint,
    @orden_visual int,
    @es_visible bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.recurso_ui
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [id_tipo_recurso_ui] = @id_tipo_recurso_ui,
        [ruta] = @ruta,
        [componente] = @componente,
        [icono] = @icono,
        [id_recurso_ui_padre] = @id_recurso_ui_padre,
        [orden_visual] = @orden_visual,
        [es_visible] = @es_visible,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_recurso_ui] = @id_recurso_ui;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_recurso_ui_desactivar
    @id_recurso_ui bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.recurso_ui
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_recurso_ui] = @id_recurso_ui;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
