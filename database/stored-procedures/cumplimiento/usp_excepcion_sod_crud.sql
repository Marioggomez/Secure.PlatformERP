CREATE OR ALTER PROCEDURE cumplimiento.usp_excepcion_sod_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_excepcion_sod], [id_regla_sod], [id_usuario], [id_empresa], [fecha_inicio_utc], [fecha_fin_utc], [motivo], [aprobado_por], [activo], [creado_utc]
    FROM cumplimiento.excepcion_sod;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_excepcion_sod_obtener
    @id_excepcion_sod bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_excepcion_sod], [id_regla_sod], [id_usuario], [id_empresa], [fecha_inicio_utc], [fecha_fin_utc], [motivo], [aprobado_por], [activo], [creado_utc]
    FROM cumplimiento.excepcion_sod
    WHERE [id_excepcion_sod] = @id_excepcion_sod;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_excepcion_sod_crear
    @id_regla_sod bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @motivo nvarchar(300),
    @aprobado_por bigint,
    @activo bit,
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO cumplimiento.excepcion_sod ([id_regla_sod], [id_usuario], [id_empresa], [fecha_inicio_utc], [fecha_fin_utc], [motivo], [aprobado_por], [activo], [creado_utc])
    VALUES (@id_regla_sod, @id_usuario, @id_empresa, @fecha_inicio_utc, @fecha_fin_utc, @motivo, @aprobado_por, @activo, @creado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_excepcion_sod_actualizar
    @id_excepcion_sod bigint,
    @id_regla_sod bigint,
    @id_usuario bigint,
    @id_empresa bigint,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @motivo nvarchar(300),
    @aprobado_por bigint,
    @activo bit,
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cumplimiento.excepcion_sod
    SET [id_regla_sod] = @id_regla_sod,
        [id_usuario] = @id_usuario,
        [id_empresa] = @id_empresa,
        [fecha_inicio_utc] = @fecha_inicio_utc,
        [fecha_fin_utc] = @fecha_fin_utc,
        [motivo] = @motivo,
        [aprobado_por] = @aprobado_por,
        [activo] = @activo,
        [creado_utc] = @creado_utc
    WHERE [id_excepcion_sod] = @id_excepcion_sod;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_excepcion_sod_desactivar
    @id_excepcion_sod bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cumplimiento.excepcion_sod
    SET [activo] = 0
    WHERE [id_excepcion_sod] = @id_excepcion_sod;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
