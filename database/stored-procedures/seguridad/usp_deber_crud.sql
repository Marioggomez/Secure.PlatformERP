CREATE OR ALTER PROCEDURE seguridad.usp_deber_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_deber], [id_tenant], [codigo], [nombre], [descripcion], [es_sistema], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM seguridad.deber;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_deber_obtener
    @id_deber bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_deber], [id_tenant], [codigo], [nombre], [descripcion], [es_sistema], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM seguridad.deber
    WHERE [id_deber] = @id_deber;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_deber_crear
    @id_tenant bigint,
    @codigo nvarchar(120),
    @nombre nvarchar(150),
    @descripcion nvarchar(300),
    @es_sistema bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.deber ([id_tenant], [codigo], [nombre], [descripcion], [es_sistema], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @codigo, @nombre, @descripcion, @es_sistema, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_deber_actualizar
    @id_deber bigint,
    @id_tenant bigint,
    @codigo nvarchar(120),
    @nombre nvarchar(150),
    @descripcion nvarchar(300),
    @es_sistema bit,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.deber
    SET [id_tenant] = @id_tenant,
        [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [es_sistema] = @es_sistema,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_deber] = @id_deber;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_deber_desactivar
    @id_deber bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.deber
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_deber] = @id_deber;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
