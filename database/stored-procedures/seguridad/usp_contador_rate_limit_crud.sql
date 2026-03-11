CREATE OR ALTER PROCEDURE seguridad.usp_contador_rate_limit_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_contador_rate_limit], [id_tenant], [id_empresa], [ambito], [llave], [endpoint], [inicio_ventana_utc], [conteo]
    FROM seguridad.contador_rate_limit;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_contador_rate_limit_obtener
    @id_contador_rate_limit bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_contador_rate_limit], [id_tenant], [id_empresa], [ambito], [llave], [endpoint], [inicio_ventana_utc], [conteo]
    FROM seguridad.contador_rate_limit
    WHERE [id_contador_rate_limit] = @id_contador_rate_limit;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_contador_rate_limit_crear
    @id_tenant bigint,
    @id_empresa bigint,
    @ambito nvarchar(20),
    @llave nvarchar(200),
    @endpoint nvarchar(200),
    @inicio_ventana_utc datetime2,
    @conteo int
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO seguridad.contador_rate_limit ([id_tenant], [id_empresa], [ambito], [llave], [endpoint], [inicio_ventana_utc], [conteo])
    VALUES (@id_tenant, @id_empresa, @ambito, @llave, @endpoint, @inicio_ventana_utc, @conteo);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_contador_rate_limit_actualizar
    @id_contador_rate_limit bigint,
    @id_tenant bigint,
    @id_empresa bigint,
    @ambito nvarchar(20),
    @llave nvarchar(200),
    @endpoint nvarchar(200),
    @inicio_ventana_utc datetime2,
    @conteo int
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE seguridad.contador_rate_limit
    SET [id_tenant] = @id_tenant,
        [id_empresa] = @id_empresa,
        [ambito] = @ambito,
        [llave] = @llave,
        [endpoint] = @endpoint,
        [inicio_ventana_utc] = @inicio_ventana_utc,
        [conteo] = @conteo
    WHERE [id_contador_rate_limit] = @id_contador_rate_limit;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE seguridad.usp_contador_rate_limit_desactivar
    @id_contador_rate_limit bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM seguridad.contador_rate_limit
    WHERE [id_contador_rate_limit] = @id_contador_rate_limit;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
