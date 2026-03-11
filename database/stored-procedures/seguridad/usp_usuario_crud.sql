CREATE OR ALTER PROCEDURE seguridad.usp_usuario_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario], [codigo], [login_principal], [login_normalizado], [nombre], [apellido], [nombre_mostrar], [correo_electronico], [correo_normalizado], [telefono_movil], [idioma], [zona_horaria], [id_estado_usuario], [bloqueado_hasta_utc], [mfa_habilitado], [requiere_cambio_clave], [ultimo_acceso_utc], [activo], [creado_por], [creado_utc], [actualizado_por], [actualizado_utc], [version_fila]
    FROM seguridad.usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_obtener
    @id_usuario bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_usuario], [codigo], [login_principal], [login_normalizado], [nombre], [apellido], [nombre_mostrar], [correo_electronico], [correo_normalizado], [telefono_movil], [idioma], [zona_horaria], [id_estado_usuario], [bloqueado_hasta_utc], [mfa_habilitado], [requiere_cambio_clave], [ultimo_acceso_utc], [activo], [creado_por], [creado_utc], [actualizado_por], [actualizado_utc], [version_fila]
    FROM seguridad.usuario
    WHERE [id_usuario] = @id_usuario;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_crear
    @codigo nvarchar(60),
    @login_principal nvarchar(120),
    @login_normalizado nvarchar(120),
    @nombre nvarchar(120),
    @apellido nvarchar(120),
    @nombre_mostrar nvarchar(250),
    @correo_electronico nvarchar(250),
    @correo_normalizado nvarchar(250),
    @telefono_movil nvarchar(50),
    @idioma nvarchar(10),
    @zona_horaria nvarchar(80),
    @id_estado_usuario smallint,
    @bloqueado_hasta_utc datetime2,
    @mfa_habilitado bit,
    @requiere_cambio_clave bit,
    @ultimo_acceso_utc datetime2,
    @activo bit,
    @creado_por bigint,
    @creado_utc datetime2,
    @actualizado_por bigint,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.usuario ([codigo], [login_principal], [login_normalizado], [nombre], [apellido], [nombre_mostrar], [correo_electronico], [correo_normalizado], [telefono_movil], [idioma], [zona_horaria], [id_estado_usuario], [bloqueado_hasta_utc], [mfa_habilitado], [requiere_cambio_clave], [ultimo_acceso_utc], [activo], [creado_por], [creado_utc], [actualizado_por], [actualizado_utc])
    VALUES (@codigo, @login_principal, @login_normalizado, @nombre, @apellido, @nombre_mostrar, @correo_electronico, @correo_normalizado, @telefono_movil, @idioma, @zona_horaria, @id_estado_usuario, @bloqueado_hasta_utc, @mfa_habilitado, @requiere_cambio_clave, @ultimo_acceso_utc, @activo, @creado_por, @creado_utc, @actualizado_por, @actualizado_utc);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_actualizar
    @id_usuario bigint,
    @codigo nvarchar(60),
    @login_principal nvarchar(120),
    @login_normalizado nvarchar(120),
    @nombre nvarchar(120),
    @apellido nvarchar(120),
    @nombre_mostrar nvarchar(250),
    @correo_electronico nvarchar(250),
    @correo_normalizado nvarchar(250),
    @telefono_movil nvarchar(50),
    @idioma nvarchar(10),
    @zona_horaria nvarchar(80),
    @id_estado_usuario smallint,
    @bloqueado_hasta_utc datetime2,
    @mfa_habilitado bit,
    @requiere_cambio_clave bit,
    @ultimo_acceso_utc datetime2,
    @activo bit,
    @creado_por bigint,
    @creado_utc datetime2,
    @actualizado_por bigint,
    @actualizado_utc datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.usuario
    SET [codigo] = @codigo,
        [login_principal] = @login_principal,
        [login_normalizado] = @login_normalizado,
        [nombre] = @nombre,
        [apellido] = @apellido,
        [nombre_mostrar] = @nombre_mostrar,
        [correo_electronico] = @correo_electronico,
        [correo_normalizado] = @correo_normalizado,
        [telefono_movil] = @telefono_movil,
        [idioma] = @idioma,
        [zona_horaria] = @zona_horaria,
        [id_estado_usuario] = @id_estado_usuario,
        [bloqueado_hasta_utc] = @bloqueado_hasta_utc,
        [mfa_habilitado] = @mfa_habilitado,
        [requiere_cambio_clave] = @requiere_cambio_clave,
        [ultimo_acceso_utc] = @ultimo_acceso_utc,
        [activo] = @activo,
        [creado_por] = @creado_por,
        [creado_utc] = @creado_utc,
        [actualizado_por] = @actualizado_por,
        [actualizado_utc] = @actualizado_utc
    WHERE [id_usuario] = @id_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_usuario_desactivar
    @id_usuario bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.usuario
    SET [activo] = 0,
        [actualizado_utc] = SYSUTCDATETIME()
    WHERE [id_usuario] = @id_usuario;
    SELECT @@ROWCOUNT AS filas_afectadas;
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

    IF @page < 1 SET @page = 1;
    IF @size < 5 SET @size = 25;
    IF @size > 500 SET @size = 500;

    SET @sort_by = LOWER(LTRIM(RTRIM(ISNULL(@sort_by, 'id_usuario'))));
    SET @sort_dir = UPPER(LTRIM(RTRIM(ISNULL(@sort_dir, 'ASC'))));
    IF @sort_dir NOT IN ('ASC', 'DESC') SET @sort_dir = 'ASC';

    DECLARE @offset int = (@page - 1) * @size;

    ;WITH base AS (
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
            (@id_tenant IS NULL OR ut.id_tenant = @id_tenant)
            AND (
                @filter IS NULL
                OR LTRIM(RTRIM(@filter)) = ''
                OR (
                    (@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'login_principal')
                    AND u.login_principal LIKE '%' + @filter + '%'
                )
                OR (
                    (@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'nombre_mostrar')
                    AND u.nombre_mostrar LIKE '%' + @filter + '%'
                )
                OR (
                    (@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'correo_electronico')
                    AND u.correo_electronico LIKE '%' + @filter + '%'
                )
                OR (
                    (@filter_field IS NULL OR @filter_field = '' OR @filter_field = 'estado')
                    AND COALESCE(eu.codigo, CAST(u.id_estado_usuario AS varchar(20))) LIKE '%' + @filter + '%'
                )
            )
    )
    SELECT @total_registros = COUNT(1)
    FROM base;

    ;WITH page_data AS (
        SELECT
            id_usuario,
            id_tenant,
            login_principal,
            nombre_mostrar,
            correo_electronico,
            estado,
            ultimo_acceso_utc,
            activo
        FROM base
    )
    SELECT
        id_usuario,
        id_tenant,
        login_principal,
        nombre_mostrar,
        correo_electronico,
        estado,
        ultimo_acceso_utc,
        activo
    FROM page_data
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
        (@id_tenant IS NULL OR ut.id_tenant = @id_tenant)
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
