CREATE OR ALTER PROCEDURE plataforma.usp_bitacora_instalacion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_bitacora_instalacion], [componente], [accion], [estado], [detalle], [iniciado_utc], [finalizado_utc]
    FROM plataforma.bitacora_instalacion;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_bitacora_instalacion_obtener
    @id_bitacora_instalacion bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_bitacora_instalacion], [componente], [accion], [estado], [detalle], [iniciado_utc], [finalizado_utc]
    FROM plataforma.bitacora_instalacion
    WHERE [id_bitacora_instalacion] = @id_bitacora_instalacion;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_bitacora_instalacion_crear
    @componente nvarchar(100),
    @accion nvarchar(50),
    @estado nvarchar(30),
    @detalle nvarchar(max),
    @iniciado_utc datetime2,
    @finalizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO plataforma.bitacora_instalacion ([componente], [accion], [estado], [detalle], [iniciado_utc], [finalizado_utc])
    VALUES (@componente, @accion, @estado, @detalle, @iniciado_utc, @finalizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_bitacora_instalacion_actualizar
    @id_bitacora_instalacion bigint,
    @componente nvarchar(100),
    @accion nvarchar(50),
    @estado nvarchar(30),
    @detalle nvarchar(max),
    @iniciado_utc datetime2,
    @finalizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.bitacora_instalacion
    SET [componente] = @componente,
        [accion] = @accion,
        [estado] = @estado,
        [detalle] = @detalle,
        [iniciado_utc] = @iniciado_utc,
        [finalizado_utc] = @finalizado_utc
    WHERE [id_bitacora_instalacion] = @id_bitacora_instalacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_bitacora_instalacion_desactivar
    @id_bitacora_instalacion bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM plataforma.bitacora_instalacion
    WHERE [id_bitacora_instalacion] = @id_bitacora_instalacion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
