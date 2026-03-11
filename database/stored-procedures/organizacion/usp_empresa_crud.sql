CREATE OR ALTER PROCEDURE organizacion.usp_empresa_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_empresa], [id_tenant], [codigo], [nombre], [nombre_legal], [id_tipo_empresa], [id_estado_empresa], [identificacion_fiscal], [moneda_base], [zona_horaria], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM organizacion.empresa;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_empresa_obtener
    @id_empresa bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_empresa], [id_tenant], [codigo], [nombre], [nombre_legal], [id_tipo_empresa], [id_estado_empresa], [identificacion_fiscal], [moneda_base], [zona_horaria], [activo], [creado_utc], [actualizado_utc], [version_fila]
    FROM organizacion.empresa
    WHERE [id_empresa] = @id_empresa;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_empresa_crear
    @id_tenant bigint,
    @codigo nvarchar(50),
    @nombre nvarchar(250),
    @nombre_legal nvarchar(300),
    @id_tipo_empresa smallint,
    @id_estado_empresa smallint,
    @identificacion_fiscal nvarchar(50),
    @moneda_base char(3),
    @zona_horaria nvarchar(80),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO organizacion.empresa ([id_tenant], [codigo], [nombre], [nombre_legal], [id_tipo_empresa], [id_estado_empresa], [identificacion_fiscal], [moneda_base], [zona_horaria], [activo], [creado_utc], [actualizado_utc])
    VALUES (@id_tenant, @codigo, @nombre, @nombre_legal, @id_tipo_empresa, @id_estado_empresa, @identificacion_fiscal, @moneda_base, @zona_horaria, @activo, @creado_utc, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_empresa_actualizar
    @id_empresa bigint,
    @id_tenant bigint,
    @codigo nvarchar(50),
    @nombre nvarchar(250),
    @nombre_legal nvarchar(300),
    @id_tipo_empresa smallint,
    @id_estado_empresa smallint,
    @identificacion_fiscal nvarchar(50),
    @moneda_base char(3),
    @zona_horaria nvarchar(80),
    @activo bit,
    @creado_utc datetime2,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE organizacion.empresa
    SET [id_tenant] = @id_tenant,
        [codigo] = @codigo,
        [nombre] = @nombre,
        [nombre_legal] = @nombre_legal,
        [id_tipo_empresa] = @id_tipo_empresa,
        [id_estado_empresa] = @id_estado_empresa,
        [identificacion_fiscal] = @identificacion_fiscal,
        [moneda_base] = @moneda_base,
        [zona_horaria] = @zona_horaria,
        [activo] = @activo,
        [creado_utc] = @creado_utc,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_empresa] = @id_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_empresa_desactivar
    @id_empresa bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE organizacion.empresa
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_empresa] = @id_empresa;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_empresa_listar_paginado
    @page int = 1,
    @size int = 25,
    @sort_by nvarchar(64) = N'id_empresa',
    @sort_dir varchar(4) = 'ASC',
    @filter nvarchar(200) = NULL,
    @filter_field nvarchar(64) = NULL,
    @id_tenant bigint = NULL,
    @total_registros int OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @page < 1 SET @page = 1;
    IF @size < 5 SET @size = 25;
    IF @size > 500 SET @size = 500;

    SET @sort_by = LOWER(LTRIM(RTRIM(ISNULL(@sort_by, 'id_empresa'))));
    SET @sort_dir = UPPER(LTRIM(RTRIM(ISNULL(@sort_dir, 'ASC'))));
    IF @sort_dir NOT IN ('ASC', 'DESC') SET @sort_dir = 'ASC';

    DECLARE @offset int = (@page - 1) * @size;

    ;WITH base AS (
        SELECT
            e.id_empresa,
            e.id_tenant,
            e.codigo,
            e.nombre,
            e.nombre_legal,
            e.identificacion_fiscal,
            COALESCE(ee.codigo, CAST(e.id_estado_empresa AS varchar(20))) AS estado,
            e.activo
        FROM organizacion.empresa AS e
        LEFT JOIN catalogo.estado_empresa AS ee
            ON ee.id_estado_empresa = e.id_estado_empresa
        WHERE
            (@id_tenant IS NULL OR e.id_tenant = @id_tenant)
            AND (
                @filter IS NULL
                OR LTRIM(RTRIM(@filter)) = ''
                OR (
                    (@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'codigo')
                    AND e.codigo LIKE '%' + @filter + '%'
                )
                OR (
                    (@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'nombre')
                    AND e.nombre LIKE '%' + @filter + '%'
                )
                OR (
                    (@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'nombre_legal')
                    AND e.nombre_legal LIKE '%' + @filter + '%'
                )
                OR (
                    (@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'identificacion_fiscal')
                    AND e.identificacion_fiscal LIKE '%' + @filter + '%'
                )
                OR (
                    (@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'estado')
                    AND COALESCE(ee.codigo, CAST(e.id_estado_empresa AS varchar(20))) LIKE '%' + @filter + '%'
                )
            )
    )
    SELECT @total_registros = COUNT(1)
    FROM base;

    SELECT
        id_empresa,
        id_tenant,
        codigo,
        nombre,
        nombre_legal,
        identificacion_fiscal,
        estado,
        activo
    FROM base
    ORDER BY
        CASE WHEN @sort_by = 'id_empresa' AND @sort_dir = 'ASC' THEN id_empresa END ASC,
        CASE WHEN @sort_by = 'id_empresa' AND @sort_dir = 'DESC' THEN id_empresa END DESC,
        CASE WHEN @sort_by = 'codigo' AND @sort_dir = 'ASC' THEN codigo END ASC,
        CASE WHEN @sort_by = 'codigo' AND @sort_dir = 'DESC' THEN codigo END DESC,
        CASE WHEN @sort_by = 'nombre' AND @sort_dir = 'ASC' THEN nombre END ASC,
        CASE WHEN @sort_by = 'nombre' AND @sort_dir = 'DESC' THEN nombre END DESC,
        CASE WHEN @sort_by = 'nombre_legal' AND @sort_dir = 'ASC' THEN nombre_legal END ASC,
        CASE WHEN @sort_by = 'nombre_legal' AND @sort_dir = 'DESC' THEN nombre_legal END DESC,
        CASE WHEN @sort_by = 'identificacion_fiscal' AND @sort_dir = 'ASC' THEN identificacion_fiscal END ASC,
        CASE WHEN @sort_by = 'identificacion_fiscal' AND @sort_dir = 'DESC' THEN identificacion_fiscal END DESC,
        CASE WHEN @sort_by = 'estado' AND @sort_dir = 'ASC' THEN estado END ASC,
        CASE WHEN @sort_by = 'estado' AND @sort_dir = 'DESC' THEN estado END DESC,
        id_empresa ASC
    OFFSET @offset ROWS FETCH NEXT @size ROWS ONLY;
