CREATE OR ALTER PROCEDURE cumplimiento.usp_paso_instancia_aprobacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_paso_instancia_aprobacion], [id_instancia_aprobacion], [nivel_orden], [id_estado_aprobacion], [iniciado_utc], [resuelto_utc]
    FROM cumplimiento.paso_instancia_aprobacion;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_paso_instancia_aprobacion_obtener
    @id_paso_instancia_aprobacion bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_paso_instancia_aprobacion], [id_instancia_aprobacion], [nivel_orden], [id_estado_aprobacion], [iniciado_utc], [resuelto_utc]
    FROM cumplimiento.paso_instancia_aprobacion
    WHERE [id_paso_instancia_aprobacion] = @id_paso_instancia_aprobacion;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_paso_instancia_aprobacion_crear
    @id_instancia_aprobacion bigint,
    @nivel_orden tinyint,
    @id_estado_aprobacion smallint,
    @iniciado_utc datetime2,
    @resuelto_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO cumplimiento.paso_instancia_aprobacion ([id_instancia_aprobacion], [nivel_orden], [id_estado_aprobacion], [iniciado_utc], [resuelto_utc])
    VALUES (@id_instancia_aprobacion, @nivel_orden, @id_estado_aprobacion, @iniciado_utc, @resuelto_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_paso_instancia_aprobacion_actualizar
    @id_paso_instancia_aprobacion bigint,
    @id_instancia_aprobacion bigint,
    @nivel_orden tinyint,
    @id_estado_aprobacion smallint,
    @iniciado_utc datetime2,
    @resuelto_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cumplimiento.paso_instancia_aprobacion
    SET [id_instancia_aprobacion] = @id_instancia_aprobacion,
        [nivel_orden] = @nivel_orden,
        [id_estado_aprobacion] = @id_estado_aprobacion,
        [iniciado_utc] = @iniciado_utc,
        [resuelto_utc] = @resuelto_utc
    WHERE [id_paso_instancia_aprobacion] = @id_paso_instancia_aprobacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_paso_instancia_aprobacion_desactivar
    @id_paso_instancia_aprobacion bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM cumplimiento.paso_instancia_aprobacion
    WHERE [id_paso_instancia_aprobacion] = @id_paso_instancia_aprobacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
