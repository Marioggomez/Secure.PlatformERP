Imports DevExpress.LookAndFeel
Imports System.IO
Imports System.Text.Json

Namespace Infrastructure
    ''' <summary>
    ''' Servicio de apariencia y persistencia de tema de usuario.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public NotInheritable Class ThemeService
        Private Shared ReadOnly _prefsFile As String = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Secure.Platform",
            "ui-preferences.json")

        Private Sub New()
        End Sub

        Public Shared Sub Initialize()
            Dim theme = LoadTheme()
            If Not String.IsNullOrWhiteSpace(theme) Then
                UserLookAndFeel.Default.SetSkinStyle(theme)
            End If

            AddHandler UserLookAndFeel.Default.StyleChanged, AddressOf OnStyleChanged
        End Sub

        Public Shared Function GetCurrentTheme() As String
            Return UserLookAndFeel.Default.ActiveSkinName
        End Function

        Public Shared Sub ApplyTheme(ByVal themeName As String)
            If String.IsNullOrWhiteSpace(themeName) Then
                Return
            End If

            UserLookAndFeel.Default.SetSkinStyle(themeName)
            SaveTheme(themeName)
        End Sub

        Private Shared Sub OnStyleChanged(ByVal sender As Object, ByVal e As EventArgs)
            SaveTheme(UserLookAndFeel.Default.ActiveSkinName)
        End Sub

        Private Shared Function LoadTheme() As String
            Try
                If Not File.Exists(_prefsFile) Then
                    Return String.Empty
                End If

                Dim json = File.ReadAllText(_prefsFile)
                Using doc = JsonDocument.Parse(json)
                    Dim themeElement As JsonElement
                    If doc.RootElement.TryGetProperty("theme", themeElement) Then
                        Return themeElement.GetString()
                    End If
                End Using
            Catch
                Return String.Empty
            End Try

            Return String.Empty
        End Function

        Private Shared Sub SaveTheme(ByVal theme As String)
            Try
                Dim folder = Path.GetDirectoryName(_prefsFile)
                If Not String.IsNullOrWhiteSpace(folder) AndAlso Not Directory.Exists(folder) Then
                    Directory.CreateDirectory(folder)
                End If

                Dim payload = JsonSerializer.Serialize(New With {Key .theme = theme})
                File.WriteAllText(_prefsFile, payload)
            Catch
                ' No interrumpir la ejecucion por errores de persistencia de tema.
            End Try
        End Sub
    End Class
End Namespace
