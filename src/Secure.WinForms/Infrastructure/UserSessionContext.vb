Namespace Infrastructure
    ''' <summary>
    ''' Contexto de sesion autenticada para consumo de formularios ERP.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class UserSessionContext
        Public Property Usuario As String = String.Empty
        Public Property IdUsuario As Long?
        Public Property IdTenant As Long?
        Public Property IdEmpresa As Long?

        Public ReadOnly Property TieneIdentidadValida As Boolean
            Get
                Return IdUsuario.HasValue AndAlso IdTenant.HasValue
            End Get
        End Property

        Public Shared Function CrearDiseno() As UserSessionContext
            Return New UserSessionContext With {
                .Usuario = "Usuario diseño",
                .IdUsuario = 0,
                .IdTenant = 0,
                .IdEmpresa = 0
            }
        End Function
    End Class
End Namespace
