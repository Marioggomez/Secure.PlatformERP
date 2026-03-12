/*
    Migracion: 20260312141850__jerarquia_menu_recurso_ui
    Autor: Mario Gomez
    Fecha UTC: 2026-03-12 14:18:50
*/
SET NOCOUNT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @now DATETIME2(3) = SYSUTCDATETIME();

    UPDATE seguridad.recurso_ui
    SET
        id_recurso_ui_padre = NULL,
        actualizado_utc = @now
    WHERE codigo IN
    (
        N'NAV.TIPO_PERSONA',
        N'NAV.IDENTIFICACION_TERCERO',
        N'NAV.DIRECCION_TERCERO',
        N'NAV.CONTACTO_TERCERO',
        N'NAV.CUENTA_BANCARIA_TERCERO',
        N'NAV.TERCERO_ROL'
    )
      AND id_recurso_ui_padre IS NOT NULL;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
