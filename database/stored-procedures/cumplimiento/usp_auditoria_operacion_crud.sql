CREATE OR ALTER PROCEDURE cumplimiento.usp_auditoria_operacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_auditoria], [tabla], [operacion], [id_registro], [valores_anteriores], [valores_nuevos], [usuario], [correlation_id], [fecha]
    FROM cumplimiento.auditoria_operacion;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_auditoria_operacion_obtener
    @id_auditoria bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_auditoria], [tabla], [operacion], [id_registro], [valores_anteriores], [valores_nuevos], [usuario], [correlation_id], [fecha]
    FROM cumplimiento.auditoria_operacion
    WHERE [id_auditoria] = @id_auditoria;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_auditoria_operacion_crear
    @tabla varchar(100),
    @operacion varchar(50),
    @id_registro bigint,
    @valores_anteriores nvarchar(max),
    @valores_nuevos nvarchar(max),
    @usuario varchar(100),
    @correlation_id uniqueidentifier,
    @fecha datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO cumplimiento.auditoria_operacion ([tabla], [operacion], [id_registro], [valores_anteriores], [valores_nuevos], [usuario], [correlation_id], [fecha])
    VALUES (@tabla, @operacion, @id_registro, @valores_anteriores, @valores_nuevos, @usuario, @correlation_id, @fecha);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_auditoria_operacion_actualizar
    @id_auditoria bigint,
    @tabla varchar(100),
    @operacion varchar(50),
    @id_registro bigint,
    @valores_anteriores nvarchar(max),
    @valores_nuevos nvarchar(max),
    @usuario varchar(100),
    @correlation_id uniqueidentifier,
    @fecha datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE cumplimiento.auditoria_operacion
    SET [tabla] = @tabla,
        [operacion] = @operacion,
        [id_registro] = @id_registro,
        [valores_anteriores] = @valores_anteriores,
        [valores_nuevos] = @valores_nuevos,
        [usuario] = @usuario,
        [correlation_id] = @correlation_id,
        [fecha] = @fecha
    WHERE [id_auditoria] = @id_auditoria;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE cumplimiento.usp_auditoria_operacion_desactivar
    @id_auditoria bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM cumplimiento.auditoria_operacion
    WHERE [id_auditoria] = @id_auditoria;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
