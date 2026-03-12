CREATE OR ALTER PROCEDURE tercero.usp_tercero_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tercero], [codigo], [id_tipo_persona], [nombre], [segundo_nombre], [apellido], [segundo_apellido], [razon_social], [nombre_comercial], [fecha_nacimiento], [fecha_constitucion], [activo], [creado_por], [creado_utc], [version_fila]
    FROM tercero.tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tercero_obtener
    @id_tercero bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tercero], [codigo], [id_tipo_persona], [nombre], [segundo_nombre], [apellido], [segundo_apellido], [razon_social], [nombre_comercial], [fecha_nacimiento], [fecha_constitucion], [activo], [creado_por], [creado_utc], [version_fila]
    FROM tercero.tercero
    WHERE [id_tercero] = @id_tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tercero_crear
    @codigo nvarchar(50),
    @id_tipo_persona int,
    @nombre nvarchar(200),
    @segundo_nombre nvarchar(200),
    @apellido nvarchar(200),
    @segundo_apellido nvarchar(200),
    @razon_social nvarchar(400),
    @nombre_comercial nvarchar(400),
    @fecha_nacimiento date,
    @fecha_constitucion date,
    @activo bit,
    @creado_por int,
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tercero.tercero ([codigo], [id_tipo_persona], [nombre], [segundo_nombre], [apellido], [segundo_apellido], [razon_social], [nombre_comercial], [fecha_nacimiento], [fecha_constitucion], [activo], [creado_por], [creado_utc])
    VALUES (@codigo, @id_tipo_persona, @nombre, @segundo_nombre, @apellido, @segundo_apellido, @razon_social, @nombre_comercial, @fecha_nacimiento, @fecha_constitucion, @activo, @creado_por, @creado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tercero_actualizar
    @id_tercero bigint,
    @codigo nvarchar(50),
    @id_tipo_persona int,
    @nombre nvarchar(200),
    @segundo_nombre nvarchar(200),
    @apellido nvarchar(200),
    @segundo_apellido nvarchar(200),
    @razon_social nvarchar(400),
    @nombre_comercial nvarchar(400),
    @fecha_nacimiento date,
    @fecha_constitucion date,
    @activo bit,
    @creado_por int,
    @creado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tercero.tercero
    SET [codigo] = @codigo,
        [id_tipo_persona] = @id_tipo_persona,
        [nombre] = @nombre,
        [segundo_nombre] = @segundo_nombre,
        [apellido] = @apellido,
        [segundo_apellido] = @segundo_apellido,
        [razon_social] = @razon_social,
        [nombre_comercial] = @nombre_comercial,
        [fecha_nacimiento] = @fecha_nacimiento,
        [fecha_constitucion] = @fecha_constitucion,
        [activo] = @activo,
        [creado_por] = @creado_por,
        [creado_utc] = @creado_utc
    WHERE [id_tercero] = @id_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tercero_desactivar
    @id_tercero bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tercero.tercero
    SET [activo] = 0
    WHERE [id_tercero] = @id_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tercero_listar_paginado
    @page int = 1,
    @size int = 25,
    @sort_by nvarchar(64) = N'id_tercero',
    @sort_dir varchar(4) = 'ASC',
    @filter nvarchar(200) = NULL,
    @filter_field nvarchar(64) = NULL,
    @total_registros int OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @page < 1 SET @page = 1;
    IF @size < 5 SET @size = 25;
    IF @size > 500 SET @size = 500;

    SET @sort_by = LOWER(LTRIM(RTRIM(ISNULL(@sort_by, 'id_tercero'))));
    SET @sort_dir = UPPER(LTRIM(RTRIM(ISNULL(@sort_dir, 'ASC'))));
    IF @sort_dir NOT IN ('ASC', 'DESC') SET @sort_dir = 'ASC';

    DECLARE @offset int = (@page - 1) * @size;

    CREATE TABLE #base
    (
        id_tercero bigint NOT NULL,
        codigo nvarchar(50) NOT NULL,
        id_tipo_persona int NOT NULL,
        tipo_persona nvarchar(100) NULL,
        nombre_principal nvarchar(400) NULL,
        activo bit NOT NULL,
        creado_utc datetime2 NOT NULL
    );

    INSERT INTO #base
    (
        id_tercero,
        codigo,
        id_tipo_persona,
        tipo_persona,
        nombre_principal,
        activo,
        creado_utc
    )
    SELECT
        t.id_tercero,
        t.codigo,
        t.id_tipo_persona,
        tp.nombre AS tipo_persona,
        CASE
            WHEN t.id_tipo_persona = 1 THEN LTRIM(RTRIM(CONCAT(ISNULL(t.nombre, ''), ' ', ISNULL(t.apellido, ''))))
            WHEN t.id_tipo_persona = 2 THEN ISNULL(t.razon_social, '')
            ELSE COALESCE(t.nombre_comercial, t.razon_social, LTRIM(RTRIM(CONCAT(ISNULL(t.nombre, ''), ' ', ISNULL(t.apellido, '')))), t.codigo)
        END AS nombre_principal,
        t.activo,
        t.creado_utc
    FROM tercero.tercero AS t
    LEFT JOIN tercero.tipo_persona AS tp
        ON tp.id_tipo_persona = t.id_tipo_persona
    WHERE
        (
            @filter IS NULL
            OR LTRIM(RTRIM(@filter)) = ''
            OR ((@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'codigo') AND t.codigo LIKE '%' + @filter + '%')
            OR ((@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'tipo_persona') AND tp.nombre LIKE '%' + @filter + '%')
            OR ((@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'nombre_principal') AND (
                ISNULL(t.nombre, '') LIKE '%' + @filter + '%'
                OR ISNULL(t.apellido, '') LIKE '%' + @filter + '%'
                OR ISNULL(t.razon_social, '') LIKE '%' + @filter + '%'
                OR ISNULL(t.nombre_comercial, '') LIKE '%' + @filter + '%'
            ))
        );

    SELECT @total_registros = COUNT(1) FROM #base;

    SELECT
        id_tercero,
        codigo,
        id_tipo_persona,
        tipo_persona,
        nombre_principal,
        activo,
        creado_utc
    FROM #base
    ORDER BY
        CASE WHEN @sort_by = 'id_tercero' AND @sort_dir = 'ASC' THEN id_tercero END ASC,
        CASE WHEN @sort_by = 'id_tercero' AND @sort_dir = 'DESC' THEN id_tercero END DESC,
        CASE WHEN @sort_by = 'codigo' AND @sort_dir = 'ASC' THEN codigo END ASC,
        CASE WHEN @sort_by = 'codigo' AND @sort_dir = 'DESC' THEN codigo END DESC,
        CASE WHEN @sort_by = 'id_tipo_persona' AND @sort_dir = 'ASC' THEN id_tipo_persona END ASC,
        CASE WHEN @sort_by = 'id_tipo_persona' AND @sort_dir = 'DESC' THEN id_tipo_persona END DESC,
        CASE WHEN @sort_by = 'tipo_persona' AND @sort_dir = 'ASC' THEN tipo_persona END ASC,
        CASE WHEN @sort_by = 'tipo_persona' AND @sort_dir = 'DESC' THEN tipo_persona END DESC,
        CASE WHEN @sort_by = 'nombre_principal' AND @sort_dir = 'ASC' THEN nombre_principal END ASC,
        CASE WHEN @sort_by = 'nombre_principal' AND @sort_dir = 'DESC' THEN nombre_principal END DESC,
        CASE WHEN @sort_by = 'activo' AND @sort_dir = 'ASC' THEN activo END ASC,
        CASE WHEN @sort_by = 'activo' AND @sort_dir = 'DESC' THEN activo END DESC,
        CASE WHEN @sort_by = 'creado_utc' AND @sort_dir = 'ASC' THEN creado_utc END ASC,
        CASE WHEN @sort_by = 'creado_utc' AND @sort_dir = 'DESC' THEN creado_utc END DESC,
        id_tercero ASC
    OFFSET @offset ROWS FETCH NEXT @size ROWS ONLY;
END
GO
