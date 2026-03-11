CREATE OR ALTER PROCEDURE organizacion.usp_relacion_empresa_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_relacion_empresa], [id_tenant], [id_empresa_origen], [id_empresa_destino], [id_tipo_relacion_empresa], [fecha_inicio_utc], [fecha_fin_utc], [observacion], [activo], [creado_utc], [actualizado_utc]
    FROM organizacion.relacion_empresa;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_relacion_empresa_obtener
    @id_relacion_empresa bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_relacion_empresa], [id_tenant], [id_empresa_origen], [id_empresa_destino], [id_tipo_relacion_empresa], [fecha_inicio_utc], [fecha_fin_utc], [observacion], [activo], [creado_utc], [actualizado_utc]
    FROM organizacion.relacion_empresa
    WHERE [id_relacion_empresa] = @id_relacion_empresa;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_relacion_empresa_crear
    @id_tenant bigint,
    @id_empresa_origen bigint,
    @id_empresa_destino bigint,
    @id_tipo_relacion_empresa smallint,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @observacion nvarchar(500),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO organizacion.relacion_empresa ([id_tenant], [id_empresa_origen], [id_empresa_destino], [id_tipo_relacion_empresa], [fecha_inicio_utc], [fecha_fin_utc], [observacion], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @id_empresa_origen, @id_empresa_destino, @id_tipo_relacion_empresa, @fecha_inicio_utc, @fecha_fin_utc, @observacion, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_relacion_empresa_actualizar
    @id_relacion_empresa bigint,
    @id_tenant bigint,
    @id_empresa_origen bigint,
    @id_empresa_destino bigint,
    @id_tipo_relacion_empresa smallint,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @observacion nvarchar(500),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE organizacion.relacion_empresa
    SET [id_tenant] = @id_tenant,
        [id_empresa_origen] = @id_empresa_origen,
        [id_empresa_destino] = @id_empresa_destino,
        [id_tipo_relacion_empresa] = @id_tipo_relacion_empresa,
        [fecha_inicio_utc] = @fecha_inicio_utc,
        [fecha_fin_utc] = @fecha_fin_utc,
        [observacion] = @observacion,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_relacion_empresa] = @id_relacion_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_relacion_empresa_desactivar
    @id_relacion_empresa bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE organizacion.relacion_empresa
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_relacion_empresa] = @id_relacion_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
