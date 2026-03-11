CREATE OR ALTER PROCEDURE seguridad.usp_privilegio_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_privilegio], [id_tenant], [codigo], [nombre], [descripcion], [es_sistema], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM seguridad.privilegio;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_privilegio_obtener
    @id_privilegio bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_privilegio], [id_tenant], [codigo], [nombre], [descripcion], [es_sistema], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM seguridad.privilegio
    WHERE [id_privilegio] = @id_privilegio;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_privilegio_crear
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
    INSERT INTO seguridad.privilegio ([id_tenant], [codigo], [nombre], [descripcion], [es_sistema], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @codigo, @nombre, @descripcion, @es_sistema, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_privilegio_actualizar
    @id_privilegio bigint,
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
    UPDATE seguridad.privilegio
    SET [id_tenant] = @id_tenant,
        [codigo] = @codigo,
        [nombre] = @nombre,
        [descripcion] = @descripcion,
        [es_sistema] = @es_sistema,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_privilegio] = @id_privilegio;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_privilegio_desactivar
    @id_privilegio bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.privilegio
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_privilegio] = @id_privilegio;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
