CREATE OR ALTER PROCEDURE plataforma.usp_job_sistema_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_job], [codigo], [nombre], [cron], [activo]
    FROM plataforma.job_sistema;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_job_sistema_obtener
    @id_job bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_job], [codigo], [nombre], [cron], [activo]
    FROM plataforma.job_sistema
    WHERE [id_job] = @id_job;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_job_sistema_crear
    @codigo varchar(100),
    @nombre varchar(200),
    @cron varchar(50),
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO plataforma.job_sistema ([codigo], [nombre], [cron], [activo])
    VALUES (@codigo, @nombre, @cron, @activo);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_job_sistema_actualizar
    @id_job bigint,
    @codigo varchar(100),
    @nombre varchar(200),
    @cron varchar(50),
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.job_sistema
    SET [codigo] = @codigo,
        [nombre] = @nombre,
        [cron] = @cron,
        [activo] = @activo
    WHERE [id_job] = @id_job;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_job_sistema_desactivar
    @id_job bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.job_sistema
    SET [activo] = 0
    WHERE [id_job] = @id_job;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
