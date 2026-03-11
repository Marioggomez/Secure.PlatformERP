CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_filtro_dato_usuario], [id_usuario], [id_tenant], [id_empresa], [codigo_entidad], [id_modo_filtro_dato], [valor_filtro], [id_unidad_organizativa], [fecha_inicio_utc], [fecha_fin_utc], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.filtro_dato_usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_obtener
    @id_filtro_dato_usuario bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_filtro_dato_usuario], [id_usuario], [id_tenant], [id_empresa], [codigo_entidad], [id_modo_filtro_dato], [valor_filtro], [id_unidad_organizativa], [fecha_inicio_utc], [fecha_fin_utc], [activo], [creado_utc], [actualizado_utc]
    FROM seguridad.filtro_dato_usuario
    WHERE [id_filtro_dato_usuario] = @id_filtro_dato_usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_crear
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @codigo_entidad nvarchar(128),
    @id_modo_filtro_dato smallint,
    @valor_filtro nvarchar(150),
    @id_unidad_organizativa bigint,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.filtro_dato_usuario ([id_usuario], [id_tenant], [id_empresa], [codigo_entidad], [id_modo_filtro_dato], [valor_filtro], [id_unidad_organizativa], [fecha_inicio_utc], [fecha_fin_utc], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_usuario, @id_tenant, @id_empresa, @codigo_entidad, @id_modo_filtro_dato, @valor_filtro, @id_unidad_organizativa, @fecha_inicio_utc, @fecha_fin_utc, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_actualizar
    @id_filtro_dato_usuario bigint,
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @codigo_entidad nvarchar(128),
    @id_modo_filtro_dato smallint,
    @valor_filtro nvarchar(150),
    @id_unidad_organizativa bigint,
    @fecha_inicio_utc datetime2,
    @fecha_fin_utc datetime2,
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.filtro_dato_usuario
    SET [id_usuario] = @id_usuario,
        [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [codigo_entidad] = @codigo_entidad,
        [id_modo_filtro_dato] = @id_modo_filtro_dato,
        [valor_filtro] = @valor_filtro,
        [id_unidad_organizativa] = @id_unidad_organizativa,
        [fecha_inicio_utc] = @fecha_inicio_utc,
        [fecha_fin_utc] = @fecha_fin_utc,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_filtro_dato_usuario] = @id_filtro_dato_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_desactivar
    @id_filtro_dato_usuario bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.filtro_dato_usuario
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_filtro_dato_usuario] = @id_filtro_dato_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_guardar_layout_ui
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint = NULL,
    @codigo_entidad nvarchar(256),
    @layout_payload nvarchar(300)
