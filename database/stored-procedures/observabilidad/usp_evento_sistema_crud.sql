CREATE OR ALTER PROCEDURE observabilidad.usp_evento_sistema_listar
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_evento_sistema], [correlation_id], [tipo_evento], [descripcion], [usuario], [fecha]
    FROM observabilidad.evento_sistema;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_evento_sistema_obtener
    @id_evento_sistema bigint
AS
BEGIN
    SET NOCOUNT ON;
    SELECT [id_evento_sistema], [correlation_id], [tipo_evento], [descripcion], [usuario], [fecha]
    FROM observabilidad.evento_sistema
    WHERE [id_evento_sistema] = @id_evento_sistema;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_evento_sistema_crear
    @correlation_id uniqueidentifier,
    @tipo_evento varchar(200),
    @descripcion varchar(1000),
    @usuario varchar(150),
    @fecha datetime2
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO observabilidad.evento_sistema ([correlation_id], [tipo_evento], [descripcion], [usuario], [fecha])
    VALUES (@correlation_id, @tipo_evento, @descripcion, @usuario, @fecha);
    SELECT CAST(SCOPE_IDENTITY() AS bigint) AS id;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_evento_sistema_actualizar
    @id_evento_sistema bigint,
    @correlation_id uniqueidentifier,
    @tipo_evento varchar(200),
    @descripcion varchar(1000),
    @usuario varchar(150),
    @fecha datetime2
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE observabilidad.evento_sistema
    SET [correlation_id] = @correlation_id,
        [tipo_evento] = @tipo_evento,
        [descripcion] = @descripcion,
        [usuario] = @usuario,
        [fecha] = @fecha
    WHERE [id_evento_sistema] = @id_evento_sistema;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO

CREATE OR ALTER PROCEDURE observabilidad.usp_evento_sistema_desactivar
    @id_evento_sistema bigint,
    @usuario varchar(180) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM observabilidad.evento_sistema
    WHERE [id_evento_sistema] = @id_evento_sistema;
    SELECT @@ROWCOUNT AS filas_afectadas;
END
GO
