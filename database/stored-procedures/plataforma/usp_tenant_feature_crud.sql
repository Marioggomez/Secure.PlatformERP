CREATE OR ALTER PROCEDURE plataforma.usp_tenant_feature_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tenant_feature], [id_tenant], [id_feature], [activo]
    FROM plataforma.tenant_feature;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_tenant_feature_obtener
    @id_tenant_feature bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_tenant_feature], [id_tenant], [id_feature], [activo]
    FROM plataforma.tenant_feature
    WHERE [id_tenant_feature] = @id_tenant_feature;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_tenant_feature_crear
    @id_tenant bigint,
    @id_feature bigint,
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO plataforma.tenant_feature ([id_tenant], [id_feature], [activo])
    VALUES (@id_tenant, @id_feature, @activo);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_tenant_feature_actualizar
    @id_tenant_feature bigint,
    @id_tenant bigint,
    @id_feature bigint,
    @activo bit
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.tenant_feature
    SET [id_tenant] = @id_tenant,
        [id_feature] = @id_feature,
        [activo] = @activo
    WHERE [id_tenant_feature] = @id_tenant_feature;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE plataforma.usp_tenant_feature_desactivar
    @id_tenant_feature bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE plataforma.tenant_feature
    SET [activo] = 0
    WHERE [id_tenant_feature] = @id_tenant_feature;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