AS
BEGIN
    SET NOCOUNT ON;

    IF @id_usuario IS NULL OR @id_tenant IS NULL OR @codigo_entidad IS NULL OR LTRIM(RTRIM(@codigo_entidad)) = ''
    BEGIN
        SELECT 0 AS filas_afectadas;
        RETURN;
    END

    DECLARE @payload nvarchar(300) = LTRIM(RTRIM(ISNULL(@layout_payload, '')));
    IF @payload = ''
    BEGIN
        SELECT 0 AS filas_afectadas;
        RETURN;
    END

    DECLARE @id_modo_filtro_dato smallint;
    SELECT TOP (1) @id_modo_filtro_dato = id_modo_filtro_dato
    FROM catalogo.modo_filtro_dato
    WHERE codigo = 'UI_LAYOUT';

    IF @id_modo_filtro_dato IS NULL
    BEGIN
        INSERT INTO catalogo.modo_filtro_dato (
            codigo,
            nombre,
            descripcion,
            orden_visual,
            activo,
            creado_utc,
            actualizado_utc
        )
        VALUES (
            'UI_LAYOUT',
            N'Layout UI',
            N'Persistencia de layout de interfaz por usuario.',
            900,
            1,
            SYSUTCDATETIME(),
            SYSUTCDATETIME()
        );

        SET @id_modo_filtro_dato = CAST(SCOPE_IDENTITY() AS smallint);
    END

    IF NOT EXISTS (SELECT 1 FROM seguridad.entidad_alcance_dato WHERE codigo_entidad = @codigo_entidad)
    BEGIN
        INSERT INTO seguridad.entidad_alcance_dato (
            codigo_entidad,
            nombre_tabla,
            columna_llave_primaria,
            columna_tenant,
            columna_empresa,
            columna_unidad_organizativa,
            columna_propietario,
            columna_contexto,
            descripcion,
            activo,
            creado_utc,
            actualizado_utc
        )
        VALUES (
            @codigo_entidad,
            N'seguridad.filtro_dato_usuario',
            N'id_filtro_dato_usuario',
            N'id_tenant',
            N'id_empresa',
            N'id_unidad_organizativa',
            N'id_usuario',
            N'codigo_entidad',
            N'Layout UI persistido para formularios de busqueda.',
            1,
            SYSUTCDATETIME(),
            SYSUTCDATETIME()
        );
    END

    DECLARE @id_filtro_dato_usuario bigint;
    SELECT TOP (1) @id_filtro_dato_usuario = id_filtro_dato_usuario
    FROM seguridad.filtro_dato_usuario
    WHERE
        id_usuario = @id_usuario
        AND id_tenant = @id_tenant
        AND codigo_entidad = @codigo_entidad
        AND activo = 1
        AND (
            (@id_empresa IS NULL AND id_empresa IS NULL)
            OR id_empresa = @id_empresa
            OR (@id_empresa IS NOT NULL AND id_empresa IS NULL)
        )
    ORDER BY CASE WHEN id_empresa = @id_empresa THEN 0 ELSE 1 END, id_filtro_dato_usuario DESC;

    IF @id_filtro_dato_usuario IS NULL
    BEGIN
        INSERT INTO seguridad.filtro_dato_usuario (
            id_usuario,
            id_tenant,
            id_empresa,
            codigo_entidad,
            id_modo_filtro_dato,
            valor_filtro,
            id_unidad_organizativa,
            fecha_inicio_utc,
            fecha_fin_utc,
            activo,
            creado_utc,
            actualizado_utc
        )
        VALUES (
            @id_usuario,
            @id_tenant,
            @id_empresa,
            @codigo_entidad,
            @id_modo_filtro_dato,
            @payload,
            NULL,
            SYSUTCDATETIME(),
            NULL,
            1,
            SYSUTCDATETIME(),
            SYSUTCDATETIME()
        );

        SELECT 1 AS filas_afectadas;
        RETURN;
    END

    UPDATE seguridad.filtro_dato_usuario
    SET
        id_empresa = @id_empresa,
        id_modo_filtro_dato = @id_modo_filtro_dato,
        valor_filtro = @payload,
        activo = 1,
        fecha_inicio_utc = CASE WHEN fecha_inicio_utc IS NULL THEN SYSUTCDATETIME() ELSE fecha_inicio_utc END,
        fecha_fin_utc = NULL,
        actualizado_utc = SYSUTCDATETIME()
    WHERE id_filtro_dato_usuario = @id_filtro_dato_usuario;

    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_filtro_dato_usuario_obtener_layout_ui
    @id_usuario bigint,
    @id_tenant bigint,
    @id_empresa bigint = NULL,
    @codigo_entidad nvarchar(256)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        codigo_entidad,
        valor_filtro AS layout_payload,
        actualizado_utc
    FROM seguridad.filtro_dato_usuario
    WHERE
        id_usuario = @id_usuario
        AND id_tenant = @id_tenant
        AND codigo_entidad = @codigo_entidad
        AND activo = 1
        AND (
            (@id_empresa IS NULL AND id_empresa IS NULL)
            OR id_empresa = @id_empresa
            OR (@id_empresa IS NOT NULL AND id_empresa IS NULL)
        )
    ORDER BY
        CASE WHEN id_empresa = @id_empresa THEN 0 ELSE 1 END,
        id_filtro_dato_usuario DESC;
END
GO