END
GO

CREATE OR ALTER PROCEDURE organizacion.usp_empresa_listar_paginado
    @page int = 1,
    @size int = 25,
    @sort_by nvarchar(64) = N'id_empresa',
    @sort_dir varchar(4) = 'ASC',
    @filter nvarchar(200) = NULL,
    @filter_field nvarchar(64) = NULL,
    @id_tenant bigint = NULL,
    @total_registros int OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @page < 1 SET @page = 1;
    IF @size < 5 SET @size = 25;
    IF @size > 500 SET @size = 500;

    SET @sort_by = LOWER(LTRIM(RTRIM(ISNULL(@sort_by, 'id_empresa'))));
    SET @sort_dir = UPPER(LTRIM(RTRIM(ISNULL(@sort_dir, 'ASC'))));
    IF @sort_dir NOT IN ('ASC', 'DESC') SET @sort_dir = 'ASC';

    DECLARE @offset int = (@page - 1) * @size;

    CREATE TABLE #base (
        id_empresa bigint NOT NULL,
        id_tenant bigint NOT NULL,
        codigo nvarchar(50) NOT NULL,
        nombre nvarchar(250) NOT NULL,
        nombre_legal nvarchar(300) NULL,
        identificacion_fiscal nvarchar(50) NULL,
        estado varchar(20) NOT NULL,
        activo bit NOT NULL
    );

    INSERT INTO #base (id_empresa, id_tenant, codigo, nombre, nombre_legal, identificacion_fiscal, estado, activo)
    SELECT
        e.id_empresa,
        e.id_tenant,
        e.codigo,
        e.nombre,
        e.nombre_legal,
        e.identificacion_fiscal,
        COALESCE(ee.codigo, CAST(e.id_estado_empresa AS varchar(20))) AS estado,
        e.activo
    FROM organizacion.empresa AS e
    LEFT JOIN catalogo.estado_empresa AS ee
        ON ee.id_estado_empresa = e.id_estado_empresa
    WHERE
        (@id_tenant IS NULL OR e.id_tenant = @id_tenant)
        AND (
            @filter IS NULL
            OR LTRIM(RTRIM(@filter)) = ''
            OR ((@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'codigo') AND e.codigo LIKE '%' + @filter + '%')
            OR ((@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'nombre') AND e.nombre LIKE '%' + @filter + '%')
            OR ((@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'nombre_legal') AND e.nombre_legal LIKE '%' + @filter + '%')
            OR ((@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'identificacion_fiscal') AND e.identificacion_fiscal LIKE '%' + @filter + '%')
            OR ((@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'estado') AND COALESCE(ee.codigo, CAST(e.id_estado_empresa AS varchar(20))) LIKE '%' + @filter + '%')
        );

    SELECT @total_registros = COUNT(1) FROM #base;

    SELECT
        id_empresa,
        id_tenant,
        codigo,
        nombre,
        nombre_legal,
        identificacion_fiscal,
        estado,
        activo
    FROM #base
    ORDER BY
        CASE WHEN @sort_by = 'id_empresa' AND @sort_dir = 'ASC' THEN id_empresa END ASC,
        CASE WHEN @sort_by = 'id_empresa' AND @sort_dir = 'DESC' THEN id_empresa END DESC,
        CASE WHEN @sort_by = 'codigo' AND @sort_dir = 'ASC' THEN codigo END ASC,
        CASE WHEN @sort_by = 'codigo' AND @sort_dir = 'DESC' THEN codigo END DESC,
        CASE WHEN @sort_by = 'nombre' AND @sort_dir = 'ASC' THEN nombre END ASC,
        CASE WHEN @sort_by = 'nombre' AND @sort_dir = 'DESC' THEN nombre END DESC,
        CASE WHEN @sort_by = 'nombre_legal' AND @sort_dir = 'ASC' THEN nombre_legal END ASC,
        CASE WHEN @sort_by = 'nombre_legal' AND @sort_dir = 'DESC' THEN nombre_legal END DESC,
        CASE WHEN @sort_by = 'identificacion_fiscal' AND @sort_dir = 'ASC' THEN identificacion_fiscal END ASC,
        CASE WHEN @sort_by = 'identificacion_fiscal' AND @sort_dir = 'DESC' THEN identificacion_fiscal END DESC,
        CASE WHEN @sort_by = 'estado' AND @sort_dir = 'ASC' THEN estado END ASC,
        CASE WHEN @sort_by = 'estado' AND @sort_dir = 'DESC' THEN estado END DESC,
        id_empresa ASC
    OFFSET @offset ROWS FETCH NEXT @size ROWS ONLY;
END
GO
