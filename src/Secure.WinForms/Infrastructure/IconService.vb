Imports DevExpress.Images

Namespace Infrastructure
    ''' <summary>
    ''' Servicio centralizado para iconos SVG de DevExpress.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public NotInheritable Class IconService
        Private Sub New()
        End Sub

        ''' <summary>
        ''' Obtiene un icono por clave desde ImageResourceCache.
        ''' </summary>
        Public Shared Function GetIcon(ByVal iconKey As String) As Object
            If String.IsNullOrWhiteSpace(iconKey) Then
                Return Nothing
            End If

            Return ImageResourceCache.Default.GetSvgImage(iconKey)
        End Function
    End Class
End Namespace
