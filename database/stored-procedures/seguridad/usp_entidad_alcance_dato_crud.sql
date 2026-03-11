CREATE OR ALTER PROCEDURE seguridad.usp_entidad_alcance_dato_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [codigo_entidad], [nombre_tabla], [columna_llave_primaria], [columna_tenant], [columna_empresa], [columna_unidad_organizativa], [columna_propietario], [columna_contexto], [descripcion], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.entidad_alcance_dato;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_entidad_alcance_dato_obtener
    @codigo_entidad nvarchar(128)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [codigo_entidad], [nombre_tabla], [columna_llave_primaria], [columna_tenant], [columna_empresa], [columna_unidad_organizativa], [columna_propietario], [columna_contexto], [descripcion], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.entidad_alcance_dato
    WHERE [codigo_entidad] = @codigo_entidad;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_entidad_alcance_dato_crear
    @codigo_entidad nvarchar(128),
    @nombre_tabla nvarchar(128),
    @columna_llave_primaria nvarchar(128),
    @columna_tenant nvarchar(128),
    @columna_empresa nvarchar(128),
    @columna_unidad_organizativa nvarchar(128),
    @columna_propietario nvarchar(128),
    @columna_contexto nvarchar(128),
    @descripcion nvarchar(256),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.entidad_alcance_dato ([codigo_entidad], [nombre_tabla], [columna_llave_primaria], [columna_tenant], [columna_empresa], [columna_unidad_organizativa], [columna_propietario], [columna_contexto], [descripcion], [activo], [creado_utc], [actualizado_utc])
    VALUES (@codigo_entidad, @nombre_tabla, @columna_llave_primaria, @columna_tenant, @columna_empresa, @columna_unidad_organizativa, @columna_propietario, @columna_contexto, @descripcion, @activo, @creado_utc, @actualizado_utc);
    SELECT @codigo_entidad AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_entidad_alcance_dato_actualizar
    @codigo_entidad nvarchar(128),
    @nombre_tabla nvarchar(128),
    @columna_llave_primaria nvarchar(128),
    @columna_tenant nvarchar(128),
    @columna_empresa nvarchar(128),
    @columna_unidad_organizativa nvarchar(128),
    @columna_propietario nvarchar(128),
    @columna_contexto nvarchar(128),
    @descripcion nvarchar(256),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.entidad_alcance_dato
    SET [nombre_tabla] = @nombre_tabla,
        [columna_llave_primaria] = @columna_llave_primaria,
        [columna_tenant] = @columna_tenant,
        [columna_empresa] = @columna_empresa,
        [columna_unidad_organizativa] = @columna_unidad_organizativa,
        [columna_propietario] = @columna_propietario,
        [columna_contexto] = @columna_contexto,
        [descripcion] = @descripcion,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [codigo_entidad] = @codigo_entidad;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_entidad_alcance_dato_desactivar
    @codigo_entidad nvarchar(128),
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.entidad_alcance_dato
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [codigo_entidad] = @codigo_entidad;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
