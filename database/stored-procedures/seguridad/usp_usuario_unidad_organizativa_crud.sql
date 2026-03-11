CREATE OR ALTER PROCEDURE seguridad.usp_usuario_unidad_organizativa_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario_unidad_organizativa], [id_usuario], [id_empresa], [id_unidad_organizativa], [rol_operativo], [fecha_inicio_utc], [fecha_fin_utc], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.usuario_unidad_organizativa;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_unidad_organizativa_obtener
    @id_usuario_unidad_organizativa bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario_unidad_organizativa], [id_usuario], [id_empresa], [id_unidad_organizativa], [rol_operativo], [fecha_inicio_utc], [fecha_fin_utc], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.usuario_unidad_organizativa
    WHERE [id_usuario_unidad_organizativa] = @id_usuario_unidad_organizativa;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_unidad_organizativa_crear
    @id_usuario bigint,
    @id_empresa bigint,
    @id_unidad_organizativa bigint,
    @rol_operativo nvarchar(50),
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.usuario_unidad_organizativa ([id_usuario], [id_empresa], [id_unidad_organizativa], [rol_operativo], [fecha_inicio_utc], [fecha_fin_utc], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_usuario, @id_empresa, @id_unidad_organizativa, @rol_operativo, @fecha_inicio_utc, @fecha_fin_utc, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_unidad_organizativa_actualizar
    @id_usuario_unidad_organizativa bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @id_unidad_organizativa bigint,
    @rol_operativo nvarchar(50),
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.usuario_unidad_organizativa
    SET [id_usuario] = @id_usuario,
        [id_empresa] = @id_empresa,
        [id_unidad_organizativa] = @id_unidad_organizativa,
        [rol_operativo] = @rol_operativo,
        [fecha_inicio_utc] = @fecha_inicio_utc,
        [fecha_fin_utc] = @fecha_fin_utc,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_usuario_unidad_organizativa] = @id_usuario_unidad_organizativa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_unidad_organizativa_desactivar
    @id_usuario_unidad_organizativa bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.usuario_unidad_organizativa
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_usuario_unidad_organizativa] = @id_usuario_unidad_organizativa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
