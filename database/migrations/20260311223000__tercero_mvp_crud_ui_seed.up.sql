/*
    Migracion: 20260311223000__tercero_mvp_crud_ui_seed
    Autor: Mario Gomez
    Fecha UTC: 2026-03-12 14:01:30
*/

CREATE OR ALTER PROCEDURE tercero.usp_contacto_tercero_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_contacto_tercero], [id_tercero], [id_tipo_contacto], [valor], [principal]
    FROM tercero.contacto_tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_contacto_tercero_obtener
    @id_contacto_tercero bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_contacto_tercero], [id_tercero], [id_tipo_contacto], [valor], [principal]
    FROM tercero.contacto_tercero
    WHERE [id_contacto_tercero] = @id_contacto_tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_contacto_tercero_crear
    @id_tercero bigint,
    @id_tipo_contacto int,
    @valor nvarchar(300),
    @principal bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tercero.contacto_tercero ([id_tercero], [id_tipo_contacto], [valor], [principal])
    VALUES (@id_tercero, @id_tipo_contacto, @valor, @principal);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_contacto_tercero_actualizar
    @id_contacto_tercero bigint,
    @id_tercero bigint,
    @id_tipo_contacto int,
    @valor nvarchar(300),
    @principal bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tercero.contacto_tercero
    SET [id_tercero] = @id_tercero,
        [id_tipo_contacto] = @id_tipo_contacto,
        [valor] = @valor,
        [principal] = @principal
    WHERE [id_contacto_tercero] = @id_contacto_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_contacto_tercero_desactivar
    @id_contacto_tercero bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tercero.contacto_tercero
    WHERE [id_contacto_tercero] = @id_contacto_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO


CREATE OR ALTER PROCEDURE tercero.usp_cuenta_bancaria_tercero_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_cuenta_bancaria_tercero], [id_tercero], [id_banco], [numero_cuenta], [id_moneda], [principal]
    FROM tercero.cuenta_bancaria_tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_cuenta_bancaria_tercero_obtener
    @id_cuenta_bancaria_tercero bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_cuenta_bancaria_tercero], [id_tercero], [id_banco], [numero_cuenta], [id_moneda], [principal]
    FROM tercero.cuenta_bancaria_tercero
    WHERE [id_cuenta_bancaria_tercero] = @id_cuenta_bancaria_tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_cuenta_bancaria_tercero_crear
    @id_tercero bigint,
    @id_banco int,
    @numero_cuenta nvarchar(100),
    @id_moneda int,
    @principal bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tercero.cuenta_bancaria_tercero ([id_tercero], [id_banco], [numero_cuenta], [id_moneda], [principal])
    VALUES (@id_tercero, @id_banco, @numero_cuenta, @id_moneda, @principal);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_cuenta_bancaria_tercero_actualizar
    @id_cuenta_bancaria_tercero bigint,
    @id_tercero bigint,
    @id_banco int,
    @numero_cuenta nvarchar(100),
    @id_moneda int,
    @principal bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tercero.cuenta_bancaria_tercero
    SET [id_tercero] = @id_tercero,
        [id_banco] = @id_banco,
        [numero_cuenta] = @numero_cuenta,
        [id_moneda] = @id_moneda,
        [principal] = @principal
    WHERE [id_cuenta_bancaria_tercero] = @id_cuenta_bancaria_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_cuenta_bancaria_tercero_desactivar
    @id_cuenta_bancaria_tercero bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tercero.cuenta_bancaria_tercero
    WHERE [id_cuenta_bancaria_tercero] = @id_cuenta_bancaria_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO


