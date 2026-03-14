Imports System.Net
Imports System.Text.Json
Imports System.Windows.Forms
Imports Secure.Platform.WinForms.Forms.Common

Namespace Infrastructure
    Public NotInheritable Class ApiErrorPresenter
        Private Sub New()
        End Sub

        Public Shared Sub Show(ByVal owner As IWin32Window, ByVal ex As ApiClientException, Optional ByVal contexto As String = "operacion")
            Dim info = BuildInfo(ex, contexto)
            Using dialog As New FrmApiErrorDetails(info.userMessage, info.technicalDetails)
                dialog.ShowDialog(owner)
            End Using
        End Sub

        Public Shared Sub ShowUnexpected(ByVal owner As IWin32Window, ByVal ex As Exception, Optional ByVal contexto As String = "operacion")
            Dim userMessage = $"Ocurrio un error inesperado durante {contexto}. Intente nuevamente y contacte soporte si persiste."
            Dim technicalDetails = $"Tipo: {ex.GetType().FullName}{Environment.NewLine}" &
                                   $"Mensaje: {ex.Message}{Environment.NewLine}" &
                                   $"StackTrace:{Environment.NewLine}{ex.StackTrace}"
            Using dialog As New FrmApiErrorDetails(userMessage, technicalDetails)
                dialog.ShowDialog(owner)
            End Using
        End Sub

        Private Shared Function BuildInfo(ByVal ex As ApiClientException, ByVal contexto As String) As (userMessage As String, technicalDetails As String)
            Dim statusCode = CInt(ex.StatusCode)
            Dim correlation = If(String.IsNullOrWhiteSpace(ex.CorrelationId), ExtractJsonValue(ex.ResponseBody, "correlationId"), ex.CorrelationId)
            Dim traceId = ExtractJsonValue(ex.ResponseBody, "traceId")
            Dim apiMessage = ExtractJsonValue(ex.ResponseBody, "mensaje")
            Dim title = ExtractJsonValue(ex.ResponseBody, "title")

            Dim userMessage As String
            If ex.IsConnectivityError Then
                userMessage = "No hay conexion con el servidor API o el servicio esta caido. Verifique red/VPN e intente nuevamente."
            ElseIf statusCode = 401 AndAlso ex.Endpoint.Contains("/seguridad/flujo_autenticacion/iniciar", StringComparison.OrdinalIgnoreCase) Then
                userMessage = "Usuario o contrasena invalida."
            ElseIf statusCode = 403 Then
                userMessage = "No tiene permisos para realizar esta accion."
            ElseIf statusCode = 404 Then
                userMessage = "La operacion solicitada no esta disponible en este momento."
            ElseIf statusCode = 409 Then
                userMessage = "Conflicto de datos. Revise la informacion e intente de nuevo."
            ElseIf statusCode >= 500 Then
                userMessage = "Ocurrio un error interno del servicio. Puede continuar trabajando y reintentar en unos minutos."
            ElseIf Not String.IsNullOrWhiteSpace(apiMessage) Then
                userMessage = apiMessage
            ElseIf Not String.IsNullOrWhiteSpace(title) Then
                userMessage = title
            Else
                userMessage = $"No fue posible completar {contexto}. Revise los datos e intente nuevamente."
            End If

            Dim technicalDetails =
                $"Contexto: {contexto}{Environment.NewLine}" &
                $"Metodo: {If(ex.Method, String.Empty)}{Environment.NewLine}" &
                $"Endpoint: {If(ex.Endpoint, String.Empty)}{Environment.NewLine}" &
                $"HTTP: {statusCode} {ex.StatusCode}{Environment.NewLine}" &
                $"CorrelationId: {If(correlation, String.Empty)}{Environment.NewLine}" &
                $"TraceId: {If(traceId, String.Empty)}{Environment.NewLine}" &
                $"Message: {ex.Message}{Environment.NewLine}" &
                $"Body: {If(ex.ResponseBody, String.Empty)}"

            Return (userMessage, technicalDetails)
        End Function

        Private Shared Function ExtractJsonValue(ByVal json As String, ByVal propertyName As String) As String
            If String.IsNullOrWhiteSpace(json) Then
                Return String.Empty
            End If

            Try
                Using doc = JsonDocument.Parse(json)
                    Dim prop As JsonElement
                    If doc.RootElement.ValueKind = JsonValueKind.Object AndAlso doc.RootElement.TryGetProperty(propertyName, prop) Then
                        Return prop.ToString()
                    End If
                End Using
            Catch
            End Try

            Return String.Empty
        End Function
    End Class
End Namespace
