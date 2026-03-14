Imports DevExpress.XtraEditors
Imports DevExpress.XtraLayout
Imports Secure.Platform.WinForms.Infrastructure
Imports System.ComponentModel

Namespace Forms.Shell
    ''' <summary>
    ''' Modulo de apariencia y herramientas visuales del ERP.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class FrmAparienciaHerramientas
        Inherits XtraForm

        Private Const NoPaletteText As String = "(Sin paletas para este skin)"

        Private ReadOnly _listTemas As ListBoxControl
        Private ReadOnly _listPaletas As ListBoxControl
        Private ReadOnly _btnAplicar As SimpleButton
        Private ReadOnly _btnRestablecer As SimpleButton
        Private ReadOnly _chkRoundedCorners As CheckEdit
        Private ReadOnly _chkCompactMode As CheckEdit
        Private ReadOnly _lblTemaActual As LabelControl
        Private ReadOnly _lblPaletteActual As LabelControl
        Private _loading As Boolean

        Public Sub New()
            _listTemas = New ListBoxControl()
            _listPaletas = New ListBoxControl()
            _btnAplicar = New SimpleButton()
            _btnRestablecer = New SimpleButton()
            _chkRoundedCorners = New CheckEdit()
            _chkCompactMode = New CheckEdit()
            _lblTemaActual = New LabelControl()
            _lblPaletteActual = New LabelControl()

            InitializeComponent()

            If LicenseManager.UsageMode = LicenseUsageMode.Designtime Then
                Return
            End If

            LoadThemes()
            WireEvents()
        End Sub

        Private Sub InitializeComponent()
            Text = "Apariencia y herramientas"
            FormBorderStyle = FormBorderStyle.None
            Dock = DockStyle.Fill

            Dim mainLayout As New LayoutControl() With {
                .Dock = DockStyle.Fill
            }
            Controls.Add(mainLayout)

            Dim lblTitulo As New LabelControl() With {
                .Text = "Personalizacion visual del ERP"
            }
            lblTitulo.Appearance.Font = New Font("Segoe UI", 18.0F, FontStyle.Bold)

            Dim lblHelp As New LabelControl() With {
                .Text = "Seleccione skin + paleta. Tambien puede activar esquinas redondeadas y modo compacto."
            }
            lblHelp.Appearance.Font = New Font("Segoe UI", 10.0F)
            lblHelp.Appearance.ForeColor = Color.DimGray

            _listTemas.Height = 260
            _listPaletas.Height = 260

            _chkRoundedCorners.Text = "Esquinas redondeadas (Windows 11)"
            _chkCompactMode.Text = "Modo compacto"

            _lblTemaActual.Appearance.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
            _lblTemaActual.Appearance.ForeColor = Color.FromArgb(40, 80, 120)

            _lblPaletteActual.Appearance.Font = New Font("Segoe UI", 9.5F, FontStyle.Bold)
            _lblPaletteActual.Appearance.ForeColor = Color.FromArgb(40, 80, 120)

            _btnAplicar.Text = "Aplicar apariencia"
            _btnAplicar.Height = 40

            _btnRestablecer.Text = "Restablecer"
            _btnRestablecer.Height = 40

            mainLayout.Controls.Add(lblTitulo)
            mainLayout.Controls.Add(lblHelp)
            mainLayout.Controls.Add(_lblTemaActual)
            mainLayout.Controls.Add(_lblPaletteActual)
            mainLayout.Controls.Add(_listTemas)
            mainLayout.Controls.Add(_listPaletas)
            mainLayout.Controls.Add(_chkRoundedCorners)
            mainLayout.Controls.Add(_chkCompactMode)
            mainLayout.Controls.Add(_btnAplicar)
            mainLayout.Controls.Add(_btnRestablecer)

            Dim root As New LayoutControlGroup()
            root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True
            root.GroupBordersVisible = False
            mainLayout.Root = root

            Dim itemTitulo = root.AddItem(String.Empty, lblTitulo)
            itemTitulo.TextVisible = False
            itemTitulo.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 18, 4)

            Dim itemHelp = root.AddItem(String.Empty, lblHelp)
            itemHelp.TextVisible = False
            itemHelp.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 2, 12)

            Dim itemTemaActual = root.AddItem(String.Empty, _lblTemaActual)
            itemTemaActual.TextVisible = False
            itemTemaActual.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 2, 4)

            Dim itemPaletteActual = root.AddItem(String.Empty, _lblPaletteActual)
            itemPaletteActual.TextVisible = False
            itemPaletteActual.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 2, 8)

            Dim groupListas As New LayoutControlGroup() With {
                .Text = "Skins y paletas"
            }
            root.Add(groupListas)

            Dim itemTemas = groupListas.AddItem("Skins", _listTemas)
            itemTemas.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 8, 8)

            Dim itemPaletas = groupListas.AddItem("Paletas", _listPaletas)
            itemPaletas.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 8, 8)

            Dim splitter As New SplitterItem() With {
                .AllowHotTrack = True
            }
            groupListas.AddItem(splitter, itemTemas, DevExpress.XtraLayout.Utils.InsertType.Right)

            Dim groupOpciones As New LayoutControlGroup() With {
                .Text = "Opciones globales"
            }
            root.Add(groupOpciones)

            Dim itemRounded = groupOpciones.AddItem(String.Empty, _chkRoundedCorners)
            itemRounded.TextVisible = False
            itemRounded.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 6, 4)

            Dim itemCompact = groupOpciones.AddItem(String.Empty, _chkCompactMode)
            itemCompact.TextVisible = False
            itemCompact.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 4, 6)

            Dim groupAcciones As New LayoutControlGroup() With {
                .Text = "Acciones"
            }
            root.Add(groupAcciones)

            Dim itemAplicar = groupAcciones.AddItem(String.Empty, _btnAplicar)
            itemAplicar.TextVisible = False
            itemAplicar.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 8, 6)

            Dim itemRestablecer = groupAcciones.AddItem(String.Empty, _btnRestablecer)
            itemRestablecer.TextVisible = False
            itemRestablecer.Padding = New DevExpress.XtraLayout.Utils.Padding(8, 8, 6, 8)
        End Sub

        Private Sub WireEvents()
            AddHandler _btnAplicar.Click, AddressOf OnAplicarClick
            AddHandler _btnRestablecer.Click, AddressOf OnRestablecerClick
            AddHandler _listTemas.SelectedIndexChanged, AddressOf OnTemaChanged
            AddHandler _listTemas.DoubleClick, AddressOf OnAplicarClick
            AddHandler _listPaletas.DoubleClick, AddressOf OnAplicarClick
        End Sub

        Private Sub LoadThemes()
            _loading = True
            Try
                _listTemas.Items.Clear()
                For Each theme In ThemeService.GetAvailableThemes()
                    _listTemas.Items.Add(theme)
                Next

                Dim currentTheme = ThemeService.GetCurrentTheme()
                Dim themeIndex = FindItemIndex(_listTemas, currentTheme)
                If themeIndex >= 0 Then
                    _listTemas.SelectedIndex = themeIndex
                ElseIf _listTemas.Items.Count > 0 Then
                    _listTemas.SelectedIndex = 0
                End If

                _chkRoundedCorners.Checked = ThemeService.GetCurrentRoundedCornersEnabled()
                _chkCompactMode.Checked = ThemeService.GetCurrentCompactModeEnabled()
                ReloadPalettes()
                UpdateCurrentLabel()
            Finally
                _loading = False
            End Try
        End Sub

        Private Sub ReloadPalettes()
            _listPaletas.Items.Clear()

            Dim selectedTheme = TryCast(_listTemas.SelectedItem, String)
            If String.IsNullOrWhiteSpace(selectedTheme) Then
                _listPaletas.Enabled = False
                _listPaletas.Items.Add(NoPaletteText)
                _listPaletas.SelectedIndex = 0
                Return
            End If

            Dim palettes = ThemeService.GetAvailablePalettes(selectedTheme)
            If palettes.Count = 0 Then
                _listPaletas.Enabled = False
                _listPaletas.Items.Add(NoPaletteText)
                _listPaletas.SelectedIndex = 0
                Return
            End If

            _listPaletas.Enabled = True
            For Each palette In palettes
                _listPaletas.Items.Add(palette)
            Next

            Dim currentPalette = ThemeService.GetCurrentPalette()
            Dim selectedIndex = FindItemIndex(_listPaletas, currentPalette)
            If selectedIndex < 0 Then
                selectedIndex = FindItemIndex(_listPaletas, ThemeService.DefaultPaletteName)
            End If

            If selectedIndex < 0 AndAlso _listPaletas.Items.Count > 0 Then
                selectedIndex = 0
            End If

            If selectedIndex >= 0 Then
                _listPaletas.SelectedIndex = selectedIndex
            End If
        End Sub

        Private Sub UpdateCurrentLabel()
            Dim actualTheme = ThemeService.GetCurrentTheme()
            Dim actualPalette = ThemeService.GetCurrentPalette()

            _lblTemaActual.Text = $"Skin actual: {actualTheme}"
            _lblPaletteActual.Text = $"Paleta actual: {If(String.IsNullOrWhiteSpace(actualPalette), ThemeService.DefaultPaletteName, actualPalette)}"
        End Sub

        Private Sub OnTemaChanged(ByVal sender As Object, ByVal e As EventArgs)
            If _loading Then
                Return
            End If

            ReloadPalettes()
        End Sub

        Private Sub OnAplicarClick(ByVal sender As Object, ByVal e As EventArgs)
            If _listTemas.SelectedItem Is Nothing Then
                XtraMessageBox.Show(Me, "Seleccione un skin para aplicar.", "Apariencia", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim selectedTheme = _listTemas.SelectedItem.ToString()
            Dim selectedPalette As String = Nothing
            If _listPaletas.Enabled AndAlso _listPaletas.SelectedItem IsNot Nothing Then
                selectedPalette = _listPaletas.SelectedItem.ToString()
            End If

            ThemeService.ApplyTheme(
                selectedTheme,
                selectedPalette,
                roundedCornersEnabled:=_chkRoundedCorners.Checked,
                compactModeEnabled:=_chkCompactMode.Checked)

            UpdateCurrentLabel()
        End Sub

        Private Sub OnRestablecerClick(ByVal sender As Object, ByVal e As EventArgs)
            ThemeService.ApplyTheme(
                "WXI",
                ThemeService.DefaultPaletteName,
                roundedCornersEnabled:=True,
                compactModeEnabled:=False)

            LoadThemes()
        End Sub

        Private Shared Function FindItemIndex(ByVal list As ListBoxControl, ByVal target As String) As Integer
            If list Is Nothing OrElse list.Items Is Nothing OrElse String.IsNullOrWhiteSpace(target) Then
                Return -1
            End If

            For i = 0 To list.Items.Count - 1
                Dim itemText = list.Items(i)?.ToString()
                If String.Equals(itemText, target, StringComparison.OrdinalIgnoreCase) Then
                    Return i
                End If
            Next

            Return -1
        End Function
    End Class
End Namespace
