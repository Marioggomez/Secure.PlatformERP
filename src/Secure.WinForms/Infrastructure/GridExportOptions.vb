Namespace Infrastructure
    ''' <summary>
    ''' Formatos soportados para exportacion de grilla.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Enum GridExportFormat
        Excel2003
        Excel2007Plus
        Word
        TextoPlano
        Pdf
        VistaPrevia
    End Enum

    ''' <summary>
    ''' Opciones seleccionadas por el usuario para exportar datos.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class GridExportOptions
        Public Property NombreReporte As String = "Reporte"
        Public Property Formato As GridExportFormat = GridExportFormat.VistaPrevia
    End Class
End Namespace