CREATE OR ALTER PROCEDURE tercero.usp_direccion_tercero_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_direccion_tercero], [id_tercero], [id_tipo_direccion], [direccion_linea1], [direccion_linea2], [id_pais], [id_estado], [id_ciudad], [codigo_postal], [principal]
    FROM tercero.direccion_tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_direccion_tercero_obtener
    @id_direccion_tercero bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_direccion_tercero], [id_tercero], [id_tipo_direccion], [direccion_linea1], [direccion_linea2], [id_pais], [id_estado], [id_ciudad], [codigo_postal], [principal]
    FROM tercero.direccion_tercero
    WHERE [id_direccion_tercero] = @id_direccion_tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_direccion_tercero_crear
    @id_tercero bigint,
    @id_tipo_direccion int,
    @direccion_linea1 nvarchar(400),
    @direccion_linea2 nvarchar(400),
    @id_pais int,
    @id_estado int,
    @id_ciudad int,
    @codigo_postal nvarchar(50),
    @principal bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tercero.direccion_tercero ([id_tercero], [id_tipo_direccion], [direccion_linea1], [direccion_linea2], [id_pais], [id_estado], [id_ciudad], [codigo_postal], [principal])
    VALUES (@id_tercero, @id_tipo_direccion, @direccion_linea1, @direccion_linea2, @id_pais, @id_estado, @id_ciudad, @codigo_postal, @principal);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_direccion_tercero_actualizar
    @id_direccion_tercero bigint,
    @id_tercero bigint,
    @id_tipo_direccion int,
    @direccion_linea1 nvarchar(400),
    @direccion_linea2 nvarchar(400),
    @id_pais int,
    @id_estado int,
    @id_ciudad int,
    @codigo_postal nvarchar(50),
    @principal bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tercero.direccion_tercero
    SET [id_tercero] = @id_tercero,
        [id_tipo_direccion] = @id_tipo_direccion,
        [direccion_linea1] = @direccion_linea1,
        [direccion_linea2] = @direccion_linea2,
        [id_pais] = @id_pais,
        [id_estado] = @id_estado,
        [id_ciudad] = @id_ciudad,
        [codigo_postal] = @codigo_postal,
        [principal] = @principal
    WHERE [id_direccion_tercero] = @id_direccion_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_direccion_tercero_desactivar
    @id_direccion_tercero bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tercero.direccion_tercero
    WHERE [id_direccion_tercero] = @id_direccion_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO


CREATE OR ALTER PROCEDURE tercero.usp_identificacion_tercero_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_identificacion_tercero], [id_tercero], [id_tipo_identificacion], [numero_identificacion], [fecha_emision], [fecha_vencimiento], [principal]
    FROM tercero.identificacion_tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_identificacion_tercero_obtener
    @id_identificacion_tercero bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_identificacion_tercero], [id_tercero], [id_tipo_identificacion], [numero_identificacion], [fecha_emision], [fecha_vencimiento], [principal]
    FROM tercero.identificacion_tercero
    WHERE [id_identificacion_tercero] = @id_identificacion_tercero;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_identificacion_tercero_crear
    @id_tercero bigint,
    @id_tipo_identificacion int,
    @numero_identificacion nvarchar(100),
    @fecha_emision date,
    @fecha_vencimiento date,
    @principal bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tercero.identificacion_tercero ([id_tercero], [id_tipo_identificacion], [numero_identificacion], [fecha_emision], [fecha_vencimiento], [principal])
    VALUES (@id_tercero, @id_tipo_identificacion, @numero_identificacion, @fecha_emision, @fecha_vencimiento, @principal);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_identificacion_tercero_actualizar
    @id_identificacion_tercero bigint,
    @id_tercero bigint,
    @id_tipo_identificacion int,
    @numero_identificacion nvarchar(100),
    @fecha_emision date,
    @fecha_vencimiento date,
    @principal bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tercero.identificacion_tercero
    SET [id_tercero] = @id_tercero,
        [id_tipo_identificacion] = @id_tipo_identificacion,
        [numero_identificacion] = @numero_identificacion,
        [fecha_emision] = @fecha_emision,
        [fecha_vencimiento] = @fecha_vencimiento,
        [principal] = @principal
    WHERE [id_identificacion_tercero] = @id_identificacion_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_identificacion_tercero_desactivar
    @id_identificacion_tercero bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tercero.identificacion_tercero
    WHERE [id_identificacion_tercero] = @id_identificacion_tercero;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO


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


