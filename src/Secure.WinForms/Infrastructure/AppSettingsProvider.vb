Imports System.IO
Imports System.Text.Json

Namespace Infrastructure
    ''' <summary>
    ''' Cargador de configuracion local del cliente WinForms.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public NotInheritable Class AppSettingsProvider
        Private Sub New()
        End Sub

        Public Shared Function GetApiBaseUrl() As String
            Dim fromEnv = Environment.GetEnvironmentVariable("SECURE_API_BASE_URL")
            If Not String.IsNullOrWhiteSpace(fromEnv) Then
                Return fromEnv.Trim()
            End If

            Dim value = GetSettingValue({"Api", "BaseUrl"})
            Return If(String.IsNullOrWhiteSpace(value), "http://localhost:5236/", value)
        End Function

        Public Shared Function GetReportCompanyName() As String
            Dim value = GetSettingValue({"Reporting", "CompanyName"})
            Return If(String.IsNullOrWhiteSpace(value), "Secure Platform ERP", value)
        End Function

        Public Shared Function GetReportLogoPath() As String
            Dim value = GetSettingValue({"Reporting", "LogoPath"})
            If String.IsNullOrWhiteSpace(value) Then
                Return String.Empty
            End If

            If System.IO.Path.IsPathRooted(value) Then
                Return value
            End If

            Return System.IO.Path.Combine(AppContext.BaseDirectory, value)
        End Function

        Private Shared Function GetSettingValue(ByVal segments As IEnumerable(Of String)) As String
            Dim appSettingsPath = System.IO.Path.Combine(AppContext.BaseDirectory, "appsettings.json")
            If Not File.Exists(appSettingsPath) Then
                Return String.Empty
            End If

            Try
                Dim json = File.ReadAllText(appSettingsPath)
                Using doc = JsonDocument.Parse(json)
                    Dim current = doc.RootElement
                    For Each segment In segments
                        Dim nextElement As JsonElement
                        If Not current.TryGetProperty(segment, nextElement) Then
                            Return String.Empty
                        End If

                        current = nextElement
                    Next

                    If current.ValueKind = JsonValueKind.String Then
                        Return If(current.GetString(), String.Empty).Trim()
                    End If
                End Using
            Catch
                Return String.Empty
            End Try

            Return String.Empty
        End Function
    End Class
End Namespace
