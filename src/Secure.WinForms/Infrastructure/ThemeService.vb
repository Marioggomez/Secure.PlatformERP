Imports DevExpress.LookAndFeel
Imports DevExpress.Skins
Imports DevExpress.Utils
Imports DevExpress.XtraEditors
Imports System.IO
Imports System.Reflection
Imports System.Text.Json

Namespace Infrastructure
    ''' <summary>
    ''' Servicio de apariencia y persistencia de tema de usuario.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public NotInheritable Class ThemeService
        Public Const DefaultPaletteName As String = "DefaultSkinPalette"

        Private Shared ReadOnly _prefsFile As String = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Secure.Platform",
            "ui-preferences.json")

        Private Shared _suppressPersist As Boolean

        Private NotInheritable Class UiPreferences
            Public Property Theme As String = String.Empty
            Public Property Palette As String = String.Empty
            Public Property RoundedCorners As Boolean = True
            Public Property CompactMode As Boolean = False
        End Class

        Private Sub New()
        End Sub

        Public Shared Sub Initialize()
            Dim prefs = LoadPreferences()
            ApplyPreferences(prefs, persistChanges:=False)
            AddHandler UserLookAndFeel.Default.StyleChanged, AddressOf OnStyleChanged
        End Sub

        Public Shared Function GetCurrentTheme() As String
            Return UserLookAndFeel.Default.ActiveSkinName
        End Function

        Public Shared Function GetCurrentPalette() As String
            Return UserLookAndFeel.Default.ActiveSvgPaletteName
        End Function

        Public Shared Function GetCurrentRoundedCornersEnabled() As Boolean
            Return WindowsFormsSettings.GetAllowRoundedWindowCorners()
        End Function

        Public Shared Function GetCurrentCompactModeEnabled() As Boolean
            Return WindowsFormsSettings.CompactUIMode = DefaultBoolean.True
        End Function

        Public Shared Function GetAvailableThemes() As IReadOnlyList(Of String)
            Dim results As New List(Of String)()
            For Each container As SkinContainer In SkinManager.Default.Skins
                If container IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(container.SkinName) Then
                    results.Add(container.SkinName)
                End If
            Next

            Return results.
                Distinct(StringComparer.OrdinalIgnoreCase).
                OrderBy(Function(x) x).
                ToList()
        End Function

        Public Shared Function GetAvailablePalettes(ByVal themeName As String) As IReadOnlyList(Of String)
            Dim palettes As New List(Of String)()
            If String.IsNullOrWhiteSpace(themeName) Then
                Return palettes
            End If

            Dim paletteType = ResolvePaletteOwnerType(themeName)
            If paletteType Is Nothing Then
                Return palettes
            End If

            Try
                Dim ctor = paletteType.GetConstructors(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance).
                    FirstOrDefault(Function(x) x.GetParameters().Length = 1)
                If ctor Is Nothing Then
                    Return palettes
                End If

                Dim owner = ctor.Invoke(New Object() {themeName})
                Dim paletteSetProperty = paletteType.GetProperty("PaletteSet", BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
                If paletteSetProperty Is Nothing Then
                    Return palettes
                End If

                Dim paletteSet = TryCast(paletteSetProperty.GetValue(owner, Nothing), IDictionary(Of String, SkinSvgPalette))
                If paletteSet Is Nothing Then
                    Return palettes
                End If

                palettes.AddRange(paletteSet.Keys.Where(Function(x) Not String.IsNullOrWhiteSpace(x)))
            Catch
                Return New List(Of String)()
            End Try

            If Not palettes.Any(Function(x) String.Equals(x, DefaultPaletteName, StringComparison.OrdinalIgnoreCase)) Then
                palettes.Insert(0, DefaultPaletteName)
            End If

            Return palettes.
                Distinct(StringComparer.OrdinalIgnoreCase).
                ToList()
        End Function

        Public Shared Sub ApplyTheme(
            ByVal themeName As String,
            Optional ByVal paletteName As String = Nothing,
            Optional ByVal roundedCornersEnabled As Boolean? = Nothing,
            Optional ByVal compactModeEnabled As Boolean? = Nothing)

            If String.IsNullOrWhiteSpace(themeName) Then
                Return
            End If

            _suppressPersist = True
            Try
                Dim applyPalette = If(paletteName, String.Empty).Trim()
                Dim hasPalette = Not String.IsNullOrWhiteSpace(applyPalette) AndAlso
                    Not String.Equals(applyPalette, DefaultPaletteName, StringComparison.OrdinalIgnoreCase)

                If hasPalette Then
                    Try
                        UserLookAndFeel.Default.SetSkinStyle(themeName, applyPalette)
                    Catch
                        UserLookAndFeel.Default.SetSkinStyle(themeName)
                    End Try
                Else
                    UserLookAndFeel.Default.SetSkinStyle(themeName)
                End If

                If roundedCornersEnabled.HasValue Then
                    WindowsFormsSettings.AllowRoundedWindowCorners = If(roundedCornersEnabled.Value, DefaultBoolean.True, DefaultBoolean.False)
                End If

                If compactModeEnabled.HasValue Then
                    WindowsFormsSettings.CompactUIMode = If(compactModeEnabled.Value, DefaultBoolean.True, DefaultBoolean.False)
                End If
            Finally
                _suppressPersist = False
            End Try

            SavePreferences(ReadCurrentPreferences())
        End Sub

        Private Shared Sub OnStyleChanged(ByVal sender As Object, ByVal e As EventArgs)
            If _suppressPersist Then
                Return
            End If

            SavePreferences(ReadCurrentPreferences())
        End Sub

        Private Shared Function LoadPreferences() As UiPreferences
            Try
                If Not File.Exists(_prefsFile) Then
                    Return New UiPreferences()
                End If

                Dim json = File.ReadAllText(_prefsFile)
                Using doc = JsonDocument.Parse(json)
                    Dim prefs As New UiPreferences()

                    Dim themeElement As JsonElement
                    If doc.RootElement.TryGetProperty("theme", themeElement) Then
                        prefs.Theme = themeElement.GetString()
                    End If

                    Dim paletteElement As JsonElement
                    If doc.RootElement.TryGetProperty("palette", paletteElement) Then
                        prefs.Palette = paletteElement.GetString()
                    End If

                    Dim roundedCornersElement As JsonElement
                    If doc.RootElement.TryGetProperty("roundedCorners", roundedCornersElement) Then
                        Dim parsedRounded As Boolean
                        If roundedCornersElement.ValueKind = JsonValueKind.True OrElse roundedCornersElement.ValueKind = JsonValueKind.False Then
                            prefs.RoundedCorners = roundedCornersElement.GetBoolean()
                        ElseIf Boolean.TryParse(roundedCornersElement.GetString(), parsedRounded) Then
                            prefs.RoundedCorners = parsedRounded
                        End If
                    End If

                    Dim compactModeElement As JsonElement
                    If doc.RootElement.TryGetProperty("compactMode", compactModeElement) Then
                        Dim parsedCompact As Boolean
                        If compactModeElement.ValueKind = JsonValueKind.True OrElse compactModeElement.ValueKind = JsonValueKind.False Then
                            prefs.CompactMode = compactModeElement.GetBoolean()
                        ElseIf Boolean.TryParse(compactModeElement.GetString(), parsedCompact) Then
                            prefs.CompactMode = parsedCompact
                        End If
                    End If

                    Return prefs
                End Using
            Catch
                Return New UiPreferences()
            End Try
        End Function

        Private Shared Sub ApplyPreferences(ByVal prefs As UiPreferences, ByVal persistChanges As Boolean)
            If prefs Is Nothing Then
                prefs = New UiPreferences()
            End If

            _suppressPersist = True
            Try
                If Not String.IsNullOrWhiteSpace(prefs.Theme) Then
                    ApplyTheme(
                        prefs.Theme,
                        prefs.Palette,
                        roundedCornersEnabled:=prefs.RoundedCorners,
                        compactModeEnabled:=prefs.CompactMode)
                Else
                    WindowsFormsSettings.AllowRoundedWindowCorners = If(prefs.RoundedCorners, DefaultBoolean.True, DefaultBoolean.False)
                    WindowsFormsSettings.CompactUIMode = If(prefs.CompactMode, DefaultBoolean.True, DefaultBoolean.False)
                End If
            Finally
                _suppressPersist = False
            End Try

            If persistChanges Then
                SavePreferences(ReadCurrentPreferences())
            End If
        End Sub

        Private Shared Function ReadCurrentPreferences() As UiPreferences
            Return New UiPreferences With {
                .Theme = UserLookAndFeel.Default.ActiveSkinName,
                .Palette = UserLookAndFeel.Default.ActiveSvgPaletteName,
                .RoundedCorners = WindowsFormsSettings.GetAllowRoundedWindowCorners(),
                .CompactMode = (WindowsFormsSettings.CompactUIMode = DefaultBoolean.True)
            }
        End Function

        Private Shared Sub SavePreferences(ByVal prefs As UiPreferences)
            If prefs Is Nothing Then
                Return
            End If

            Try
                Dim folder = Path.GetDirectoryName(_prefsFile)
                If Not String.IsNullOrWhiteSpace(folder) AndAlso Not Directory.Exists(folder) Then
                    Directory.CreateDirectory(folder)
                End If

                Dim payload = JsonSerializer.Serialize(New With {
                    Key .theme = prefs.Theme,
                    Key .palette = prefs.Palette,
                    Key .roundedCorners = prefs.RoundedCorners,
                    Key .compactMode = prefs.CompactMode
                })
                File.WriteAllText(_prefsFile, payload)
            Catch
                ' No interrumpir la ejecucion por errores de persistencia de tema.
            End Try
        End Sub

        Private Shared Function ResolvePaletteOwnerType(ByVal themeName As String) As Type
            If String.IsNullOrWhiteSpace(themeName) Then
                Return Nothing
            End If

            Dim assembly = GetType(UserLookAndFeel).Assembly
            If assembly Is Nothing Then
                Return Nothing
            End If

            Dim candidates As New List(Of String)()
            Dim normalized = themeName.Trim()
            candidates.Add(normalized)
            candidates.Add(normalized.Replace(" ", String.Empty))
            candidates.Add(normalized.Replace(" ", String.Empty).Replace("-", String.Empty))
            If normalized.StartsWith("The ", StringComparison.OrdinalIgnoreCase) Then
                candidates.Add(normalized.Substring(4))
                candidates.Add(normalized.Substring(4).Replace(" ", String.Empty))
            End If

            For Each candidate In candidates.Distinct(StringComparer.OrdinalIgnoreCase)
                Dim fullName = $"DevExpress.LookAndFeel.{candidate}"
                Dim paletteType = assembly.GetType(fullName, throwOnError:=False, ignoreCase:=False)
                If paletteType IsNot Nothing Then
                    Return paletteType
                End If
            Next

            Return Nothing
        End Function
    End Class
End Namespace