CREATE OR ALTER PROCEDURE tercero.usp_tercero_rol_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tercero_rol], [id_tercero], [id_rol_tercero], [id_empresa], [activo]
    FROM tercero.tercero_rol;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tercero_rol_obtener
    @id_tercero_rol bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tercero_rol], [id_tercero], [id_rol_tercero], [id_empresa], [activo]
    FROM tercero.tercero_rol
    WHERE [id_tercero_rol] = @id_tercero_rol;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tercero_rol_crear
    @id_tercero bigint,
    @id_rol_tercero int,
    @id_empresa bigint,
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tercero.tercero_rol ([id_tercero], [id_rol_tercero], [id_empresa], [activo])
    VALUES (@id_tercero, @id_rol_tercero, @id_empresa, @activo);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tercero_rol_actualizar
    @id_tercero_rol bigint,
    @id_tercero bigint,
    @id_rol_tercero int,
    @id_empresa bigint,
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tercero.tercero_rol
    SET [id_tercero] = @id_tercero,
        [id_rol_tercero] = @id_rol_tercero,
        [id_empresa] = @id_empresa,
        [activo] = @activo
    WHERE [id_tercero_rol] = @id_tercero_rol;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tercero_rol_desactivar
    @id_tercero_rol bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tercero.tercero_rol
    SET [activo] = 0
    WHERE [id_tercero_rol] = @id_tercero_rol;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO


CREATE OR ALTER PROCEDURE tercero.usp_tipo_persona_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tipo_persona], [codigo], [nombre]
    FROM tercero.tipo_persona;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tipo_persona_obtener
    @id_tipo_persona int
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tipo_persona], [codigo], [nombre]
    FROM tercero.tipo_persona
    WHERE [id_tipo_persona] = @id_tipo_persona;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tipo_persona_crear
    @codigo nvarchar(50),
    @nombre nvarchar(100)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO tercero.tipo_persona ([codigo], [nombre])
    VALUES (@codigo, @nombre);
    SELECT CAST(SCOPE_IDENTITY() AS int) AS id;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tipo_persona_actualizar
    @id_tipo_persona int,
    @codigo nvarchar(50),
    @nombre nvarchar(100)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE tercero.tipo_persona
    SET [codigo] = @codigo,
        [nombre] = @nombre
    WHERE [id_tipo_persona] = @id_tipo_persona;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE tercero.usp_tipo_persona_desactivar
    @id_tipo_persona int,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM tercero.tipo_persona
    WHERE [id_tipo_persona] = @id_tipo_persona;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO


