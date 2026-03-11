CREATE OR ALTER PROCEDURE plataforma.usp_job_sistema_ejecucion_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_ejecucion], [id_job], [fecha_inicio], [fecha_fin], [estado], [mensaje]
    FROM plataforma.job_sistema_ejecucion;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_job_sistema_ejecucion_obtener
    @id_ejecucion bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_ejecucion], [id_job], [fecha_inicio], [fecha_fin], [estado], [mensaje]
    FROM plataforma.job_sistema_ejecucion
    WHERE [id_ejecucion] = @id_ejecucion;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_job_sistema_ejecucion_crear
    @id_job bigint,
    @fecha_inicio datetime2,
    @fecha_fin datetime2,
    @estado varchar(50),
    @mensaje nvarchar(max)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO plataforma.job_sistema_ejecucion ([id_job], [fecha_inicio], [fecha_fin], [estado], [mensaje])
    VALUES (@id_job, @fecha_inicio, @fecha_fin, @estado, @mensaje);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_job_sistema_ejecucion_actualizar
    @id_ejecucion bigint,
    @id_job bigint,
    @fecha_inicio datetime2,
    @fecha_fin datetime2,
    @estado varchar(50),
    @mensaje nvarchar(max)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.job_sistema_ejecucion
    SET [id_job] = @id_job,
        [fecha_inicio] = @fecha_inicio,
        [fecha_fin] = @fecha_fin,
        [estado] = @estado,
        [mensaje] = @mensaje
    WHERE [id_ejecucion] = @id_ejecucion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_job_sistema_ejecucion_desactivar
    @id_ejecucion bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM plataforma.job_sistema_ejecucion
    WHERE [id_ejecucion] = @id_ejecucion;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
