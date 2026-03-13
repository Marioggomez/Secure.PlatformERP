/*
    Migracion: 20260313174520__hardening_scope_crud_and_paginado_claims
    Autor: Mario Gomez
    Fecha UTC: 2026-03-13 17:45:20
*/
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO
/* Endurecer listados paginados para usar scope de SESSION_CONTEXT */
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
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SET @id_tenant = @ctx_id_tenant;

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
        e.id_tenant = @id_tenant
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
CREATE OR ALTER PROCEDURE seguridad.usp_usuario_listar_paginado
    @page int = 1,
    @size int = 25,
    @sort_by nvarchar(64) = N'id_usuario',
    @sort_dir varchar(4) = 'ASC',
    @filter nvarchar(200) = NULL,
    @filter_field nvarchar(64) = NULL,
    @id_tenant bigint = NULL,
    @total_registros int OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    SET @id_tenant = @ctx_id_tenant;

    IF @page < 1 SET @page = 1;
    IF @size < 5 SET @size = 25;
    IF @size > 500 SET @size = 500;

    SET @sort_by = LOWER(LTRIM(RTRIM(ISNULL(@sort_by, 'id_usuario'))));
    SET @sort_dir = UPPER(LTRIM(RTRIM(ISNULL(@sort_dir, 'ASC'))));
    IF @sort_dir NOT IN ('ASC', 'DESC') SET @sort_dir = 'ASC';

    DECLARE @offset int = (@page - 1) * @size;

    CREATE TABLE #base (
        id_usuario bigint NOT NULL,
        id_tenant bigint NULL,
        login_principal nvarchar(120) NOT NULL,
        nombre_mostrar nvarchar(250) NOT NULL,
        correo_electronico nvarchar(250) NULL,
        estado varchar(20) NOT NULL,
        ultimo_acceso_utc datetime2 NULL,
        activo bit NOT NULL
    );

    INSERT INTO #base (id_usuario, id_tenant, login_principal, nombre_mostrar, correo_electronico, estado, ultimo_acceso_utc, activo)
    SELECT
        u.id_usuario,
        ut.id_tenant,
        u.login_principal,
        u.nombre_mostrar,
        u.correo_electronico,
        COALESCE(eu.codigo, CAST(u.id_estado_usuario AS varchar(20))) AS estado,
        u.ultimo_acceso_utc,
        u.activo
    FROM seguridad.usuario AS u
    LEFT JOIN seguridad.usuario_tenant AS ut
        ON ut.id_usuario = u.id_usuario
       AND ut.activo = 1
    LEFT JOIN catalogo.estado_usuario AS eu
        ON eu.id_estado_usuario = u.id_estado_usuario
    WHERE
        ut.id_tenant = @id_tenant
        AND (
            @filter IS NULL
            OR LTRIM(RTRIM(@filter)) = ''
            OR ((@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'login_principal') AND u.login_principal LIKE '%' + @filter + '%')
            OR ((@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'nombre_mostrar') AND u.nombre_mostrar LIKE '%' + @filter + '%')
            OR ((@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'correo_electronico') AND u.correo_electronico LIKE '%' + @filter + '%')
            OR ((@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'estado') AND COALESCE(eu.codigo, CAST(u.id_estado_usuario AS varchar(20))) LIKE '%' + @filter + '%')
        );

    SELECT @total_registros = COUNT(1) FROM #base;

    SELECT
        id_usuario,
        id_tenant,
        login_principal,
        nombre_mostrar,
        correo_electronico,
        estado,
        ultimo_acceso_utc,
        activo
    FROM #base
    ORDER BY
        CASE WHEN @sort_by = 'id_usuario' AND @sort_dir = 'ASC' THEN id_usuario END ASC,
        CASE WHEN @sort_by = 'id_usuario' AND @sort_dir = 'DESC' THEN id_usuario END DESC,
        CASE WHEN @sort_by = 'login_principal' AND @sort_dir = 'ASC' THEN login_principal END ASC,
        CASE WHEN @sort_by = 'login_principal' AND @sort_dir = 'DESC' THEN login_principal END DESC,
        CASE WHEN @sort_by = 'nombre_mostrar' AND @sort_dir = 'ASC' THEN nombre_mostrar END ASC,
        CASE WHEN @sort_by = 'nombre_mostrar' AND @sort_dir = 'DESC' THEN nombre_mostrar END DESC,
        CASE WHEN @sort_by = 'correo_electronico' AND @sort_dir = 'ASC' THEN correo_electronico END ASC,
        CASE WHEN @sort_by = 'correo_electronico' AND @sort_dir = 'DESC' THEN correo_electronico END DESC,
        CASE WHEN @sort_by = 'estado' AND @sort_dir = 'ASC' THEN estado END ASC,
        CASE WHEN @sort_by = 'estado' AND @sort_dir = 'DESC' THEN estado END DESC,
        CASE WHEN @sort_by = 'ultimo_acceso_utc' AND @sort_dir = 'ASC' THEN ultimo_acceso_utc END ASC,
        CASE WHEN @sort_by = 'ultimo_acceso_utc' AND @sort_dir = 'DESC' THEN ultimo_acceso_utc END DESC,
        id_usuario ASC
    OFFSET @offset ROWS FETCH NEXT @size ROWS ONLY;
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
    DECLARE @ctx_id_tenant BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_tenant') AS BIGINT);
    IF @ctx_id_tenant IS NULL THROW 51050, N'Scope id_tenant no disponible en SESSION_CONTEXT.', 1;
    DECLARE @ctx_id_empresa BIGINT = TRY_CAST(SESSION_CONTEXT(N'id_empresa') AS BIGINT);
    IF @ctx_id_empresa IS NULL THROW 51051, N'Scope id_empresa no disponible en SESSION_CONTEXT.', 1;

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
    INNER JOIN tercero.tercero_empresa AS te
        ON te.id_tercero = t.id_tercero
       AND te.id_empresa = @ctx_id_empresa
       AND te.activo = 1
    INNER JOIN organizacion.empresa AS e
        ON e.id_empresa = te.id_empresa
       AND e.id_tenant = @ctx_id_tenant
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
