CREATE OR ALTER PROCEDURE cumplimiento.usp_paso_perfil_aprobacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_paso_perfil_aprobacion], [id_perfil_aprobacion], [nivel_orden], [id_rol_requerido], [id_privilegio_requerido], [id_alcance_asignacion_requerido], [aprobadores_minimos], [todos_deben_aprobar], [monto_minimo], [monto_maximo], [activo]
    FROM cumplimiento.paso_perfil_aprobacion;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_paso_perfil_aprobacion_obtener
    @id_paso_perfil_aprobacion bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_paso_perfil_aprobacion], [id_perfil_aprobacion], [nivel_orden], [id_rol_requerido], [id_privilegio_requerido], [id_alcance_asignacion_requerido], [aprobadores_minimos], [todos_deben_aprobar], [monto_minimo], [monto_maximo], [activo]
    FROM cumplimiento.paso_perfil_aprobacion
    WHERE [id_paso_perfil_aprobacion] = @id_paso_perfil_aprobacion;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_paso_perfil_aprobacion_crear
    @id_perfil_aprobacion bigint,
    @nivel_orden tinyint,
    @id_rol_requerido bigint,
    @id_privilegio_requerido bigint,
    @id_alcance_asignacion_requerido smallint,
    @aprobadores_minimos tinyint,
    @todos_deben_aprobar bit,
    @monto_minimo decimal(18,2),
    @monto_maximo decimal(18,2),
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO cumplimiento.paso_perfil_aprobacion ([id_perfil_aprobacion], [nivel_orden], [id_rol_requerido], [id_privilegio_requerido], [id_alcance_asignacion_requerido], [aprobadores_minimos], [todos_deben_aprobar], [monto_minimo], [monto_maximo], [activo])
    VALUES (@id_perfil_aprobacion, @nivel_orden, @id_rol_requerido, @id_privilegio_requerido, @id_alcance_asignacion_requerido, @aprobadores_minimos, @todos_deben_aprobar, @monto_minimo, @monto_maximo, @activo);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_paso_perfil_aprobacion_actualizar
    @id_paso_perfil_aprobacion bigint,
    @id_perfil_aprobacion bigint,
    @nivel_orden tinyint,
    @id_rol_requerido bigint,
    @id_privilegio_requerido bigint,
    @id_alcance_asignacion_requerido smallint,
    @aprobadores_minimos tinyint,
    @todos_deben_aprobar bit,
    @monto_minimo decimal(18,2),
    @monto_maximo decimal(18,2),
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cumplimiento.paso_perfil_aprobacion
    SET [id_perfil_aprobacion] = @id_perfil_aprobacion,
        [nivel_orden] = @nivel_orden,
        [id_rol_requerido] = @id_rol_requerido,
        [id_privilegio_requerido] = @id_privilegio_requerido,
        [id_alcance_asignacion_requerido] = @id_alcance_asignacion_requerido,
        [aprobadores_minimos] = @aprobadores_minimos,
        [todos_deben_aprobar] = @todos_deben_aprobar,
        [monto_minimo] = @monto_minimo,
        [monto_maximo] = @monto_maximo,
        [activo] = @activo
    WHERE [id_paso_perfil_aprobacion] = @id_paso_perfil_aprobacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_paso_perfil_aprobacion_desactivar
    @id_paso_perfil_aprobacion bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cumplimiento.paso_perfil_aprobacion
    SET [activo] = 0
    WHERE [id_paso_perfil_aprobacion] = @id_paso_perfil_aprobacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