/*
    Seed inicial para modulo tercero (alcance MVP).
    Autor: Mario Gomez
*/
SET NOCOUNT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @now DATETIME2(3) = SYSUTCDATETIME();
    DECLARE @idTipoForm SMALLINT = (
        SELECT TOP (1) id_tipo_recurso_ui
        FROM catalogo.tipo_recurso_ui
        WHERE codigo = 'FORM'
    );

    IF @idTipoForm IS NULL
    BEGIN
        THROW 50001, 'No existe catalogo.tipo_recurso_ui codigo=FORM.', 1;
    END;

    DECLARE @idEmpresaSeed BIGINT = (
        SELECT TOP (1) id_empresa
        FROM organizacion.empresa
        WHERE activo = 1
        ORDER BY id_empresa
    );

    DECLARE @idRolTerceroDefault INT = (
        SELECT TOP (1) id_rol_tercero
        FROM tercero.rol_tercero
        WHERE activo = 1
        ORDER BY id_rol_tercero
    );

    ------------------------------------------------------------
    -- 1) Catalogo tercero.tipo_persona
    ------------------------------------------------------------
    MERGE tercero.tipo_persona AS target
    USING (VALUES
        (N'NATURAL', N'Persona Natural'),
        (N'JURIDICA', N'Persona Juridica')
    ) AS source (codigo, nombre)
    ON target.codigo = source.codigo
    WHEN MATCHED THEN
        UPDATE SET nombre = source.nombre
    WHEN NOT MATCHED THEN
        INSERT (codigo, nombre)
        VALUES (source.codigo, source.nombre);

    DECLARE @idTipoPersonaNatural INT = (
        SELECT TOP (1) id_tipo_persona
        FROM tercero.tipo_persona
        WHERE codigo = N'NATURAL'
    );

    DECLARE @idTipoPersonaJuridica INT = (
        SELECT TOP (1) id_tipo_persona
        FROM tercero.tipo_persona
        WHERE codigo = N'JURIDICA'
    );

    ------------------------------------------------------------
    -- 2) Terceros base
    ------------------------------------------------------------
    IF EXISTS (SELECT 1 FROM tercero.tercero WHERE codigo = N'TER-001')
    BEGIN
        UPDATE tercero.tercero
        SET
            id_tipo_persona = @idTipoPersonaNatural,
            nombre = N'Carlos',
            apellido = N'Mejia',
            nombre_comercial = N'Carlos Mejia',
            activo = 1,
            creado_por = 1
        WHERE codigo = N'TER-001';
    END
    ELSE
    BEGIN
        INSERT INTO tercero.tercero
        (
            codigo,
            id_tipo_persona,
            nombre,
            segundo_nombre,
            apellido,
            segundo_apellido,
            razon_social,
            nombre_comercial,
            fecha_nacimiento,
            fecha_constitucion,
            activo,
            creado_por,
            creado_utc
        )
        VALUES
        (
            N'TER-001',
            @idTipoPersonaNatural,
            N'Carlos',
            N'Enrique',
            N'Mejia',
            N'Lopez',
            NULL,
            N'Carlos Mejia',
            '1989-02-14',
            NULL,
            1,
            1,
            @now
        );
    END;

    IF EXISTS (SELECT 1 FROM tercero.tercero WHERE codigo = N'TER-002')
    BEGIN
        UPDATE tercero.tercero
        SET
            id_tipo_persona = @idTipoPersonaJuridica,
            razon_social = N'Comercializadora Seed, S.A.',
            nombre_comercial = N'Comercializadora Seed',
            activo = 1,
            creado_por = 1
        WHERE codigo = N'TER-002';
    END
    ELSE
    BEGIN
        INSERT INTO tercero.tercero
        (
            codigo,
            id_tipo_persona,
            nombre,
            segundo_nombre,
            apellido,
            segundo_apellido,
            razon_social,
            nombre_comercial,
            fecha_nacimiento,
            fecha_constitucion,
            activo,
            creado_por,
            creado_utc
        )
        VALUES
        (
            N'TER-002',
            @idTipoPersonaJuridica,
            NULL,
            NULL,
            NULL,
            NULL,
            N'Comercializadora Seed, S.A.',
            N'Comercializadora Seed',
            NULL,
            '2017-06-30',
            1,
            1,
            @now
        );
    END;

    DECLARE @idTercero1 BIGINT = (
        SELECT TOP (1) id_tercero
        FROM tercero.tercero
        WHERE codigo = N'TER-001'
    );

    DECLARE @idTercero2 BIGINT = (
        SELECT TOP (1) id_tercero
        FROM tercero.tercero
        WHERE codigo = N'TER-002'
    );

    ------------------------------------------------------------
    -- 3) Datos relacionados
    ------------------------------------------------------------
    IF NOT EXISTS (
        SELECT 1
        FROM tercero.identificacion_tercero
        WHERE id_tercero = @idTercero1
          AND numero_identificacion = N'1234567-8'
    )
    BEGIN
        INSERT INTO tercero.identificacion_tercero
        (
            id_tercero,
            id_tipo_identificacion,
            numero_identificacion,
            fecha_emision,
            fecha_vencimiento,
            principal
        )
        VALUES
        (
            @idTercero1,
            1,
            N'1234567-8',
            '2020-01-01',
            NULL,
            1
        );
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM tercero.direccion_tercero
        WHERE id_tercero = @idTercero1
          AND ISNULL(direccion_linea1, N'') = N'Zona 10, Ciudad de Guatemala'
    )
    BEGIN
        INSERT INTO tercero.direccion_tercero
        (
            id_tercero,
            id_tipo_direccion,
            direccion_linea1,
            direccion_linea2,
            id_pais,
            id_estado,
            id_ciudad,
            codigo_postal,
            principal
        )
        VALUES
        (
            @idTercero1,
            1,
            N'Zona 10, Ciudad de Guatemala',
            N'Edificio Empresarial',
            1,
            1,
            1,
            N'01010',
            1
        );
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM tercero.contacto_tercero
        WHERE id_tercero = @idTercero1
          AND valor = N'carlos.seed@example.com'
    )
    BEGIN
        INSERT INTO tercero.contacto_tercero
        (
            id_tercero,
            id_tipo_contacto,
            valor,
            principal
        )
        VALUES
        (
            @idTercero1,
            1,
            N'carlos.seed@example.com',
            1
        );
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM tercero.cuenta_bancaria_tercero
        WHERE id_tercero = @idTercero2
          AND numero_cuenta = N'010000123456'
    )
    BEGIN
        INSERT INTO tercero.cuenta_bancaria_tercero
        (
            id_tercero,
            id_banco,
            numero_cuenta,
            id_moneda,
            principal
        )
        VALUES
        (
            @idTercero2,
            1,
            N'010000123456',
            1,
            1
        );
    END;

    IF @idRolTerceroDefault IS NOT NULL AND NOT EXISTS (
        SELECT 1
        FROM tercero.tercero_rol
        WHERE id_tercero = @idTercero2
          AND id_rol_tercero = @idRolTerceroDefault
    )
    BEGIN
        INSERT INTO tercero.tercero_rol
        (
            id_tercero,
            id_rol_tercero,
            id_empresa,
            activo
        )
        VALUES
        (
            @idTercero2,
            @idRolTerceroDefault,
            @idEmpresaSeed,
            1
        );
    END;

    ------------------------------------------------------------
    -- 4) Recursos UI y permisos de navegacion
    ------------------------------------------------------------
    MERGE seguridad.permiso AS target
    USING (VALUES
        (N'TER.TERCERO.LISTAR', N'TERCERO', N'LISTAR', N'Listar terceros', N'Permiso para consultar terceros', 0, 1),
        (N'TER.TIPO_PERSONA.LISTAR', N'TERCERO', N'LISTAR', N'Listar tipos persona', N'Permiso para consultar tipos de persona', 0, 1),
        (N'TER.IDENTIFICACION.LISTAR', N'TERCERO', N'LISTAR', N'Listar identificaciones', N'Permiso para consultar identificaciones de tercero', 0, 1),
        (N'TER.DIRECCION.LISTAR', N'TERCERO', N'LISTAR', N'Listar direcciones', N'Permiso para consultar direcciones de tercero', 0, 1),
        (N'TER.CONTACTO.LISTAR', N'TERCERO', N'LISTAR', N'Listar contactos', N'Permiso para consultar contactos de tercero', 0, 1),
        (N'TER.CUENTA.LISTAR', N'TERCERO', N'LISTAR', N'Listar cuentas bancarias', N'Permiso para consultar cuentas de tercero', 0, 1),
        (N'TER.ROL.LISTAR', N'TERCERO', N'LISTAR', N'Listar roles de tercero', N'Permiso para consultar roles de tercero', 0, 1)
    ) AS source (codigo, modulo, accion, nombre, descripcion, es_sensible, activo)
    ON target.codigo = source.codigo
    WHEN MATCHED THEN
        UPDATE SET
            modulo = source.modulo,
            accion = source.accion,
            nombre = source.nombre,
            descripcion = source.descripcion,
            es_sensible = source.es_sensible,
            activo = source.activo,
            actualizado_utc = @now
    WHEN NOT MATCHED THEN
        INSERT (codigo, modulo, accion, nombre, descripcion, es_sensible, activo)
        VALUES (source.codigo, source.modulo, source.accion, source.nombre, source.descripcion, source.es_sensible, source.activo);

    MERGE seguridad.recurso_ui AS target
    USING (VALUES
        (N'NAV.TERCEROS', N'Terceros', @idTipoForm, N'/tercero/terceros', N'FrmTercerosBuscar', N'BusinessObjects.BOPerson', CAST(NULL AS BIGINT), 30, 1, 1),
        (N'NAV.TIPO_PERSONA', N'Tipos Persona', @idTipoForm, N'/tercero/tipo-persona', N'FrmTipoPersonaBuscar', N'BusinessObjects.BOContact', CAST(NULL AS BIGINT), 31, 1, 1),
        (N'NAV.IDENTIFICACION_TERCERO', N'Identificaciones', @idTipoForm, N'/tercero/identificaciones', N'FrmIdentificacionTerceroBuscar', N'BusinessObjects.BOValidation', CAST(NULL AS BIGINT), 32, 1, 1),
        (N'NAV.DIRECCION_TERCERO', N'Direcciones', @idTipoForm, N'/tercero/direcciones', N'FrmDireccionTerceroBuscar', N'BusinessObjects.BOAddress', CAST(NULL AS BIGINT), 33, 1, 1),
        (N'NAV.CONTACTO_TERCERO', N'Contactos', @idTipoForm, N'/tercero/contactos', N'FrmContactoTerceroBuscar', N'BusinessObjects.BOLead', CAST(NULL AS BIGINT), 34, 1, 1),
        (N'NAV.CUENTA_BANCARIA_TERCERO', N'Cuentas Bancarias', @idTipoForm, N'/tercero/cuentas', N'FrmCuentaBancariaTerceroBuscar', N'BusinessObjects.BOInvoice', CAST(NULL AS BIGINT), 35, 1, 1),
        (N'NAV.TERCERO_ROL', N'Roles de Tercero', @idTipoForm, N'/tercero/roles', N'FrmTerceroRolBuscar', N'BusinessObjects.BORole', CAST(NULL AS BIGINT), 36, 1, 1)
    ) AS source (codigo, nombre, id_tipo_recurso_ui, ruta, componente, icono, id_recurso_ui_padre, orden_visual, es_visible, activo)
    ON target.codigo = source.codigo
    WHEN MATCHED THEN
        UPDATE SET
            nombre = source.nombre,
            id_tipo_recurso_ui = source.id_tipo_recurso_ui,
            ruta = source.ruta,
            componente = source.componente,
            icono = source.icono,
            id_recurso_ui_padre = source.id_recurso_ui_padre,
            orden_visual = source.orden_visual,
            es_visible = source.es_visible,
            activo = source.activo,
            actualizado_utc = @now
    WHEN NOT MATCHED THEN
        INSERT
        (
            codigo,
            nombre,
            id_tipo_recurso_ui,
            ruta,
            componente,
            icono,
            id_recurso_ui_padre,
            orden_visual,
            es_visible,
            activo
        )
        VALUES
        (
            source.codigo,
            source.nombre,
            source.id_tipo_recurso_ui,
            source.ruta,
            source.componente,
            source.icono,
            source.id_recurso_ui_padre,
            source.orden_visual,
            source.es_visible,
            source.activo
        );

    MERGE seguridad.recurso_ui_permiso AS target
    USING (
        SELECT r.id_recurso_ui, p.id_permiso
        FROM seguridad.recurso_ui r
        INNER JOIN seguridad.permiso p
            ON (
                (r.codigo = N'NAV.TERCEROS' AND p.codigo = N'TER.TERCERO.LISTAR')
                OR (r.codigo = N'NAV.TIPO_PERSONA' AND p.codigo = N'TER.TIPO_PERSONA.LISTAR')
                OR (r.codigo = N'NAV.IDENTIFICACION_TERCERO' AND p.codigo = N'TER.IDENTIFICACION.LISTAR')
                OR (r.codigo = N'NAV.DIRECCION_TERCERO' AND p.codigo = N'TER.DIRECCION.LISTAR')
                OR (r.codigo = N'NAV.CONTACTO_TERCERO' AND p.codigo = N'TER.CONTACTO.LISTAR')
                OR (r.codigo = N'NAV.CUENTA_BANCARIA_TERCERO' AND p.codigo = N'TER.CUENTA.LISTAR')
                OR (r.codigo = N'NAV.TERCERO_ROL' AND p.codigo = N'TER.ROL.LISTAR')
            )
    ) AS source (id_recurso_ui, id_permiso)
    ON target.id_recurso_ui = source.id_recurso_ui
       AND target.id_permiso = source.id_permiso
    WHEN MATCHED THEN
        UPDATE SET activo = 1
    WHEN NOT MATCHED THEN
        INSERT (id_recurso_ui, id_permiso, activo)
        VALUES (source.id_recurso_ui, source.id_permiso, 1);

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;

