CREATE OR ALTER PROCEDURE seguridad.usp_politica_ip_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_politica_ip], [id_tenant], [id_empresa], [ip_o_cidr], [accion], [prioridad], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.politica_ip;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_ip_obtener
    @id_politica_ip bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_politica_ip], [id_tenant], [id_empresa], [ip_o_cidr], [accion], [prioridad], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.politica_ip
    WHERE [id_politica_ip] = @id_politica_ip;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_ip_crear
    @id_tenant bigint,
    @id_empresa bigint,
    @ip_o_cidr nvarchar(64),
    @accion varchar(10),
    @prioridad int,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.politica_ip ([id_tenant], [id_empresa], [ip_o_cidr], [accion], [prioridad], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @id_empresa, @ip_o_cidr, @accion, @prioridad, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_ip_actualizar
    @id_politica_ip bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @ip_o_cidr nvarchar(64),
    @accion varchar(10),
    @prioridad int,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.politica_ip
    SET [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [ip_o_cidr] = @ip_o_cidr,
        [accion] = @accion,
        [prioridad] = @prioridad,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_politica_ip] = @id_politica_ip;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_politica_ip_desactivar
    @id_politica_ip bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.politica_ip
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_politica_ip] = @id_politica_ip;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
